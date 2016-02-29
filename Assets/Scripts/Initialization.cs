using UnityEngine;
using System.Collections;

public class Initialization : MonoBehaviour {

	int targetFPS = 60;

	[SerializeField] protected GameObject guiPrefab;

	void Awake(){
		Application.targetFrameRate = targetFPS;
		GameObject.Instantiate(guiPrefab);
	}
}
