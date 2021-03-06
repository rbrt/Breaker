﻿using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;


public class TerrainManager : Singleton<TerrainManager> {

	GroundSpawner groundSpawner;
	PlatformSpawner platformSpawner;

	protected override void Startup(){
		groundSpawner = GetComponent<GroundSpawner>();
		platformSpawner = GetComponent<PlatformSpawner>();
	}

	public Transform GetTransformNearestToPosition(Vector3 position, int ignoreFirst = 0){
		List<Transform> transforms = groundSpawner.GetGroundTransforms();
		transforms.AddRange(platformSpawner.GetPlatformTransforms());
		transforms = transforms.OrderBy(x => Vector3.Distance(x.position, position)).ToList();

		for (int i = 0; i < ignoreFirst && transforms.Count > 0; i++){
			transforms.RemoveAt(0);
		}

		return transforms.OrderBy(x => Vector3.Distance(x.position, position)).FirstOrDefault();
	}

	public bool PointContainedInExistingTerrain(Vector3 point){
		return groundSpawner.GetGroundTransforms().Any(ground => ground.GetComponent<Collider>().bounds.Contains(point));
	}
}
