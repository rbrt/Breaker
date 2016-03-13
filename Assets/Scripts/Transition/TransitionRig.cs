﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;

public class TransitionRig : MonoBehaviour {

	static TransitionRig instance;

	public static TransitionRig Instance {
		get {
			return instance;
		}
	}

	[SerializeField] protected GameObject gameplayTransitionRig;
	[SerializeField] protected GameObject menuTransitionRig;

    [SerializeField] protected Camera gameplayUI;
	[SerializeField] protected Camera gameplayGame;
	[SerializeField] protected Camera menuUI;
	[SerializeField] protected Camera viewportCamera;

	HandleTransition transitionHandler;

	public Camera GameplayUICamera {
		get {
			return gameplayUI;
		}
	}

	void Awake(){
		if (instance == null){
			instance = this;
			transitionHandler = GetComponentInChildren<HandleTransition>();
		}
		else {
			Destroy(this.gameObject);
			Debug.Log("Destroyed duplicate instance of TransitionRig");
		}
	}

	Camera GetChildCameraForRole(GameObject parent, Enums.CameraRoles role){
		return parent.GetComponentsInChildren<CameraRole>()
					 .FirstOrDefault(child => child.role == role)
					 .GetComponent<Camera>();
	}

	public void TransitionFromMenuToGameplay(){
		SceneManager.LoadScene("Prototyping", LoadSceneMode.Additive);

		this.StartSafeCoroutine(Handoff());
	}

	bool renderTransition = false;

	void Update(){
		if (renderTransition){
			menuUI.Render();
			gameplayUI.Render();
			gameplayGame.Render();
		}
	}

	IEnumerator Handoff(){
		while (CameraManager.Instance.GameCamera == null){
			yield return null;
		}

		renderTransition = true;

		yield return this.StartSafeCoroutine(SetUpGameTransitionElements());

		var menuCam = MenuTransitionSetup.Instance.MenuCamera;
		var menuCanvas = MenuTransitionSetup.Instance.MenuCanvas;

		var rt = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
        rt.Create();

		menuUI.targetTexture = rt;
		transitionHandler.SetMenuTexture(rt);

		yield return new WaitForEndOfFrame();
		menuCanvas.worldCamera = menuUI;
	    viewportCamera.enabled = true;

		yield return this.StartSafeCoroutine(transitionHandler.TransitionToA(time: 1.5f));

		gameplayUI.enabled = false;
		gameplayGame.enabled = false;
		menuUI.enabled = false;
		viewportCamera.enabled = false;

		Destroy(MenuTransitionSetup.Instance.gameObject);
		yield return new WaitForEndOfFrame();
		GUIController.Instance.GetComponent<Canvas>().worldCamera = null;
	}

	IEnumerator SetUpGameTransitionElements(){
		var rt = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
        rt.Create();

		gameplayUI.targetTexture = rt;
		gameplayGame.targetTexture = rt;
		transitionHandler.SetGameplayTexture(rt);

		this.StartSafeCoroutine(FollowGameCamera(gameplayGame, CameraManager.Instance.GameCamera));

		while (GUIController.Instance == null){
			yield return null;
		}

		var gameGUICanvas = GUIController.Instance.GetComponent<Canvas>();

		yield return new WaitForEndOfFrame();
		gameGUICanvas.worldCamera = gameplayUI;
	}

	IEnumerator FollowGameCamera(Camera transitionCamera, Camera gameCamera){
		while (true){
			transitionCamera.transform.position = gameCamera.transform.position;
			yield return null;
		}
	}
}
