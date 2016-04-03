using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;
using System.Linq;

public class TransitionRig : MonoBehaviour {

	static TransitionRig instance;

	public static TransitionRig Instance {
		get {
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

	public Camera GameplayUICamera {
		get {
			return gameplayUI;
		}
	}

	void Awake(){
		if (instance == null){
			instance = this;
			transitionHandler = GetComponentInChildren<HandleTransition>();
		}
		else {
			Destroy(this.gameObject);
			Debug.Log("Destroyed duplicate instance of TransitionRig");
		}
	}

	void Update(){
		if (renderTransition){
			menuUI.Render();
			gameplayUI.Render();
			gameplayGame.Render();
		}
	}

	void DisableChildren(){
		gameplayTransitionRig.SetActive(false);
		menuTransitionRig.SetActive(false);
		viewportCamera.gameObject.SetActive(false);
	}

	Camera GetChildCameraForRole(GameObject parent, Enums.CameraRoles role){
		return parent.GetComponentsInChildren<CameraRole>()
					 .FirstOrDefault(child => child.role == role)
					 .GetComponent<Camera>();
	}

	public void TransitionFromGameplayToEndOfRound(){
		// Handle transition here
		LoadingController.LoadEndOfRoundScene();
	}

	public void TransitionFromEndOfRoundToGameplay(){
		// Handle transition here
		LoadingController.LoadGameplayScene();
		this.StartSafeCoroutine(SetActiveSceneWhenReady());
		this.StartSafeCoroutine(EndOfLevelToGameHandoff());

		LevelController.Instance.RoundOver = false;
	}

	public void TransitionFromMenuToGameplay(){
		LoadingController.LoadGameplayScene(additive: true);
		this.StartSafeCoroutine(SetActiveSceneWhenReady());
		this.StartSafeCoroutine(TitleToGameHandoff());
	}

	IEnumerator SetActiveSceneWhenReady(){
		yield return this.StartSafeCoroutine(LoadingController.SetGameplaySceneActiveWhenLoaded());
	}

	IEnumerator EndOfLevelToGameHandoff(){
		while (CameraManager.Instance.GameCamera == null){
			yield return null;
		}

		renderTransition = true;

		yield return this.StartSafeCoroutine(SetUpGameTransitionElements());

		var menuCam = MenuTransitionSetup.Instance.MenuCamera;
		var menuCanvas = MenuTransitionSetup.Instance.MenuCanvas;

		var rt = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
        rt.Create();

		menuUI.targetTexture = rt;
		transitionHandler.SetMenuTexture(rt);

		yield return new WaitForEndOfFrame();
		if (menuCanvas != null){
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

		if (MenuTransitionSetup.Instance != null){
			Destroy(MenuTransitionSetup.Instance.gameObject);
		}

		GUIController.Instance.GetComponent<Canvas>().worldCamera = null;

		var eventSystem = GUIController.Instance.GetComponentInChildren<EventSystem>();
		eventSystem.enabled = false;

		yield return null;

		renderTransition = false;

		eventSystem.enabled = true;
		DisableChildren();
	}

	IEnumerator TitleToGameHandoff(){
		while (CameraManager.Instance.GameCamera == null){
			yield return null;
		}

		renderTransition = true;

		yield return this.StartSafeCoroutine(SetUpGameTransitionElements());

		var menuCam = MenuTransitionSetup.Instance.MenuCamera;
		var menuCanvas = MenuTransitionSetup.Instance.MenuCanvas;

		var rt = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
        rt.Create();

		menuUI.targetTexture = rt;
		transitionHandler.SetMenuTexture(rt);

		yield return new WaitForEndOfFrame();
		menuCanvas.worldCamera = menuUI;
	    viewportCamera.enabled = true;

		yield return this.StartSafeCoroutine(transitionHandler.TransitionToA(time: 1.5f));

		yield return new WaitForEndOfFrame();
		gameplayUI.enabled = false;
		gameplayGame.enabled = false;
		menuUI.enabled = false;
		viewportCamera.enabled = false;

		Destroy(MenuTransitionSetup.Instance.gameObject);

		GUIController.Instance.GetComponent<Canvas>().worldCamera = null;

		var eventSystem = GUIController.Instance.GetComponentInChildren<EventSystem>();
		eventSystem.enabled = false;

		yield return null;

		renderTransition = false;

		eventSystem.enabled = true;
		DisableChildren();
	}

	IEnumerator SetUpGameTransitionElements(){
		var rt = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
        rt.Create();

		gameplayUI.targetTexture = rt;
		gameplayGame.targetTexture = rt;
		transitionHandler.SetGameplayTexture(rt);

		this.StartSafeCoroutine(FollowGameCamera(gameplayGame, CameraManager.Instance.GameCamera));

		while (GUIController.Instance == null){
			yield return null;
		}

		var gameGUICanvas = GUIController.Instance.GUICanvas;

		yield return new WaitForEndOfFrame();
		gameGUICanvas.worldCamera = gameplayGame;
		gameGUICanvas.planeDistance = 1;
	}

	IEnumerator FollowGameCamera(Camera transitionCamera, Camera gameCamera){
		while (true){
			if (transitionCamera == null || gameCamera == null){
				break;
			}
			transitionCamera.transform.position = gameCamera.transform.position;
			yield return null;
		}
	}
}
