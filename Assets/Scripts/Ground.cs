using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour {

	void OnCollisionEnter(Collision other){
		if (other.gameObject.GetComponent<PlayerController>() != null){
			PlayerController.Instance.HitGround();
		}
	}
}
