using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {

	static PlayerStats instance;

	int shotsDeflected = 0;
	int shotsHit = 0;
	int kills = 0;

	public static PlayerStats Instance {
		get {
			return instance;
		}
	}

	public void InitializeForRound(){
		shotsDeflected = 0;
		shotsHit = 0;
		kills = 0;
	}

	public void AddShotsDeflected(){
		shotsDeflected++;
	}

	public int GetShotsDeflected(){
		return shotsDeflected;
	}

	public void AddShotsHit(){
		shotsHit++;
	}

	public int GetShotsHit(){
		return shotsHit;
	}

	public void AddKills(){
		kills++;
	}

	public int GetKills(){
		return kills;
	}


	void Awake(){
		if (instance == null){
			instance = this;
		}
		else{
			Debug.Log("Destroyed duplicate instance of PlayerStats");
			Destroy(this.gameObject);
		}
	}
}
