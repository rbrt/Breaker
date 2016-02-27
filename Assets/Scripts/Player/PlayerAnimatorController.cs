using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;


public class PlayerAnimatorController : MonoBehaviour {

	static string jumpBool = "Jumping";
	static string runBool = "Running";
	static string damageTrigger = "Damage";
	static string hitGroundTrigger = "HitGround";

	[SerializeField] protected Animator playerAnimator;
	[SerializeField] protected SkinnedMeshRenderer playerRenderer;

	Transform modelTransform;
	SafeCoroutine rotationCoroutine;

	Quaternion rightRotation;

	void Awake(){
		modelTransform = playerAnimator.GetComponent<Transform>();
		rightRotation = Quaternion.Euler(new Vector3(0,180,0));
	}

	public void StartJump(){
		playerAnimator.SetBool(jumpBool, true);
		playerAnimator.ResetTrigger(hitGroundTrigger);
	}

	public void StopJump(){
		playerAnimator.SetBool(jumpBool, false);
	}

	public void HitGround(){
		playerAnimator.SetTrigger(hitGroundTrigger);
	}

	public void TakeDamage(){
		playerAnimator.SetTrigger(damageTrigger);
	}

	public void StartRun(){
		playerAnimator.SetBool(runBool, true);
	}

	public void StopRun(){
		playerAnimator.SetBool(runBool, false);
	}

	public void RotateCharacterToFaceRight(){
		KillRotationCoroutine();
		rotationCoroutine = this.StartSafeCoroutine(AnimateFacingRight());
	}

	public void RotateCharacterToFaceLeft(){
		KillRotationCoroutine();
		rotationCoroutine = this.StartSafeCoroutine(AnimateFacingLeft());
	}

	public void PlayerDeath(){
		playerRenderer.enabled = false;
	}

	void KillRotationCoroutine(){
		if (rotationCoroutine != null && rotationCoroutine.IsRunning){
			rotationCoroutine.Stop();
		}
	}

	IEnumerator AnimateFacingRight(){
		var originalRotation = modelTransform.localRotation;
		for (float i = 0; i < 1; i += Time.deltaTime / .2f){
			modelTransform.localRotation = Quaternion.Slerp(originalRotation,
															rightRotation,
															i);
			yield return null;
		}
		modelTransform.localRotation = rightRotation;
	}

	IEnumerator AnimateFacingLeft(){
		var originalRotation = modelTransform.localRotation;
		for (float i = 0; i < 1; i += Time.deltaTime / .2f){
			modelTransform.localRotation = Quaternion.Slerp(originalRotation,
															Quaternion.identity,
															i);
			yield return null;
		}
		modelTransform.localRotation = Quaternion.identity;
	}
}
