using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	Transform playerTransform;

	[SerializeField] protected Shot shotPrefab;
	[SerializeField] protected int scorePoints = 10;
	[SerializeField] protected int contactDamage = -1;
	[SerializeField] protected int shieldReward = 5;

	[SerializeField] protected Animator turretAnimator;
	[SerializeField] protected ParticleSystem shotChargeParticles;
	[SerializeField] protected ParticleSystem energyParticles;

	[SerializeField] protected Transform turretTransform;

	float idleShotParticleSize = .1f;
	float attackingShotParticleSize = .6f;

	float shotRate = 35f;

	bool dying = false;

	DestroyEnemy destroyEnemyEffect;

	SafeCoroutine shootingCoroutine;

	public int ContactDamage {
		get {
			return contactDamage;
		}
	}

	void Awake(){
		destroyEnemyEffect = GetComponent<DestroyEnemy>();
	}

	void Start () {
		playerTransform = PlayerController.Instance.transform;
		shootingCoroutine = this.StartSafeCoroutine(ShootAtPlayer());
	}

	void Update(){
		turretTransform.LookAt(playerTransform);

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
		if (!dying){
			dying = true;

			GetComponent<Collider>().enabled = false;

			if (shootingCoroutine != null && shootingCoroutine.IsRunning){
				shootingCoroutine.Stop();
			}

			this.StartSafeCoroutine(PlayDeath());
			ScoreDisplay.Instance.AddScore(scorePoints);
			Shield.Instance.AddShield(shieldReward);
			PlayerStats.Instance.AddKills();
		}
	}

	IEnumerator PlayDeath(){
		yield return this.StartSafeCoroutine(destroyEnemyEffect.ExplodeAll());
		EnemySpawner.Instance.ClearEnemy(this);
		Destroy(energyParticles.gameObject);
	}

	public void ShieldDeath(){
		Die();
	}

	IEnumerator ShootAtPlayer(){
		yield return new WaitForSeconds(Random.Range(0, .9f));

		while (true && !PlayerController.Instance.Dead && !dying){
			if (!DebugMenu.EnemiesDontAttack){
				shotChargeParticles.startSize = idleShotParticleSize;

				yield return new WaitForSeconds(shotRate * Time.deltaTime);

				for (float i = 0; i < 1; i += Time.deltaTime / .2f){
					shotChargeParticles.startSize = Mathf.Lerp(idleShotParticleSize, attackingShotParticleSize, i);
					yield return null;
				}

				for (float i = 0; i < 1; i += Time.deltaTime / .2f){
					turretAnimator.speed = Mathf.Lerp(1, 0, i);
					yield return null;
				}

				shotChargeParticles.Stop();

				var shot = GameObject.Instantiate(shotPrefab.gameObject);
				Vector3 pos = shotChargeParticles.transform.position; //+ (playerTransform.position - transform.position).normalized * .5f;
				pos.z = 0;

				shot.transform.position = pos;

				for (float i = 0; i < 1; i += Time.deltaTime / .05f){
					shotChargeParticles.startSize = Mathf.Lerp(attackingShotParticleSize, idleShotParticleSize, i);
					yield return null;
				}

				shotChargeParticles.Play();

				for (float i = 0; i < 1; i += Time.deltaTime / .05f){
					turretAnimator.speed = Mathf.Lerp(0, 1, i);
					yield return null;
				}


			}
			yield return null;
		}
	}
}
