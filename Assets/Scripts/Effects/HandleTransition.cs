using UnityEngine;
using System.Collections;

public class HandleTransition : MonoBehaviour {

	Material targetMat;

	bool transitioning = false;

	void Awake () {
		targetMat = new Material(GetComponent<Renderer>().material);
		GetComponent<Renderer>().material = targetMat;
	}

	public void SetMenuTexture(RenderTexture tex){
		targetMat.SetTexture("_TestTex2", tex);
	}

	public void SetGameplayTexture(RenderTexture tex){
		targetMat.SetTexture("_TestTex1", tex);
	}

	// Primes the material before the transition is started to get rid of any
	// issues persisting for a frame or two
	public void QueueTransitionMaterialForA()
	{
		targetMat.SetFloat("_Step", 1f);
	}

	public IEnumerator TransitionToA(float time = .5f){
		if (transitioning){
			yield break;
		}

		transitioning = true;

		for (float i = 0; i < 1; i += Time.deltaTime / time){
			targetMat.SetFloat("_Step", 1 - i);
			yield return null;
		}
		targetMat.SetFloat("_Step", 0);

		transitioning = false;
	}

	public IEnumerator TransitionToB(float time = .5f){
		if (transitioning){
			yield break;
		}

		transitioning = true;

		for (float i = 0; i < 1; i += Time.deltaTime / time){
			targetMat.SetFloat("_Step", i);
			yield return null;
		}
		targetMat.SetFloat("_Step", 1);

		transitioning = false;
	}
}
