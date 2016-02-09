using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerAttributeDisplay : MonoBehaviour {

	static PlayerAttributeDisplay instance;

	public static PlayerAttributeDisplay Instance{
		get {
			return instance;
		}
	}

	[SerializeField] protected Image shieldImage;
	[SerializeField] protected Image healthImage;

	float targetShield = 1;
	float targetHealth = 1;
	float currentShield = 1;
	float currentHealth = 1;

	float shieldDecrement = .1f;
	float healthDecrement = .1f;

	SafeCoroutine shieldCoroutine;
	SafeCoroutine healthCoroutine;

	public void SetShieldPercentage(float percentage){
		targetShield = percentage;
		if (shieldCoroutine == null && !shieldCoroutine.IsRunning){
			this.StartSafeCoroutine(AffectShield());
		}
	}

	public void SetHealthPercentage(float percentage){
		targetHealth = percentage;
		if (healthCoroutine == null && !healthCoroutine.IsRunning){
			this.StartSafeCoroutine(AffectHealth());
		}
	}

	void Awake(){
		if (instance == null){
			instance = this;
		}
		else{
			Destroy(this.gameObject);
		}
	}

	IEnumerator AffectShield(){
		while (currentShield != targetShield){
			currentShield += Mathf.Sign(currentShield - targetShield) * shieldDecrement;
			yield return null;
		}
	}

	IEnumerator AffectHealth(){
		while (currentHealth != targetHealth){
			currentHealth += Mathf.Sign(currentHealth - targetHealth) * healthDecrement;
			yield return null;
		}
	}
}
