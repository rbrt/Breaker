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

	float shieldDecrement = .05f;
	float healthDecrement = .05f;

	SafeCoroutine shieldCoroutine;
	SafeCoroutine healthCoroutine;

	public void SetShieldPercentage(float percentage){
		targetShield = percentage;

		if (shieldCoroutine == null || !shieldCoroutine.IsRunning){
			shieldCoroutine = this.StartSafeCoroutine(AffectShield());
		}
	}

	public void SetHealthPercentage(float percentage){
		targetHealth = percentage;

		if (healthCoroutine == null || !healthCoroutine.IsRunning){
			healthCoroutine = this.StartSafeCoroutine(AffectHealth());
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
		while (!Mathf.Approximately(currentShield, targetShield)){
			if (targetShield < currentShield){
				currentShield -= shieldDecrement;
			}
			else if (currentShield > targetShield){
				currentShield += shieldDecrement;
			}

			if (Mathf.Abs(currentShield - targetShield) < .05f){
				currentShield = targetShield;
			}
			shieldImage.fillAmount = currentShield;

			yield return null;
		}
		currentShield = targetShield;
		shieldImage.fillAmount = currentShield;
	}

	IEnumerator AffectHealth(){
		while (!Mathf.Approximately(currentHealth, targetHealth)){
			if (targetHealth < currentHealth){
				currentHealth -= healthDecrement;
			}
			else if (targetHealth > currentHealth){
				currentHealth += healthDecrement;
			}
			healthImage.fillAmount = currentHealth;
			yield return null;
		}
		currentHealth = targetHealth;
		healthImage.fillAmount = currentHealth;
	}
}
