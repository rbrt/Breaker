using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Linq;

public class HighScoresHandler : MonoBehaviour {

	static string highScoreString = "HighScore";
	static int scoresCount = 5;

	const string menuAnimatorString = "MenuOpen";

	[SerializeField] protected Text scoresText;
	[SerializeField] protected Text datesText;

	[SerializeField] protected Animator highScorePanelAnimator;

	public static string[] GetHighScores()
    {
		return Enumerable.Range(0,5)
						 .Select(x => PlayerPrefs.GetString(highScoreString + x))
						 .Where(x => !string.IsNullOrEmpty(x)).ToArray();
	}

	public static void AddHighScore(int score)
    {
		var scores = GetHighScores().ToList();
		scores.Add(score + "\t" + System.DateTime.Now.Date.ToString("d"));
		scores = scores.OrderByDescending(x => int.Parse(x.Split('\t')[0])).ToList();

		for (int i = 0; i < scoresCount && i < scores.Count; i++)
        {
			PlayerPrefs.SetString(highScoreString + i, scores[i]);
		}
	}

	public void DisplayScores()
    {
		string scoreText = "";
		string dateText = "";
		GetHighScores().ToList().ForEach(x => scoreText += x.Split('\t')[0] + "\n");
		GetHighScores().ToList().ForEach(x => dateText += x.Split('\t')[1] + "\n");

		scoresText.text = scoreText;
		datesText.text = dateText;
	}

	public void ShowMenu()
    {
		highScorePanelAnimator.SetBool(menuAnimatorString, true);
		DisplayScores();
	}

	public void HideMenu(){
		highScorePanelAnimator.SetBool(menuAnimatorString, false);
	}

	#if UNITY_EDITOR
	[MenuItem("Custom/Scores/Display Scores")]
	public static void PopulateScores(){
		string scoreText = "";
		string dateText = "";
		GetHighScores().ToList().ForEach(x => scoreText += x.Split('\t')[0] + "\n");
		GetHighScores().ToList().ForEach(x => dateText += x.Split('\t')[1] + "\n");

		FindObjectOfType<HighScoresHandler>().scoresText.text = scoreText;
		FindObjectOfType<HighScoresHandler>().datesText.text = dateText;
	}

	[MenuItem("Custom/Scores/Seed Scores")]
	public static void SeedScores(){
		for (int i = 0; i < scoresCount; i++){
			AddHighScore(i);
		}
	}

	[MenuItem("Custom/Scores/Clear Scores")]
	public static void ClearScores(){
		for (int i = 0; i < scoresCount; i++){
			PlayerPrefs.SetString(highScoreString + i, "");
		}
	}
	#endif

}
