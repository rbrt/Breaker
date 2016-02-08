using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

	MeshRenderer shieldRenderer;
	float maxAlpha = 0;

	SafeCoroutine shieldCoroutine;

	bool lastShield = false;

	void Awake(){
		shieldRenderer = GetComponent<MeshRenderer>();
		shieldRenderer.material = new Material(shieldRenderer.material);
		var color = shieldRenderer.material.GetColor("_Color");
		maxAlpha = color.a;
		color.a = 0;
		shieldRenderer.material.SetColor("_Color", color);
	}

	void OnTriggerEnter(Collider other){
		var shot = other.gameObject.GetComponent<Shot>();
		if (shot != null){
			if (lastShield){
				shot.SetTargetDirection((other.transform.position - transform.position).normalized, GetComponent<Collider>());
			}
		}
	}

	public void RaiseShield(){
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
