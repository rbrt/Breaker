using UnityEngine;
using System.Collections;
using System.Linq;

public class EnemySpawner : MonoBehaviour {

	[SerializeField] protected GameObject enemyPrefab;

	const int targetActiveEnemies = 1;
	const float offscreenBuffer = 20;

	int activeEnemies = 0;

	void Start () {

	}

	void Update () {
		if (activeEnemies == 0){
			while (activeEnemies < targetActiveEnemies){
				var offscreenPoint = CameraController.OffsetPastRightScreenEdge(offscreenBuffer);
				offscreenPoint.z = 0;
				offscreenPoint.y = 1;

				var newEnemy = GameObject.Instantiate(enemyPrefab);
				newEnemy.transform.position = offscreenPoint;
				newEnemy.transform.localScale = Vector3.one;
				newEnemy.transform.rotation = Quaternion.identity;
				activeEnemies++;

				PinEnemyToBestTransform(ref newEnemy, offscreenPoint);
			}
		}
	}

	void PinEnemyToBestTransform(ref GameObject enemy, Vector3 point){
		Transform target = TerrainManager.Instance.GetTransformNearestToPosition(point);
		var pos = target.transform.position;

		float xMod = target.localScale.x / 2 * .7f;

		pos.x += Random.Range(-xMod, xMod);
		pos.z = 0;
		pos.y = target.position.y + target.localScale.y / 2 + enemy.transform.position.y / 2;

		enemy.transform.position = pos;
	}
}
