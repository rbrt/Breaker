using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour {

	float shotSpeed = 20;
	Transform target;
	Vector3 targetDirection;
	Collider lastHit;
	Collider ownCollider;

	bool destroyed = false,
		 deflected = false;

	ParticleSystem particles;

	public bool Deflected{
		get {
			return deflected;
		}
	}

	public bool Destroyed {
		get {
			return destroyed;
		}
	}

	void Awake(){
		particles = GetComponent<ParticleSystem>();
		ownCollider = GetComponent<Collider>();
	}

	void Start(){
		target = PlayerController.Instance.transform;
		targetDirection = (target.position - transform.position).normalized;
	}

	void Update () {
		if (!destroyed){
			var targetPos = transform.position + targetDirection * shotSpeed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, targetPos, .2f);
		}
	}

	public void SetTargetDirection(Vector3 direction, Collider last){
		if (lastHit != null && lastHit == last){
			return;
		}

		deflected = true;
		lastHit = last;
		direction.z = 0;
		targetDirection = direction.normalized;
	}

	public void HitPlayer(){
		if (!deflected){
			destroyed = true;
			ownCollider.enabled = false;
			this.StartSafeCoroutine(DestroyShot());
		}
	}

	public void HitEnemy(){
		destroyed = true;
		this.StartSafeCoroutine(DestroyShot());
	}

	IEnumerator DestroyShot(){
		particles.startLifetime = .5f;
		particles.startSize = 4;
		yield return new WaitForSeconds(.2f);
		Destroy(this.gameObject);
	}
}
