using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	Transform playerTransform;

	[SerializeField] protected Shot shotPrefab;
	[SerializeField] protected int scorePoints = 10;
	[SerializeField] protected int contactDamage = -1;

	public int ContactDamage {
		get {
			return contactDamage;
		}
	}

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

	void OnTriggerEnter(Collider other){
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
		ScoreDisplay.Instance.AddScore(scorePoints);
		EnemySpawner.Instance.ClearEnemy(this);
	}

	public void ShieldDeath(){
		Die();
	}

	IEnumerator ShootAtPlayer(){
		while (true && !PlayerController.Instance.Dead){
			if (!DebugMenu.EnemiesDontAttack){
				yield return new WaitForSeconds(1);
				var shot = GameObject.Instantiate(shotPrefab.gameObject);
				Vector3 pos = transform.position + (playerTransform.position - transform.position).normalized * .5f;
				pos.z = 0;

				shot.transform.position = pos;
			}
			yield return null;
		}
	}

}
