using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneTransitionObject : MonoBehaviour
{

	public static SceneTransitionObject currentSTO;
	public static SceneTransitionObject nextSTO;

	[SerializeField] protected Camera[] sceneCameras;
	[SerializeField] protected Canvas[] sceneCanvases;
	[SerializeField] protected Light[] sceneLights;

	Renderer[] renderersInScene;

	void Awake()
	{
		for (int i = 0; i < sceneCameras.Length; i++)
		{
			sceneCameras[i].enabled = false;
		}
		this.StartCoroutine(WaitForTransitionControllerThenSet());
		renderersInScene = GetComponentsInChildren<Renderer>();
	}

	public void EnableSceneRenderersAndLights(bool enable)
	{
		for (int i = 0; i < renderersInScene.Length; i++)
		{
			renderersInScene[i].enabled = enable;
		}

		for (int i = 0; i < sceneLights.Length; i++)
		{
			sceneLights[i].enabled = enable;
		}
	}

	public void RenderFrame()
	{
		for (int i = 0; i < sceneCameras.Length; i++)
		{
			sceneCameras[i].Render();
		}
	}

	public void SetAsActiveSTO()
	{
		currentSTO = this;
		nextSTO = null;
	}

	IEnumerator WaitForTransitionControllerThenSet()
	{
		if (currentSTO == null)
		{
			currentSTO = this;
			for (int i = 0; i < sceneCameras.Length; i++)
			{
				while (TransitionController.Instance == null)
				{
					yield return null;
				}
				sceneCameras[i].targetTexture = TransitionController.Instance.FromTexture;
			}

		}
		else
		{
			nextSTO = this;

			for (int i = 0; i < sceneLights.Length; i++)
			{
				sceneLights[i].enabled = false;
			}

			for (int i = 0; i < sceneCameras.Length; i++)
			{
				while (TransitionController.Instance == null)
				{
					yield return null;
				}
				sceneCameras[i].targetTexture = TransitionController.Instance.ToTexture;
			}
		}
	}

}
