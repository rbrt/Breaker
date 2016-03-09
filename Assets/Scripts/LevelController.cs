using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

	static LevelController instance;

	//const float levelDistance = 175f;
	const float levelDistance = 25f;

	public static LevelController Instance{
		get {
			return instance;
		}
	}

	float distanceTravelled;

	public bool AtEndOfLevel(){
		return distanceTravelled > (levelDistance - 15f);
	}

	public float DistanceTravelled{
		get {
			return distanceTravelled;
		}
		set {
			distanceTravelled = value;
		}
	}

	void Awake(){
		if (instance == null){
			instance = this;
		}
		else{
			Destroy(this.gameObject);
			Debug.Log("Destroyed duplicate instance of LevelController");
		}
	}

}
