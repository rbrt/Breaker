using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	static CameraManager instance;

	public static CameraManager Instance {
		get {
			return instance;
		}
	}

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

	void Awake(){
		if (instance == null){
			instance = this;
		}
		else{
			Destroy(this.gameObject);
			Debug.Log("Destroyed duplicate instance of CameraManager");
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
			instance.viewportCamera = targetCamera;
			instance.transitionHandler = targetCamera.GetComponentInChildren<HandleTransition>();
		}
		else if (role == Enums.CameraRoles.Gameplay){
			instance.gameCamera = targetCamera;
		}
		else if (role == Enums.CameraRoles.Menu){
			instance.menuCamera = targetCamera;
		}
		else if (role == Enums.CameraRoles.GUI){
			instance.guiCamera = targetCamera;
		}
	}

	public static void TransitionToMenuView(bool instant = false){
		if (instance == null || instance.transitionHandler == null){
			Debug.LogError("Tried to transition but instance or transitionHandler was null.");
			return;
		}

		if (instant){
			instance.StartSafeCoroutine(instance.transitionHandler.TransitionToB(time: 0));
		}
		else{
			instance.StartSafeCoroutine(instance.transitionHandler.TransitionToB());
		}

	}

	public static void TransitionToGameView(bool instant = false){
		if (instance == null || instance.transitionHandler == null){
			Debug.LogError("Tried to transition but instance or transitionHandler was null.");
			return;
		}

		if (instant){
			instance.StartSafeCoroutine(instance.transitionHandler.TransitionToA(time: 0));
		}
		else{
			instance.StartSafeCoroutine(instance.transitionHandler.TransitionToA());
		}

	}

}
