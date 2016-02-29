using UnityEngine;
using System.Collections;

public class PlayerAttributes : MonoBehaviour {

	const float maxHealth = 10;

	float currentHealth;

	void Awake(){
		currentHealth = maxHealth;
	}

	public void AffectHealth(float amount){
		if (DebugMenu.Invincible){
			return;
		}

		currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
		PlayerAttributeDisplay.Instance.SetHealthPercentage(currentHealth / maxHealth);

		if (currentHealth <= 0){
			PlayerController.Instance.Die();
		}
	}

	public void FillHealth(){
		currentHealth = maxHealth;
		PlayerAttributeDisplay.Instance.SetHealthPercentage(currentHealth / maxHealth);
	}
}
