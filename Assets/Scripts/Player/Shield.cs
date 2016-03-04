using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

	static Shield instance;

	public static Shield Instance {
		get {
			return instance;
		}
	}

	const float maxShield = 100;
	const float shieldDecay = 1f;
	const float shieldRegen = .05f;
	const float shieldWaitTime = .5f;

	MeshRenderer shieldRenderer;

	float maxAlpha = 0;
	float currentShield = 0;
	float lastShieldTime = 0;

	SafeCoroutine shieldCoroutine;

	bool lastShield = false;

	void Awake(){
		instance = this;

		currentShield = maxShield;
		lastShieldTime = Time.time;
		shieldRenderer = GetComponent<MeshRenderer>();
		shieldRenderer.material = new Material(shieldRenderer.material);
		var color = shieldRenderer.material.GetColor("_Color");
		maxAlpha = color.a;
		color.a = 0;
		shieldRenderer.material.SetColor("_Color", color);
	}

	void Update(){
		if (lastShield && !DebugMenu.InfiniteShield){
			currentShield -= shieldDecay;
		}
		else if (currentShield < maxShield && (Time.time - lastShieldTime > shieldWaitTime)){
			currentShield += shieldRegen;
		}

		if (currentShield <= 0){
			PlayerController.Instance.DisableShields();
			if (lastShield){
				LowerShield();
			}
		}
		else if (currentShield > maxShield){
			currentShield = maxShield;
		}

		PlayerAttributeDisplay.Instance.SetShieldPercentage(currentShield / maxShield);
	}

	void OnTriggerEnter(Collider other){
		var shot = other.gameObject.GetComponent<Shot>();
		if (shot != null){
			if (lastShield){
				shot.SetTargetDirection((other.transform.position - transform.position).normalized, GetComponent<Collider>());
			}
		}
	}

	void OnTriggerStay(Collider other){
		var enemy = other.gameObject.GetComponent<Enemy>();
		if (enemy != null){
			if (lastShield){
				enemy.ShieldDeath();
			}
		}
	}

	public void RaiseShield(){
		if (currentShield <= 0){
			return;
		}

		if (shieldCoroutine != null && shieldCoroutine.IsRunning && !lastShield){
			shieldCoroutine.Stop();
		}
		if (!lastShield){
			shieldCoroutine = this.StartSafeCoroutine(RaiseShieldAnimated());
		}
		lastShield = true;
	}

	public void LowerShield(){
		if (shieldCoroutine != null && shieldCoroutine.IsRunning && lastShield){
			shieldCoroutine.Stop();
		}
		if (lastShield){
			shieldCoroutine = this.StartSafeCoroutine(LowerShieldAnimated());
		}
		lastShield = false;
		lastShieldTime = Time.time;
	}

	public void FillShield(){
		PlayerAttributeDisplay.Instance.FillShield();
		currentShield = maxShield;
		lastShield = false;
		PlayerAttributeDisplay.Instance.SetShieldPercentage(currentShield / maxShield);
	}

	public void AddShield(float amount){
		currentShield += amount;
		if (currentShield > maxShield){
			currentShield = maxShield;
		}
	}

	IEnumerator RaiseShieldAnimated(){
		Color color = shieldRenderer.material.GetColor("_Color");
		for (float i = color.a; i < maxAlpha; i += Time.deltaTime){
			color.a = i;
			shieldRenderer.material.SetColor("_Color", color);
			yield return null;
		}

		color.a = maxAlpha;
		shieldRenderer.material.SetColor("_Color", color);
	}

	IEnumerator LowerShieldAnimated(){
		Color color = shieldRenderer.material.GetColor("_Color");
		for (float i = color.a; i > 0; i -= Time.deltaTime){
			color.a = i;
			shieldRenderer.material.SetColor("_Color", color);
			yield return null;
		}

		color.a = 0;
		shieldRenderer.material.SetColor("_Color", color);
	}


}
