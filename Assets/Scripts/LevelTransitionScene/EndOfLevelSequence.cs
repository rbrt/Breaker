using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EndOfLevelSequence : MonoBehaviour {

	static EndOfLevelSequence instance;

	public static EndOfLevelSequence Instance {
		get {
			return instance;
		}
	}

	[SerializeField] protected GameObject transitionPlayer;
	[SerializeField] protected Animator transitionPlayerAnimator;

	const float offscreenXLeft = -7;
	const float offscreenXRight = 7;
	const float offscreenXCenter = .25f;

	float runSpeed = 1.5f;

	void Awake(){
		instance = this;
		var pos = transitionPlayer.transform.position;
		pos.x = offscreenXLeft;

		transitionPlayer.transform.position = pos;
		GUIController.Instance.ShowEndOfLevelCanvas();
	}

	void Start () {
		this.StartSafeCoroutine(MovePlayerToCenterOfScreen());
	}

	IEnumerator MovePlayerToCenterOfScreen(){
		var currentPos = transitionPlayer.transform.position;
		var targetPos = currentPos;
		targetPos.x = offscreenXCenter;

		for (float i = 0; i < 1; i += Time.deltaTime / runSpeed){
			transitionPlayer.transform.position = Vector3.Lerp(currentPos, targetPos, i);
			yield return null;
		}

		transitionPlayerAnimator.SetBool("Running", false);
	}

	public IEnumerator LeaveScreenAndAdvanceLevel(){
		this.StartSafeCoroutine(MovePlayerOffScreen());
		yield return new WaitForSeconds(runSpeed * .5f);

		// Transition to gameplay
		TransitionRig.Instance.TransitionFromEndOfRoundToGameplay();
	}

	IEnumerator MovePlayerOffScreen(){
		var currentPos = transitionPlayer.transform.position;
		var targetPos = currentPos;
		targetPos.x = offscreenXRight;

		transitionPlayerAnimator.SetBool("Running", true);

		for (float i = 0; i < 1; i += Time.deltaTime / runSpeed){
			transitionPlayer.transform.position = Vector3.Lerp(currentPos, targetPos, i);
			yield return null;
		}
	}
}
