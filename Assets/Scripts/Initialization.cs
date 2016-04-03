using UnityEngine;
using System.Collections;
using System.Linq;

public class Initialization : MonoBehaviour {

	int targetFPS = 60;

	[SerializeField] protected GameObject guiPrefab;

	void Awake(){
		Application.targetFrameRate = targetFPS;
		// var gui = GameObject.Instantiate(guiPrefab);
		//
		// if (TransitionRig.Instance != null){
		// 	gui.GetComponent<Canvas>().worldCamera = TransitionRig.Instance.GameplayUICamera;
		// }
		// else{
		// 	gui.GetComponent<Canvas>().worldCamera = FindObjectsOfType<CameraRole>()
		// 											 .FirstOrDefault(role => role.role == Enums.CameraRoles.Gameplay)
		// 											 .GetComponent<Camera>();
		// }
	}
}
