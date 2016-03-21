using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;


public class TitleMenu : MonoBehaviour {

	const string gameSceneName = "Prototyping";

	void Update () {
		if (Input.GetKeyDown(KeyCode.Return)){
			StartNewGame();
		}
	}

	public void StartNewGame(){
		LevelController.Instance.StartLevel();
		TransitionRig.Instance.TransitionFromMenuToGameplay();
	}
}
