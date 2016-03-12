using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuTransitionSetup : MonoBehaviour {

	static MenuTransitionSetup instance;

	public static MenuTransitionSetup Instance {
		get {
			return instance;
		}
	}

	[SerializeField] protected Canvas menuCanvas;
	[SerializeField] protected Camera menuCamera;

	public Canvas MenuCanvas {
		get {
			return menuCanvas;
		}
	}

	public Camera MenuCamera {
		get {
			return menuCamera;
		}
	}

	void Awake(){
		instance = this;
	}

}
