using UnityEngine;
using System.Collections;

public class LevelTransitionMenu : MonoBehaviour {

	public void QuitToTitle(){
		LoadingController.LoadTitleScene();
	}

	public void StartNextLevel(){
		this.StartSafeCoroutine(EndOfLevelSequence.Instance.LeaveScreenAndAdvanceLevel());
	}

}
