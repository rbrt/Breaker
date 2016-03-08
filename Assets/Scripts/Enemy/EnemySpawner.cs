using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class EnemySpawner : MonoBehaviour {

	static EnemySpawner instance;

	public static EnemySpawner Instance {
		get {
			return instance;
		}
	}

	[SerializeField] protected GameObject enemyPrefab;

	List<Enemy> activeEnemies;

	const int targetActiveEnemies = 3;
	const float offscreenBuffer = 20;
	const float spawnRangeMin = 10;
	const float spawnRangeMax = 20;

	void Awake(){
		instance = this;
	}

	void Start () {
		activeEnemies = new List<Enemy>();
	}

	void Update () {
		if (DebugMenu.NoEnemies){
			return;
		}

		if (activeEnemies.Count == 0){
			while (activeEnemies.Count < targetActiveEnemies){
				var offscreenPoint = GetOffscreenPoint();

				var newEnemy = GameObject.Instantiate(enemyPrefab);
				newEnemy.transform.position = offscreenPoint;
				newEnemy.transform.localScale = Vector3.one;
				newEnemy.transform.rotation = Quaternion.identity;

				activeEnemies.Add(newEnemy.GetComponent<Enemy>());

				PinEnemyToBestTransform(ref newEnemy, offscreenPoint);
			}
		}
	}

	Vector3 GetOffscreenPoint(){
		var offscreenPoint = CameraController.OffsetPastRightScreenEdge(offscreenBuffer + Random.Range(spawnRangeMin, spawnRangeMax));
		offscreenPoint.z = 0;
		offscreenPoint.y = Mathf.Clamp(Random.Range(-10, 10), Bounds.yMin, Bounds.yMax);
		var possiblePoint = TerrainManager.Instance.GetTransformNearestToPosition(offscreenPoint);

		int selectionOffset = 0;

		while (activeEnemies.Any(x => Vector3.Distance(x.transform.position, possiblePoint.position) < 2)){
			offscreenPoint = CameraController.OffsetPastRightScreenEdge(offscreenBuffer + Random.Range(spawnRangeMin, spawnRangeMax));

			if (selectionOffset == 0){
				possiblePoint = TerrainManager.Instance.GetTransformNearestToPosition(offscreenPoint);
			}
			else{
				possiblePoint = TerrainManager.Instance.GetTransformNearestToPosition(offscreenPoint, ignoreFirst: selectionOffset);
			}

			selectionOffset++;

			if (selectionOffset > 20){
				Debug.LogWarning("aborted");
				break;
			}
		}

		return possiblePoint.position;
	}

	void PinEnemyToBestTransform(ref GameObject enemy, Vector3 point){
		Transform target = TerrainManager.Instance.GetTransformNearestToPosition(point);
		var pos = target.transform.position;

		float xMod = target.localScale.x / 2 * .7f;

		pos.x += Random.Range(0, xMod);
		pos.z = 0;
		pos.y = target.position.y + target.localScale.y / 2 + enemy.transform.localScale.y / 2;

		enemy.transform.position = pos;
	}

	public void ClearEnemy(Enemy toClean){
		activeEnemies.Remove(toClean);
		Destroy(toClean.gameObject);
	}

	public void ClearAllEnemies(){
		for (int i = 0; i < activeEnemies.Count; i++){
			activeEnemies[i].ShieldDeath();
		}
	}
}
