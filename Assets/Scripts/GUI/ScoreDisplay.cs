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

	const int scoreLength = 8;

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
			if (actualScore - currentScore > 30){
				currentScore += 3;
			}
			else if (actualScore - currentScore > 100){
				currentScore += 10;
			}
			else{
				currentScore++;
			}
			
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
