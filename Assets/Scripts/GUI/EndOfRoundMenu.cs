using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EndOfRoundMenu : MonoBehaviour {

	[SerializeField] protected Animator menuAnimator;

	[SerializeField] protected Text nextButtonText;
	[SerializeField] protected Text menuButtonText;
	[SerializeField] protected Text shotsDeflectedResultText;
	[SerializeField] protected Text timesHitResultText;
	[SerializeField] protected Text killsResultText;


	static EndOfRoundMenu instance;

	static string menuAnimatorString = "MenuOpen";
	static string retryString = "Retry";
	static string continueString = "Continue";
	static string menuString = "Menu";

	Enums.EndOfRoundStates currentState;

	public static EndOfRoundMenu Instance {
		get {
			return instance;
		}
	}

	public void MenuButton(){
		SceneManager.LoadScene("Title", LoadSceneMode.Single);
	}

	public void NextButton(){
		if (currentState == Enums.EndOfRoundStates.Death){
			SceneManager.LoadScene("Prototyping", LoadSceneMode.Single);
		}
		else if (currentState == Enums.EndOfRoundStates.Victory){
			SceneManager.LoadScene("Title", LoadSceneMode.Single);
		}
	}

	public void ShowEndOfRoundMenu(Enums.EndOfRoundStates roundState){
		menuAnimator.SetBool(menuAnimatorString, true);
		currentState = roundState;

		if (roundState == Enums.EndOfRoundStates.Death){
			nextButtonText.text = retryString;
			menuButtonText.text = menuString;
			ScoreDisplay.Instance.SaveScore();
		}
		else if (roundState == Enums.EndOfRoundStates.Victory){
			nextButtonText.text = continueString;
			menuButtonText.text = menuString;
		}

		shotsDeflectedResultText.text = "" + PlayerStats.Instance.GetShotsDeflected();
		timesHitResultText.text = "" + PlayerStats.Instance.GetShotsHit();
		killsResultText.text = "" + PlayerStats.Instance.GetKills();
	}

	public void HideEndOfRoundMenu(){
		menuAnimator.SetBool(menuAnimatorString, false);
	}

	void Awake(){
		if (instance == null){
			instance = this;
		}
		else{
			Debug.Log("Destroyed duplicate instance of EndOfRoundMenu");
			Destroy(this.gameObject);
		}
	}

}
