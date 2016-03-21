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

	float distanceTravelled = 0;
	bool roundOver = false;

	public bool AtEndOfLevel(){
		return distanceTravelled > (levelDistance - 15f);
	}

	public bool RoundOver{
		get {
			return roundOver;
		}
		set {
			roundOver = value;
		}
	}

	public float DistanceTravelled{
		get {
			return distanceTravelled;
		}
		set {
			distanceTravelled = value;
		}
	}

	public void StartLevel(){
		roundOver = false;
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
