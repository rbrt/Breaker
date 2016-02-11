using UnityEngine;
using System.Collections;

public class PlayerAttributes : MonoBehaviour {

	const float maxHealth = 5;
	const float maxShield = 10;

	float currentHealth,
		  currentShield;

	void Awake(){
		currentHealth = maxHealth;
		currentShield = maxShield;
	}

	public void AffectHealth(float amount){
		currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
		PlayerAttributeDisplay.Instance.SetHealthPercentage(currentHealth / maxHealth);
	}

	public void AffectShield(float amount){
		currentShield = Mathf.Clamp(currentShield + amount, 0, maxShield);
		PlayerAttributeDisplay.Instance.SetHealthPercentage(currentShield / maxShield);
	}
}
