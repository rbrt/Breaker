﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GroundSpawner : Singleton<GroundSpawner> {

	const int groundElementCount = 3;

	[SerializeField] protected GameObject groundPrefab;

	List<Ground> groundElements;

	float maxPitDistance = 5,
		  minPitDistance = 2,
		  minWidth = 5,
		  maxWidth = 40;

	protected override void Startup(){
		groundElements = new List<Ground>();

		Vector3 startPos = new Vector3(0, (-5 / 2) - 1.5f, 0);
		groundElements.Add(SpawnGroundBlock(30, 5, startPos));

		while (groundElements.Count < groundElementCount){
			SpawnNextBlock();
		}
	}

	void Update(){
		if (groundElements.Count < groundElementCount && !LevelController.Instance.AtEndOfLevel()){
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

		groundElements.Add(SpawnGroundBlock(newScale.x, Random.Range(3, 5), position));
	}

	Ground SpawnGroundBlock(float width, float height, Vector3 position){
		var block = GameObject.Instantiate(groundPrefab);
		block.transform.position = position;
		Vector3 scale = new Vector3(width, height, 1);
		block.transform.localScale = scale;

		block.transform.SetParent(this.transform);

		return block.GetComponent<Ground>();
	}

	public List<Transform> GetGroundTransforms(){
		return groundElements.Where(x => x != null).Select(x => x.transform).ToList();
	}

	public void ClearGround(Ground ground){
		groundElements.Remove(ground);
		Destroy(ground.gameObject);
	}
}
