using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour {

	float shotSpeed = 20;
	Transform target;
	Vector3 targetDirection;
	Collider lastHit;

	void Start(){
		target = PlayerController.Instance.transform;
		targetDirection = (target.position - transform.position).normalized;
	}

	void Update () {
		var targetPos = transform.position + targetDirection;
		transform.position = Vector3.MoveTowards(transform.position, targetPos, .2f);
	}

	public void SetTargetDirection(Vector3 direction, Collider last){
		if (lastHit != null && lastHit == last){
			return;
		}
		lastHit = last;
		direction.z = 0;
		targetDirection = direction;
	}
}
