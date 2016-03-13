using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIController : MonoBehaviour {

	static GUIController instance;

	public static GUIController Instance {
		get {
			return instance;
		}
	}

	[SerializeField] protected Canvas guiCanvas;

	void Awake () {
		this.StartSafeCoroutine(WaitForGameplayCameraAndSetCanvas());
		instance = this;
	}

	IEnumerator WaitForGameplayCameraAndSetCanvas(){
		while (CameraManager.Instance.GUICamera == null){
			yield return null;
		}

		guiCanvas.worldCamera = CameraManager.Instance.GUICamera;
	}
}
