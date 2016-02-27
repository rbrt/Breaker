using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;


public class GenerateBackground : MonoBehaviour {

	const int segmentHeight = 4;
	const int segmentWidth = 2;
	const int segmentDepth = 2;

	const float buildingHeight = -20f;
	const float maxBuildingZ = 10f;
	const float minBuildingZ = -10f;

	[SerializeField] protected GameObject buildingComponent;
	[SerializeField] protected Transform buildingRoot;

	List<GameObject> buildings;

	Building GenerateBuilding(int height, int width, int depth){
		GameObject building = new GameObject("Building");
		building.transform.SetParent(buildingRoot);
		building.transform.localPosition = Vector3.zero;

		var buildingObject = building.AddComponent<Building>();
		List<GameObject> elements = new List<GameObject>();

		for (int i = 0; i < height; i++){
			for (int j = 0; j < width; j++){
				for (int k = 0; k < depth; k++){
					GameObject segment = Instantiate(buildingComponent);
					segment.transform.SetParent(building.transform);
					segment.transform.localPosition = (Vector3.up * i * segmentHeight) +
													  (Vector3.right * j * segmentWidth) +
													  (Vector3.forward * k * segmentDepth);

					elements.Add(segment);
				}
			}
		}

		// var collider = building.gameObject.AddComponent<BoxCollider>();
		//
		// collider.center = new Vector3(width - 1,
		// 							  (segmentHeight / 2) * (height - 1),
		// 							  depth - 1);
		//
		// collider.size = new Vector3((segmentWidth / 2f) * width,
		// 							(segmentHeight / 2f) * height,
		// 							(segmentDepth / 2f) * depth);

		buildingObject.SetValues(height, width, depth, elements.ToArray());
		return buildingObject;
	}

	#if UNITY_EDITOR
	[ContextMenu("Test Generate Buildings")]
	public void TestGenerateBuildings(){
		List<Building> buildings = new List<Building>();

		for (int i = 0; i < 30; i++){
			var newBuilding = GenerateBuilding(Random.Range(3, 10), Random.Range(3, 7), Random.Range(2, 4));

			if (buildings.Count > 0){
				var lastBuilding = buildings.Last();
				var newPosition = lastBuilding.transform.localPosition;

				newPosition.x += lastBuilding.width * segmentWidth + (Random.Range(.5f,3));
				newPosition.z += Random.Range(-4,4);
				newPosition.z = Mathf.Clamp(newPosition.z,
											minBuildingZ,
											maxBuildingZ);

				newBuilding.transform.localPosition = newPosition;
			}
			else{
				Vector3 pos = Vector3.up * buildingHeight;
				pos.z += Random.Range(-4,4);

				pos.z = Mathf.Clamp(pos.z,
									minBuildingZ,
									maxBuildingZ);

				newBuilding.transform.localPosition = pos;
			}

			buildings.Add(newBuilding);
		}
	}

	[ContextMenu("Test Generate Building")]
	public void TestGenerateBuilding(){
		int width = 5;
		int height = 10;
		int depth = 5;

		GenerateBuilding(height, width, depth);
	}

	[ContextMenu("Clean Up")]
	public void CleanUp(){
		var buildings = FindObjectsOfType<Building>();
		for (int i = 0; i < buildings.Length; i++){
			DestroyImmediate(buildings[i].gameObject);
		}
	}



	#endif
}
