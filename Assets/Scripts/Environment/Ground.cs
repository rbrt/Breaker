using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour {

	void Update(){
		var point = CameraController.ScreenPoint(transform.position + Vector3.right * (transform.localScale.x / 2));
		if (point.x + (transform.localScale.x / 2) < -10){
			GroundSpawner.Instance.ClearGround(this);
		}
	}

}
