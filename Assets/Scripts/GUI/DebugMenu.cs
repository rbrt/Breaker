using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;


public class DebugMenu : MonoBehaviour {

	[SerializeField] protected Color inactiveColor;
	[SerializeField] protected Color activeColor;

	[SerializeField] protected Image invincibleButton;
	[SerializeField] protected Image infiniteShieldButton;
	[SerializeField] protected Image enemiesDontAttackButton;
	[SerializeField] protected Image noEnemiesButton;
	[SerializeField] protected Image refillShieldButton;
	[SerializeField] protected Image refillHealthButton;

	static bool invincible = false;
	static bool infiniteShield = false;
	static bool enemiesDontAttack = false;
	static bool noEnemies = false;

	public static bool Invincible {
		get {
			return invincible;
		}
	}

	public static bool InfiniteShield{
		get {
			return infiniteShield;
		}
	}

	public static bool EnemiesDontAttack{
		get {
			return enemiesDontAttack;
		}
	}

	public static bool NoEnemies{
		get {
			return noEnemies;
		}
	}

	public void ToggleInvincible(){
		invincible = !invincible;

		if (invincible){
			invincibleButton.color = activeColor;
		}
		else{
			invincibleButton.color = inactiveColor;
		}
	}

	public void ToggleInfiniteShield(){
		infiniteShield = !infiniteShield;

		if (infiniteShield){
			infiniteShieldButton.color = activeColor;
		}
		else{
			infiniteShieldButton.color = inactiveColor;
		}
	}

	public void ToggleEnemiesDontAttack(){
		enemiesDontAttack = !enemiesDontAttack;

		if (enemiesDontAttack){
			enemiesDontAttackButton.color = activeColor;
		}
		else{
			enemiesDontAttackButton.color = inactiveColor;
		}
	}

	public void ToggleNoEnemies(){
		noEnemies = !noEnemies;

		if (noEnemies){
			if (EnemySpawner.Instance != null){
				EnemySpawner.Instance.ClearAllEnemies();
			}

			noEnemiesButton.color = activeColor;
		}
		else{
			noEnemiesButton.color = inactiveColor;
		}
	}

	public void RefillShield(){
		if (PlayerController.Instance != null){
			PlayerController.Instance.FillShield();
		}
	}

	public void RefillHealth(){
		if (PlayerController.Instance != null){
			PlayerController.Instance.FillHealth();
		}
	}

}
