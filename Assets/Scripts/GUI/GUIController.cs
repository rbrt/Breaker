using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIController : Singleton<GUIController> {

	[SerializeField] protected Canvas guiCanvas;
	[SerializeField] protected CanvasGroup gameplayCanvas;
	[SerializeField] protected CanvasGroup endOfLevelCanvas;

	protected override void Startup(){
		DontDestroyOnLoad(this.gameObject);
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
