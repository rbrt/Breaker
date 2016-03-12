using UnityEngine;
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

	Camera gameplayUI;
	Camera gameplayGame;

	Camera menuUI;

	Camera viewportCamera;

	HandleTransition transitionHandler;

	void Awake(){
		if (instance == null){
			instance = this;
			if (gameplayTransitionRig != null){
				gameplayUI = GetChildCameraForRole(gameplayTransitionRig, Enums.CameraRoles.GUI);
				gameplayGame = GetChildCameraForRole(gameplayTransitionRig, Enums.CameraRoles.Gameplay);
			}

			if (menuTransitionRig != null){
				menuUI = GetChildCameraForRole(menuTransitionRig, Enums.CameraRoles.Menu);
			}

			transitionHandler = GetComponentInChildren<HandleTransition>();
			viewportCamera = GetChildCameraForRole(gameObject, Enums.CameraRoles.Viewport);
		}
		else {
			Destroy(this.gameObject);
			Debug.Log("Destroyed duplicate instance of TransitionRig");
		}
	}

	Camera GetChildCameraForRole(GameObject parent, Enums.CameraRoles role){
		return parent.GetComponentsInChildren<CameraRole>()
					 .FirstOrDefault(child => child.role == role)
					 .GetComponent<Camera>();
	}

	public void TransitionFromMenuToGameplay(){
		this.StartSafeCoroutine(Handoff());
	}

	bool renderTransition = false;

	void Update(){
		if (renderTransition){
			menuUI.Render();
			gameplayUI.Render();
			gameplayGame.Render();
		}
	}

	IEnumerator Handoff(){
		var menuCam = MenuTransitionSetup.Instance.MenuCamera;
		var menuCanvas = MenuTransitionSetup.Instance.MenuCanvas;

		renderTransition = true;

		var rt = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
        rt.Create();

		menuUI.targetTexture = rt;
		transitionHandler.SetMenuTexture(rt);

		yield return new WaitForEndOfFrame();
		menuCanvas.worldCamera = menuUI;
	    viewportCamera.enabled = true;
	}
}
