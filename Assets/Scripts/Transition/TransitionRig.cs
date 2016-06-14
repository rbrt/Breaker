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
		this.StartSafeCoroutine(TransitionHandoff(GUIController.Instance.TitleCanvas,
												  GUIController.Instance.GameplayCanvas,
												  menuUI,
												  gameplayUI,
												  gameplayGame,
												  () => CameraManager.Instance.GameCamera,
												  () => LoadingController.UnloadGameplayScene()));
	}

	public void TransitionFromEndOfRoundToGameplay()
	{
		// Handle transition here
		LoadingController.LoadGameplayScene();
		this.StartSafeCoroutine(SetActiveSceneWhenReady());
		this.StartSafeCoroutine(EndOfLevelToGameHandoff());

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

	IEnumerator EndOfLevelToGameHandoff()
	{
		while (CameraManager.Instance.GameCamera == null)
		{
			yield return null;
		}

		EnableChildren();

		renderTransition = true;

		//yield return this.StartSafeCoroutine(SetUpGameTransitionElements());

		var menuCanvas = MenuTransitionSetup.Instance.MenuCanvas;

		var rt = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
        rt.Create();

		menuUI.targetTexture = rt;
		transitionHandler.SetMenuTexture(rt);

		yield return new WaitForEndOfFrame();
		if (menuCanvas != null)
		{
			menuCanvas.worldCamera = menuUI;
		}
	    viewportCamera.enabled = true;

		yield return this.StartSafeCoroutine(transitionHandler.TransitionToA(time: 1.5f));

		yield return new WaitForEndOfFrame();
		gameplayUI.enabled = false;
		gameplayGame.enabled = false;
		menuUI.enabled = false;
		viewportCamera.enabled = false;

		GUIController.Instance.ShowGameplayCanvas();

		if (MenuTransitionSetup.Instance != null)
		{
			Destroy(MenuTransitionSetup.Instance.gameObject);
		}

		GUIController.Instance.EndOfLevelCanvas.worldCamera = null;
		GUIController.Instance.GameplayCanvas.worldCamera = CameraManager.Instance.GameCamera;

		var eventSystem = GUIController.Instance.GetComponentInChildren<EventSystem>();
		eventSystem.enabled = false;

		yield return null;

		renderTransition = false;

		eventSystem.enabled = true;
		DisableChildren();
	}

	IEnumerator TransitionHandoff(Canvas fromCanvas,
								  Canvas toCanvas,
								  Camera fromCamera,
								  Camera toUICamera,
								  Camera toViewCamera,
								  System.Func<Camera> toCameraGetter,
								  System.Action cleanupFunction)
	{
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
