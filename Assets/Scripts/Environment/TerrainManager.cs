using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;


public class TerrainManager : MonoBehaviour {

	static TerrainManager instance;

	public static TerrainManager Instance {
		get {
			return instance;
		}
	}

	GroundSpawner groundSpawner;
	PlatformSpawner platformSpawner;

	void Awake(){
		if (instance == null){
			instance = this;
			groundSpawner = GetComponent<GroundSpawner>();
			platformSpawner = GetComponent<PlatformSpawner>();
		}
		else{
			Debug.LogWarning("Destroyed duplicate instance of TerrainManager");
			Destroy(this.gameObject);
		}
	}

	public Transform GetTransformNearestToPosition(Vector3 position){
		List<Transform> transforms = groundSpawner.GetGroundTransforms();
		transforms.AddRange(platformSpawner.GetPlatformTransforms());

		Debug.Log("using this one", transforms.OrderBy(x => Vector3.Distance(x.position, position)).FirstOrDefault().gameObject);

		return transforms.OrderBy(x => Vector3.Distance(x.position, position)).FirstOrDefault();
	}
}
