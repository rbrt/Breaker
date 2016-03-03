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

	static GenerateBackground instance;

	public static GenerateBackground Instance {
		get {
			return instance;
		}
	}

	const int segmentHeight = 4;
	const int segmentWidth = 2;
	const int segmentDepth = 2;

	const float buildingHeight = -10f;
	const float maxBuildingZ = 10f;
	const float minBuildingZ = -10f;

	int lastIndex = 0;

	[SerializeField] protected GameObject buildingComponent;
	[SerializeField] protected GameObject buildingTopCornerComponent;
	[SerializeField] protected GameObject buildingTopComponent;
	[SerializeField] protected Transform buildingRoot;

	List<Building> allBuildings;

	Building GenerateBuilding(int height, int width, int depth){
		GameObject building = new GameObject("Building");
		building.transform.SetParent(buildingRoot);
		building.transform.localPosition = Vector3.zero;

		var buildingObject = building.AddComponent<Building>();

		var elements = new List<GameObject>();

		if (Random.Range(0,100) > 75){
			elements = CreateBasicTaperedBuilding(height, width, depth, ref buildingObject);	
		}
		else{
			elements = CreateBasicBuilding(height, width, depth, ref buildingObject);
		}

		buildingObject.SetValues(height, width, depth, elements.ToArray());
		return buildingObject;
	}

	List<GameObject> CreateBasicBuilding(int height, int width, int depth, ref Building parentBuilding){
		List<GameObject> elements = new List<GameObject>();

		for (int i = 0; i < height; i++){
			for (int j = 0; j < width; j++){
				for (int k = 0; k < depth; k++){
					GameObject segment = Instantiate(buildingComponent);
					segment.transform.SetParent(parentBuilding.transform);
					segment.transform.localPosition = (Vector3.up * i * segmentHeight) +
													  (Vector3.right * j * segmentWidth) +
													  (Vector3.forward * k * segmentDepth);

					elements.Add(segment);
				}
			}
		}

		return elements;
	}

	List<GameObject> CreateBasicTaperedBuilding(int height, int width, int depth, ref Building parentBuilding){
		List<GameObject> elements = new List<GameObject>();

		height += 2;

		for (int i = 0; i < height; i++){
			for (int j = 0; j < width; j++){
				for (int k = 0; k < depth; k++){
					GameObject segment = null;

					// Top of building
					if (i == height - 1){
						// left side of building
						if (j == 0){
							// front
							if (k == 0){
								segment = Instantiate(buildingTopCornerComponent);
								segment.transform.Rotate(new Vector3(0,0,180));
							}
							else{
								segment = Instantiate(buildingTopComponent);
							}
						}
						// right side of building
						else if (j == width - 1){
							// front
							if (k == 0){
								segment = Instantiate(buildingTopCornerComponent);
								segment.transform.Rotate(new Vector3(0,0,90));
							}
							else{
								segment = Instantiate(buildingTopComponent);
								segment.transform.Rotate(new Vector3(0,0,180));
							}
						}
						// center segment
						else{
							// front
							if (k == 0){
								segment = Instantiate(buildingTopComponent);
								segment.transform.Rotate(new Vector3(0,0,-90));
							}
							else{
								segment = Instantiate(buildingComponent);
							}
						}
					}
					else {
						segment = Instantiate(buildingComponent);
					}

					segment.transform.SetParent(parentBuilding.transform);
					segment.transform.localPosition = (Vector3.up * i * segmentHeight) +
													  (Vector3.right * j * segmentWidth) +
													  (Vector3.forward * k * segmentDepth);

					elements.Add(segment);
				}
			}
		}

		return elements;
	}

	void GenerateBuildings(int buildingCount, float zOffset){
		for (int i = 0; i < buildingCount; i++){
			var newBuilding = GenerateBuilding(Random.Range(3, 10), Random.Range(3, 7), Random.Range(2, 4));
			allBuildings.Add(newBuilding);

			if (allBuildings.Count > 1){
				PlaceRelativeToLastBuilding(allBuildings, ref newBuilding, zOffset);
			}
			else{
				Vector3 pos = Vector3.up * buildingHeight;
				pos.z += Random.Range(-4,4);

				pos.z = Mathf.Clamp(pos.z,
									minBuildingZ,
									maxBuildingZ);

				newBuilding.transform.localPosition = pos;
			}
		}

		allBuildings.OrderBy(building => building.transform.position.x);
	}

	void PlaceRelativeToLastBuilding(List<Building> buildings, ref Building newBuilding, float zOffset){
		var lastBuilding = buildings[lastIndex];
		var newPosition = lastBuilding.transform.localPosition;

		newPosition.x += lastBuilding.width * segmentWidth + (Random.Range(.5f,3));
		newPosition.z += Random.Range(-4,4);
		newPosition.z = Mathf.Clamp(newPosition.z,
									minBuildingZ,
									maxBuildingZ);

		newPosition.z += zOffset;

		newBuilding.transform.localPosition = newPosition;

		lastIndex = (lastIndex + 1) % allBuildings.Count;
	}

	void SeedBuildings(){
		for (int i = 0; i < 2; i++){
			GenerateBuildings(15, i * 3);
		}
	}

	void Awake(){
		if (instance == null){
			instance = this;
			allBuildings = new List<Building>();

			lastIndex = 0;

			SeedBuildings();
		}
		else {
			Destroy(this.gameObject);
			Debug.Log("Destroyed duplicate instance of GenerateBackground.");
		}
	}

	public void RecycleBuilding(ref Building targetBuilding){
		PlaceRelativeToLastBuilding(allBuildings, ref targetBuilding, 0);
	}

	#if UNITY_EDITOR
	[ContextMenu("Test Generate Buildings")]
	public void TestGenerateBuildings(){
		allBuildings = new List<Building>();
		lastIndex = 0;

		for (int i = 0; i < 2; i++){
			GenerateBuildings(15, i * 3);
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
