using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;


public class ScoreDisplay : MonoBehaviour {

	const int scoreLength = 9;

	static ScoreDisplay instance;

	public static ScoreDisplay Instance {
		get {
			return instance;
		}
	}

	[SerializeField] protected Text scoreText;
	[SerializeField] protected Text multiplierText;

	int currentScore = 0;
	int actualScore = 0;

	string scoreString = "000000000";
	string multiplierString = "0x";

	void Awake(){
		instance = this;
		scoreText.text = scoreString;
		multiplierText.text = multiplierString;
	}

	void Update () {
		if (currentScore < actualScore){
			currentScore++;
			scoreText.text = ConvertScoreToString(currentScore);
		}
	}

	string ConvertScoreToString(int score){
		string scoreString = "" + score;

		while (scoreString.Length < scoreLength){
			scoreString = "0" + scoreString;
		}

		return scoreString;
	}

	public void AddScore(int points){
		actualScore += points;
	}
}
