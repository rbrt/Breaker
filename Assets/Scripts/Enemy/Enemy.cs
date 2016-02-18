using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	Transform playerTransform;

	[SerializeField] protected Shot shotPrefab;

	void Start () {
		playerTransform = PlayerController.Instance.transform;
		this.StartSafeCoroutine(ShootAtPlayer());
	}

	void Update(){
		var point = CameraController.ScreenPoint(transform.position + Vector3.right * (transform.localScale.x / 2));
		if (point.x + (transform.localScale.x / 2) < -10){
			EnemySpawner.Instance.ClearEnemy(this);
		}
	}

	void OnCollisionEnter(Collision other){
		var shot = other.gameObject.GetComponent<Shot>();
		if (shot != null && shot.Deflected){
			shot.HitEnemy();
			HitByShot();
		}
	}

	void HitByShot(){
		Die();
	}

	void Die(){
		EnemySpawner.Instance.ClearEnemy(this);
	}

	public void ShieldDeath(){
		Die();
	}

	IEnumerator ShootAtPlayer(){
		while (true){
			yield return new WaitForSeconds(1);
			var shot = GameObject.Instantiate(shotPrefab.gameObject);
			Vector3 pos = transform.position + (playerTransform.position - transform.position).normalized * .5f;
			pos.z = 0;

			shot.transform.position = pos;

		}
	}

}
