using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using UnityEngine.SceneManagement;


public class SettingsMenu : MonoBehaviour {

	static bool menuActive = false;
	const string menuAnimatorString = "MenuOpen";

	[SerializeField] Animator menuAnimator;

	public void QuitToTitle(){
		SceneManager.LoadScene("Title", LoadSceneMode.Single);
	}

	public void ToggleMenu(){
		menuActive = !menuActive;
		menuAnimator.SetBool(menuAnimatorString, menuActive);
	}

	public void PauseGame(){
		Time.timeScale = 0;
	}

	public void UnpauseGame(){
		Time.timeScale = 1;
	}
}
