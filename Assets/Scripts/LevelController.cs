using UnityEngine;
using System.Collections;

public class LevelController : Singleton<LevelController> {

	//const float levelDistance = 175f;
	const float levelDistance = 25f;

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
		this.StartSafeCoroutine(WaitForGUIControllerThenDisplayGameplayGUI());
	}

	IEnumerator WaitForGUIControllerThenDisplayGameplayGUI(){
		while (GUIController.Instance == null){
			yield return null;
		}
		GUIController.Instance.ShowGameplayCanvasNoSideEffects();
	}

}
