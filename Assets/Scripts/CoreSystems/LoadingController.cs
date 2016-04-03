using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingController : Singleton<LoadingController> {

	const string titleScene = "Title";
	const string gameplayScene = "Prototyping";
	const string transitionScene = "LevelTransition";


	public static void LoadTitleScene(){
		SceneManager.LoadScene(titleScene, LoadSceneMode.Single);
	}

	public static void LoadGameplayScene(bool additive = false){
		if (additive){
			SceneManager.LoadScene(gameplayScene, LoadSceneMode.Additive);
		}
		else{
			SceneManager.LoadScene(gameplayScene, LoadSceneMode.Single);
		}
	}

	public static void UnloadGameplayScene(){
		SceneManager.UnloadScene(gameplayScene);
	}

	public static void LoadEndOfRoundScene(bool additive = false){
		if (additive){
			SceneManager.LoadScene(transitionScene, LoadSceneMode.Additive);
		}
		else{
			SceneManager.LoadScene(transitionScene, LoadSceneMode.Single);
		}
	}

	public static IEnumerator SetGameplaySceneActiveWhenLoaded(){
		var scene = SceneManager.GetSceneByName(gameplayScene);
		while (!scene.isLoaded){
			scene = SceneManager.GetSceneByName(gameplayScene);
			yield return null;
		}
		SceneManager.SetActiveScene(scene);
	}

}
