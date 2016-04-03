using UnityEngine;
using System.Collections;

public class CameraManager : Singleton<CameraManager> {

	Camera gameCamera;
	Camera menuCamera;
	Camera viewportCamera;
	Camera guiCamera;

	HandleTransition transitionHandler;

	public Camera GameCamera {
		get {
			return gameCamera;
		}
	}

	public Camera MenuCamera {
		get {
			return menuCamera;
		}
	}

	public Camera ViewportCamera {
		get {
			return viewportCamera;
		}
	}

	public Camera GUICamera {
		get {
			return guiCamera;
		}
	}

	IEnumerator Start(){
		while (viewportCamera == null){
			yield return null;
		}

		TransitionToMenuView(instant: true);
	}

	public static void RegisterForRole(Camera targetCamera, Enums.CameraRoles role){
		if (role == Enums.CameraRoles.Viewport){
			Instance.viewportCamera = targetCamera;
			Instance.transitionHandler = targetCamera.GetComponentInChildren<HandleTransition>();
		}
		else if (role == Enums.CameraRoles.Gameplay){
			Instance.gameCamera = targetCamera;
		}
		else if (role == Enums.CameraRoles.Menu){
			Instance.menuCamera = targetCamera;
		}
		else if (role == Enums.CameraRoles.GUI){
			Instance.guiCamera = targetCamera;
		}
	}

	public static void TransitionToMenuView(bool instant = false){
		if (Instance == null || Instance.transitionHandler == null){
			Debug.LogError("Tried to transition but Instance or transitionHandler was null.");
			return;
		}

		if (instant){
			Instance.StartSafeCoroutine(Instance.transitionHandler.TransitionToB(time: 0));
		}
		else{
			Instance.StartSafeCoroutine(Instance.transitionHandler.TransitionToB());
		}

	}

	public static void TransitionToGameView(bool instant = false){
		if (Instance == null || Instance.transitionHandler == null){
			Debug.LogError("Tried to transition but Instance or transitionHandler was null.");
			return;
		}

		if (instant){
			Instance.StartSafeCoroutine(Instance.transitionHandler.TransitionToA(time: 0));
		}
		else{
			Instance.StartSafeCoroutine(Instance.transitionHandler.TransitionToA());
		}

	}

}
