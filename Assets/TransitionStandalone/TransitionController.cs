using UnityEngine;
using System.Collections;

public class TransitionController : Singleton<TransitionController>
{

	[SerializeField] protected RenderTexture fromTexture;
	[SerializeField] protected RenderTexture toTexture;

	[SerializeField] protected Material displayMaterial;
	[SerializeField] protected Camera displayCamera;
	[SerializeField] protected MeshRenderer displayRenderer;

	bool transitioning = false;

	public RenderTexture FromTexture
	{
		get
		{
			return fromTexture;
		}
	}

	public RenderTexture ToTexture
	{
		get
		{
			return toTexture;
		}
	}

	void Update()
	{
		if (!transitioning)
		{
			SceneTransitionObject.currentSTO.RenderFrame();
		}
	}

	protected override void Startup()
	{
		displayMaterial.SetFloat("_Step", 0);
		displayRenderer.GetComponent<Renderer>().material = displayMaterial;
	}

	void RenderTransitionFrame()
	{
		SceneTransitionObject.nextSTO.EnableSceneRenderersAndLights(false);
		SceneTransitionObject.currentSTO.RenderFrame();

		SceneTransitionObject.nextSTO.EnableSceneRenderersAndLights(true);
		SceneTransitionObject.currentSTO.EnableSceneRenderersAndLights(false);

		SceneTransitionObject.nextSTO.RenderFrame();
		SceneTransitionObject.currentSTO.EnableSceneRenderersAndLights(true);
	}

	public IEnumerator TransitionToB(float transitionTime)
	{
		transitioning = true;
		RenderTransitionFrame();
		for (float i = 0; i < 1; i += Time.deltaTime / transitionTime)
		{
			RenderTransitionFrame();
			displayMaterial.SetFloat("_Step", i);
			yield return null;
		}
		RenderTransitionFrame();
		displayMaterial.SetFloat("_Step", 1);

		SceneTransitionObject.currentSTO.EnableSceneRenderersAndLights(false);
		SceneTransitionObject.nextSTO.SetAsActiveSTO();

		yield return null;
		transitioning = false;
	}


}
