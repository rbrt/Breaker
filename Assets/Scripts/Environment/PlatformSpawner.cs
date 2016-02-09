using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlatformSpawner : MonoBehaviour {

	static PlatformSpawner instance;

	public static PlatformSpawner Instance {
		get {
			return instance;
		}
	}

	[SerializeField] protected GameObject platformPrefab;

	List<Platform> platformElements;

	float minWidth = 3,
		  maxWidth = 7;

	void Awake(){
		instance = this;
		platformElements = new List<Platform>();

		Vector3 startPos = new Vector3(0, .5f, 0);
		platformElements.Add(SpawnPlatform(minWidth, .2f, startPos));
	}

	void Update () {
		if (platformElements.Count < 5){
			SpawnNextBlock();
		}
	}

	void SpawnNextBlock(){
		var lastBlock = platformElements.Last();
		Vector3 newScale = new Vector3(Random.Range(minWidth, maxWidth), .2f, 1);
		Vector3 position = lastBlock.transform.position;

		position.x += lastBlock.transform.localScale.x / 2;
		position.x += newScale.x / 2;

		//position.x += Random.Range(minPitDistance, maxPitDistance);

		platformElements.Add(SpawnPlatform(newScale.x, newScale.y, position));
	}

	Platform SpawnPlatform(float width, float height, Vector3 position){
		var block = GameObject.Instantiate(platformPrefab);
		block.transform.position = position;
		Vector3 scale = new Vector3(width, height, 1);
		block.transform.localScale = scale;

		block.transform.SetParent(this.transform);

		return block.GetComponent<Platform>();
	}

	public void ClearPlatform(Platform platform){
		platformElements.Remove(platform);
		Destroy(platform.gameObject);
	}
}
