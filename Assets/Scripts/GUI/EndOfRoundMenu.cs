using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EndOfRoundMenu : MonoBehaviour {

	public void MenuButton(){
		SceneManager.LoadScene("Title", LoadSceneMode.Single);
	}

	public void NextButton(){
		SceneManager.LoadScene("Title", LoadSceneMode.Single);
	}

}
