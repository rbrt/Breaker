using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;
using System.Linq;

public class TransitionRig : MonoBehaviour
{

	static TransitionRig instance;

	public static TransitionRig Instance
	{
		get
		{
			return instance;
		}
	}

	[SerializeField] protected GameObject gameplayTransitionRig;
	[SerializeField] protected GameObject menuTransitionRig;

    [SerializeField] protected Camera gameplayUI;
	[SerializeField] protected Camera gameplayGame;
	[SerializeField] protected Camera menuUI;
	[SerializeField] protected Camera viewportCamera;

	HandleTransition transitionHandler;

	bool renderTransition = false;
	bool flipTransitionFlag = false;

	public Camera GameplayUICamera
	{
		get
		{
			return gameplayUI;
		}
	}

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			transitionHandler = GetComponentInChildren<HandleTransition>();
		}
		else
		{
			Destroy(this.gameObject);
			Debug.Log("Destroyed duplicate instance of TransitionRig");
		}
	}

	void Update()
	{
		if (renderTransition)
		{
			menuUI.Render();
			gameplayUI.Render();
			gameplayGame.Render();
		}
	}

	void DisableChildren()
	{
		gameplayTransitionRig.SetActive(false);
		menuTransitionRig.SetActive(false);
		viewportCamera.gameObject.SetActive(false);
	}

	void EnableChildren()
	{
		gameplayTransitionRig.SetActive(true);
		menuTransitionRig.SetActive(true);
		viewportCamera.gameObject.SetActive(true);
	}

	Camera GetChildCameraForRole(GameObject parent, Enums.CameraRoles role)
	{
		return parent.GetComponentsInChildren<CameraRole>()
					 .FirstOrDefault(child => child.role == role)
					 .GetComponent<Camera>();
	}

	public void TransitionFromGameplayToEndOfRound()
	{
		LoadingController.LoadEndOfRoundScene();
		this.StartSafeCoroutine(TransitionHandoff(GUIController.Instance.GameplayCanvas,
												  GUIController.Instance.EndOfLevelCanvas,
												  menuUI,
												  gameplayUI,
												  gameplayGame,
												  () => CameraManager.Instance.ViewportCamera,
												  () => LoadingController.UnloadGameplayScene()));
	}

	public void TransitionFromEndOfRoundToGameplay()
	{
		LoadingController.LoadGameplayScene();
		this.StartSafeCoroutine(SetActiveSceneWhenReady());
		this.StartSafeCoroutine(TransitionHandoff(GUIController.Instance.EndOfLevelCanvas,
												  GUIController.Instance.GameplayCanvas,
												  menuUI,
												  gameplayUI,
												  gameplayGame,
												  () => CameraManager.Instance.GameCamera,
												  () => {}));

		LevelController.Instance.RoundOver = false;
	}

	public void TransitionFromMenuToGameplay()
	{
		LoadingController.LoadGameplayScene(additive: true);
		this.StartSafeCoroutine(SetActiveSceneWhenReady());
		this.StartSafeCoroutine(TransitionHandoff(GUIController.Instance.TitleCanvas,
												  GUIController.Instance.GameplayCanvas,
												  menuUI,
												  gameplayUI,
												  gameplayGame,
												  () => CameraManager.Instance.GameCamera,
												  () => {}));
	}

	IEnumerator SetActiveSceneWhenReady()
	{
		yield return this.StartSafeCoroutine(LoadingController.SetGameplaySceneActiveWhenLoaded());
	}

	IEnumerator TransitionHandoff(Canvas fromCanvas,
								  Canvas toCanvas,
								  Camera fromCamera,
								  Camera toUICamera,
								  Camera toViewCamera,
								  System.Func<Camera> toCameraGetter,
								  System.Action cleanupFunction)
	{
		EnableChildren();

		Camera toCamera = toCameraGetter.Invoke();
		while (toCamera == null || toCameraGetter.Invoke() == null)
		{
			toCamera = toCameraGetter.Invoke();
			yield return null;
		}

		renderTransition = true;

		yield return this.StartSafeCoroutine(SetUpGameTransitionElements(toUICamera, toViewCamera, toCamera, toCanvas));

		var rt = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
        rt.Create();

		fromCamera.targetTexture = rt;
		transitionHandler.SetMenuTexture(rt);

		yield return new WaitForEndOfFrame();

		fromCanvas.worldCamera = fromCamera;
	    viewportCamera.enabled = true;

		yield return this.StartSafeCoroutine(transitionHandler.TransitionToA(time: 1.5f));
		yield return new WaitForEndOfFrame();

		gameplayUI.enabled = false;
		gameplayGame.enabled = false;
		fromCamera.enabled = false;
		viewportCamera.enabled = false;

        if (MenuTransitionSetup.Instance != null)
        {
            Destroy(MenuTransitionSetup.Instance.gameObject);
        }

		toCanvas.worldCamera = null;
		fromCanvas.gameObject.SetActive(false);

		var eventSystem = GUIController.Instance.GetComponentInChildren<EventSystem>();
		eventSystem.enabled = false;

		yield return null;

		renderTransition = false;

		eventSystem.enabled = true;
		DisableChildren();

		if (cleanupFunction != null)
		{
			cleanupFunction.Invoke();
		}
	}

	IEnumerator SetUpGameTransitionElements(Camera toUICamera, Camera toActionCamera, Camera toCamera, Canvas toCanvas)
	{
		var rt = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
        rt.Create();

		toUICamera.targetTexture = rt;
		toActionCamera.targetTexture = rt;
		transitionHandler.SetGameplayTexture(rt);

		Debug.Log("Set transition handler's B texture to render from camera " + toActionCamera.name, toActionCamera.gameObject);

		this.StartSafeCoroutine(FollowCameras(toActionCamera, toCamera));

		while (GUIController.Instance == null)
		{
			yield return null;
		}

		yield return new WaitForEndOfFrame();
		toCanvas.worldCamera = toActionCamera;
		toCanvas.planeDistance = 1;
	}

	IEnumerator FollowCameras(Camera transitionCamera, Camera gameCamera)
	{
		while (true){
			if (transitionCamera == null || gameCamera == null){
				break;
			}
			transitionCamera.transform.position = gameCamera.transform.position;
			yield return null;
		}
	}
}
