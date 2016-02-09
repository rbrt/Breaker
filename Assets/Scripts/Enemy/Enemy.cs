using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	[SerializeField] protected Shot shotPrefab;

	void Start () {
		this.StartSafeCoroutine(ShootAtPlayer());
	}

	void OnCollisionEnter(Collision other){
		var shot = other.gameObject.GetComponent<Shot>();
		if (shot != null && shot.Deflected){
			shot.HitEnemy();
			HitByShot();
		}
	}

	void HitByShot(){
		Debug.Log("Hit enemy");
	}

	IEnumerator ShootAtPlayer(){
		while (true){
			yield return new WaitForSeconds(1);
			var shot = GameObject.Instantiate(shotPrefab.gameObject);
			shot.transform.position = transform.position;
		}
	}

}
