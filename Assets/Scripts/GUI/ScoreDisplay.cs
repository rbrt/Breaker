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

	const float multiplierLifetime = 15;

	static ScoreDisplay instance;

	public static ScoreDisplay Instance {
		get {
			return instance;
		}
	}

	[SerializeField] protected Text scoreText;
	[SerializeField] protected Text multiplierText;
	[SerializeField] protected Image multiplierMask;

	int currentScore = 0;
	int actualScore = 0;

	int multiplier = 1;

	string scoreString = "000000000";
	string multiplierString = "0x";

	float lastScoreTime = 0;
	int lastTierScore;

	int[] scoreTiers = {0,
						20,
						70,
						160,
						250};

	void Awake(){
		instance = this;
		scoreText.text = scoreString;
		multiplierText.text = multiplierString;
		lastScoreTime = Time.time;
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


		if (Time.time - lastScoreTime > multiplierLifetime){
			if (multiplier > 1){
				multiplier--;
				lastScoreTime = Time.time;
				lastTierScore = 0;
			}
		}
		else if (multiplier < (scoreTiers.Length - 1) && lastTierScore > scoreTiers[multiplier]){
			multiplier++;
			lastScoreTime = Time.time;
			lastTierScore = 0;
		}

		multiplierText.text = "x" + multiplier;

		if (multiplier > 1){
			multiplierMask.fillAmount = 1 - ((Time.time - lastScoreTime) / multiplierLifetime);
		}
		else {
			multiplierMask.fillAmount = 0;
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
		points *= multiplier;
		actualScore += points;
		lastTierScore += points;
	}
}
