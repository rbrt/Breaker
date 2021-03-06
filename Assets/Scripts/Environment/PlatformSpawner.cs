﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlatformSpawner : Singleton<PlatformSpawner> {

	const float groundHeight = -1.5f;
	const int platformCount = 4;

	float minHeight = groundHeight + 2;
	float maxHeight = 5;
	float minDifficulty = 3;
	float maxDifficulty = 10;

	[SerializeField] protected GameObject platformPrefab;

	List<Platform> platformElements;

	float minWidth = 3,
		  maxWidth = 7,
		  minGapDistance = 2,
		  maxGapDistance = 5;

	protected override void Startup(){
		platformElements = new List<Platform>();

		Vector3 startPos = new Vector3(0, .5f, 0);
		platformElements.Add(SpawnPlatform(minWidth, .2f, startPos));

		while (platformElements.Count < platformCount){
			SpawnNextBlock();
		}
	}

	void Update () {
		if (platformElements.Count < platformCount && !LevelController.Instance.AtEndOfLevel()){
			SpawnNextBlock();
		}
	}

	void SpawnNextBlock(){
		var lastBlock = platformElements.Last();
		Vector3 newScale = new Vector3(Random.Range(minWidth, maxWidth), .2f, 1);
		Vector3 position = lastBlock.transform.position;

		ModifyPosition(ref position, ref lastBlock, ref newScale);

		int backout = 0;
		while (TerrainManager.Instance != null && TerrainManager.Instance.PointContainedInExistingTerrain(position)){
			ModifyPosition(ref position, ref lastBlock, ref newScale);
			backout++;
			if (backout > 50){
				Debug.LogError("Backed out!");
				break;
			}
		}

		float difficulty = Random.Range(minDifficulty, maxDifficulty);
		Vector3 direction = (position - lastBlock.transform.position).normalized;

		position = lastBlock.transform.position + direction * difficulty;
		position.x += newScale.x / 2;
		position.z = 0;

		while (position.y < minHeight){
			position.y += .1f;
		}
		while (position.y > maxHeight){
			position.y -= .1f;
		}

		platformElements.Add(SpawnPlatform(newScale.x, newScale.y, position));
	}

	void ModifyPosition(ref Vector3 position, ref Platform lastBlock, ref Vector3 newScale){
		position.x += lastBlock.transform.localScale.x / 2;
		position.x += newScale.x / 2;

		position.x += Random.Range(minGapDistance, maxGapDistance);
		position.y += Random.Range(-4, 4);
	}

	Platform SpawnPlatform(float width, float height, Vector3 position){
		var block = GameObject.Instantiate(platformPrefab);
		block.transform.position = position;
		Vector3 scale = new Vector3(width, height, 1);
		block.transform.localScale = scale;

		block.transform.SetParent(this.transform);
		return block.GetComponent<Platform>();
	}

	public List<Transform> GetPlatformTransforms(){
		return platformElements.Where(x => x != null).Select(x => x.transform).ToList();
	}

	public void ClearPlatform(Platform platform){
		platformElements.Remove(platform);
		Destroy(platform.gameObject);
	}
}
