using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour {

	float shotSpeed = 100;
	Transform target;
	Vector3 targetDirection;
	Collider lastHit;
	Collider ownCollider;

	bool destroyed = false,
		 deflected = false;

	ParticleSystem particles;

	Vector3 targetPos;

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
			if (!deflected){
				targetPos = transform.position + targetDirection * shotSpeed * Time.deltaTime;
			}
			else{
				targetPos = transform.position + targetDirection * shotSpeed * 2 * Time.deltaTime;
			}
			transform.position = Vector3.MoveTowards(transform.position, targetPos, .5f);
		}
	}

	public void SetTargetDirection(Vector3 direction, Collider last){
		if (lastHit != null && lastHit == last){
			return;
		}

		PlayerStats.Instance.AddShotsDeflected();

		deflected = true;
		particles.startColor = Color.yellow;
		lastHit = last;
		direction.z = 0;
		targetDirection = direction.normalized;
	}

	public void HitPlayer(){
		if (!deflected){
			PlayerStats.Instance.AddShotsHit();

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
