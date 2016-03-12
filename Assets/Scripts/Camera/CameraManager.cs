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

	void Awake(){
		if (instance == null){
			instance = this;
		}
		else{
			Destroy(this.gameObject);
			Debug.Log("Destroyed duplicate instance of CameraManager");
		}
	}

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

	public static void RegisterForRole(Camera targetCamera, Enums.CameraRoles role){
		if (role == Enums.CameraRoles.Viewport){
			instance.viewportCamera = targetCamera;
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

}
