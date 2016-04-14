using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIController : Singleton<GUIController> {

	[SerializeField] protected CanvasGroup gameplayCanvasGroup;
	[SerializeField] protected CanvasGroup endOfLevelCanvasGroup;
	[SerializeField] protected CanvasGroup titleCanvasGroup;

	protected override void Startup(){
		DontDestroyOnLoad(this.gameObject);
	}

	void Start(){
		gameplayCanvasGroup.GetComponent<Canvas>().worldCamera = TransitionRig.Instance.GameplayUICamera;
	}

	public Canvas GameplayCanvas {
		get {
			if (gameplayCanvasGroup != null){
				return gameplayCanvasGroup.GetComponent<Canvas>();
			}
			else {
				return null;
			}
		}
	}

	public Canvas TitleCanvas {
		get {
			if (titleCanvasGroup != null){
				return titleCanvasGroup.GetComponent<Canvas>();
			}
			else {
				return null;
			}
		}
	}

	public Canvas EndOfLevelCanvas {
		get {
			if (endOfLevelCanvasGroup != null){
				return endOfLevelCanvasGroup.GetComponent<Canvas>();
			}
			else {
				return null;
			}
		}
	}

	public void ShowTitleCanvasNoSideEffects(){
		EnableCanvasGroup(titleCanvasGroup);
	}

	public void ShowGameplayCanvasNoSideEffects(){
		EnableCanvasGroup(gameplayCanvasGroup);
	}

	public void ShowEndOfLevelCanvasNoSideEffects(){
		EnableCanvasGroup(endOfLevelCanvasGroup);
	}

	public void ShowTitleCanvas(){
		EnableCanvasGroup(titleCanvasGroup);

		DisableCanvasGroup(endOfLevelCanvasGroup);
		DisableCanvasGroup(gameplayCanvasGroup);
	}

	public void ShowGameplayCanvas(){
		EnableCanvasGroup(gameplayCanvasGroup);

		DisableCanvasGroup(endOfLevelCanvasGroup);
		DisableCanvasGroup(titleCanvasGroup);
	}

	public void ShowEndOfLevelCanvas(){
		EnableCanvasGroup(endOfLevelCanvasGroup);

		DisableCanvasGroup(gameplayCanvasGroup);
		DisableCanvasGroup(titleCanvasGroup);
	}

	void EnableCanvasGroup(CanvasGroup group){
		group.alpha = 1;
		group.interactable = true;
		group.blocksRaycasts = true;
	}

	void DisableCanvasGroup(CanvasGroup group){
		group.alpha = 0;
		group.interactable = false;
		group.blocksRaycasts = false;
	}
}
