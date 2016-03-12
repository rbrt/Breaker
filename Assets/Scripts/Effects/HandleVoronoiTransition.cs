using UnityEngine;
using System.Collections;

public class HandleVoronoiTransition : MonoBehaviour {

	[SerializeField] protected AnimationCurve curve;
	[SerializeField] protected float effectDuration;

	Material targetMat;

	// Use this for initialization
	void Start () {
		targetMat = new Material(GetComponent<Renderer>().material);
		GetComponent<Renderer>().material = targetMat;

		this.StartSafeCoroutine(EvaluateCurve());
	}

	IEnumerator EvaluateCurve(){
		while (true){
			for (float i = 0; i < 1; i += Time.deltaTime / effectDuration){
				targetMat.SetFloat("_Step", i);
				yield return null;
			}
			targetMat.SetFloat("_Step", 1);

			yield return new WaitForSeconds(.25f);

			for (float i = 0; i < 1; i += Time.deltaTime / effectDuration){
				targetMat.SetFloat("_Step", 1 - i);
				yield return null;
			}
			targetMat.SetFloat("_Step", 0);
		}
	}
}
