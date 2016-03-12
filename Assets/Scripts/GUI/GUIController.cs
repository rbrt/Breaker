using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIController : MonoBehaviour {

	[SerializeField] protected Canvas guiCanvas;

	void Awake () {
		//this.StartSafeCoroutine(WaitForGameplayCameraAndSetCanvas());
	}

	IEnumerator WaitForGameplayCameraAndSetCanvas(){
		while (CameraManager.Instance.GUICamera == null){
			yield return null;
		}

		guiCanvas.worldCamera = CameraManager.Instance.GUICamera;
	}
}
