using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerAttributeDisplay : Singleton<PlayerAttributeDisplay> {

	[SerializeField] protected Image shieldImage;
	[SerializeField] protected Image healthImage;

	float targetShield = 1;
	float targetHealth = 1;
	float currentShield = 1;
	float currentHealth = 1;

	float shieldDecrement = .0075f;
	float shieldIncrement = .0075f;

	float healthDecrement = .05f;

	SafeCoroutine healthCoroutine;

	public void SetShieldPercentage(float percentage){
		targetShield = percentage;
	}

	public void SetHealthPercentage(float percentage){
		targetHealth = percentage;

		if (healthCoroutine == null || !healthCoroutine.IsRunning){
			healthCoroutine = this.StartSafeCoroutine(AffectHealth());
		}
	}

	public void FillShield(){
		currentShield = 1;
	}

	void Update(){
		if (currentShield < targetShield){
			currentShield += shieldIncrement;
		}
		else if (currentShield > targetShield){
			currentShield -= shieldDecrement;
		}

		if (Mathf.Abs(currentShield - targetShield) < .01f){
			currentShield = targetShield;
		}

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
