using UnityEngine;
using System.Collections;

public class PlayerStats : Singleton<PlayerStats> {

	int shotsDeflected = 0;
	int shotsHit = 0;
	int kills = 0;

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
}
