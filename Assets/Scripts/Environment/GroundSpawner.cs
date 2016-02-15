using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GroundSpawner : MonoBehaviour {

	static GroundSpawner instance;

	public static GroundSpawner Instance{
		get {
			return instance;
		}
	}

	[SerializeField] protected GameObject groundPrefab;

	List<Ground> groundElements;

	float maxPitDistance = 5,
		  minPitDistance = 2,
		  minWidth = 5,
		  maxWidth = 40;

	void Awake(){
		instance = this;

		groundElements = new List<Ground>();

		Vector3 startPos = new Vector3(0, (-10 / 2) - 1.5f, 0);
		groundElements.Add(SpawnGroundBlock(30, 10, startPos));
	}

	void Update(){
		if (groundElements.Count < 5){
			SpawnNextBlock();
		}
	}

	void SpawnNextBlock(){
		var lastBlock = groundElements.Last();
		Vector3 newScale = new Vector3(Random.Range(minWidth, maxWidth), 10, 1);
		Vector3 position = lastBlock.transform.position;

		position.x += lastBlock.transform.localScale.x / 2;
		position.x += newScale.x / 2;

		position.x += Random.Range(minPitDistance, maxPitDistance);

		groundElements.Add(SpawnGroundBlock(newScale.x, newScale.y, position));
	}

	Ground SpawnGroundBlock(float width, float height, Vector3 position){
		var block = GameObject.Instantiate(groundPrefab);
		block.transform.position = position;
		Vector3 scale = new Vector3(width, height, 1);
		block.transform.localScale = scale;

		block.transform.SetParent(this.transform);

		return block.GetComponent<Ground>();
	}

	public void ClearGround(Ground ground){
		groundElements.Remove(ground);
		Destroy(ground.gameObject);
	}
}
