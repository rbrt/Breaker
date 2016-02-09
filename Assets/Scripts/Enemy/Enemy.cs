using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	[SerializeField] protected Shot shotPrefab;

	void Start () {
		this.StartSafeCoroutine(ShootAtPlayer());
	}

	IEnumerator ShootAtPlayer(){
		while (true){
			yield return new WaitForSeconds(1);
			var shot = GameObject.Instantiate(shotPrefab.gameObject);
			shot.transform.position = transform.position;
		}
	}

}
