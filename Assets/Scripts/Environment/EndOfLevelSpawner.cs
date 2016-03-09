using UnityEngine;
using System.Collections;
using System.Linq;

public class EndOfLevelSpawner : MonoBehaviour {

	[SerializeField] protected GameObject endOfLevelPrefab;

	bool endedLevel = false;


	void Update () {
		if (!endedLevel && LevelController.Instance.AtEndOfLevel()){
			endedLevel = true;
			var endOfLevel = GameObject.Instantiate(endOfLevelPrefab);
			var lastGroundTransform = GroundSpawner.Instance.GetGroundTransforms().Last().transform;
			var pos = lastGroundTransform.position;
			pos.y += lastGroundTransform.localScale.y;
			pos.x += lastGroundTransform.localScale.x / 2 + endOfLevel.transform.localScale.x + 3;
			endOfLevel.transform.position = pos;
		}
	}
}
