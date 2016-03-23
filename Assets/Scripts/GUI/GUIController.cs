using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIController : MonoBehaviour {

	static GUIController instance;

	public static GUIController Instance {
		get {
			return instance;
		}
	}

	[SerializeField] protected Canvas guiCanvas;
	[SerializeField] protected CanvasGroup gameplayCanvas;
	[SerializeField] protected CanvasGroup endOfLevelCanvas;


	void Awake () {
		instance = this;
	}

	public Canvas GUICanvas {
		get {
			return guiCanvas;
		}
	}

	public void ShowGameplayCanvas(){
		gameplayCanvas.alpha = 1;
		gameplayCanvas.interactable = true;

		endOfLevelCanvas.alpha = 0;
		endOfLevelCanvas.interactable = false;
	}

	public void ShowEndOfLevelCanvas(){
		endOfLevelCanvas.alpha = 1;
		endOfLevelCanvas.interactable = true;

		gameplayCanvas.alpha = 0;
		gameplayCanvas.interactable = false;
	}
}
