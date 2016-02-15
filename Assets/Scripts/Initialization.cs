using UnityEngine;
using System.Collections;

public class Initialization : MonoBehaviour {

	int targetFPS = 60;

	void Awake(){
		Application.targetFrameRate = targetFPS;
	}
}
