using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransitionTest : MonoBehaviour {

	string sceneB = "TestSceneB";

	void Start () {
		this.StartCoroutine(TestTransition());
	}

	IEnumerator TestTransition()
	{
		yield return new WaitForSeconds(1);

		SceneManager.LoadScene(sceneB, LoadSceneMode.Additive);
		while (SceneTransitionObject.nextSTO == null)
		{
			yield return null;
		}

		yield return this.StartCoroutine(TransitionController.Instance.TransitionToB(3));
	}
}
