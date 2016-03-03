using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using UnityEditor.SceneManagement;


public class FastSceneSwitch : EditorWindow {

	static List<string> sceneNames;
	static List<string> filteredScenes;
	static string filterText = "";
	static string tempText = "";
	static int selectedIndex = 0;
	static FastSceneSwitch window;

	static Color cachedColor;
	static float delay;
	static bool justOpened;
	static bool close;

	[MenuItem ("File/Scene Switcher Window %t")]
	public static void SceneSwitcherWindow(){
		if (Application.isPlaying){
			return;
		}
		window = (FastSceneSwitch)EditorWindow.GetWindow (typeof (FastSceneSwitch));
		filterText = "";
		filteredScenes = sceneNames;
		delay = (float)EditorApplication.timeSinceStartup + .1f;
		justOpened = true;
		close = true;
		window.Show();
		FocusWindowIfItsOpen(typeof(FastSceneSwitch));
	}

	void OnGUI(){
		if (Application.isPlaying){
			window.Close();
			return;
		}
		if (sceneNames == null){
			sceneNames = Directory.GetFiles("Assets/Scenes/").Where(x => x.Contains(".unity") && !x.Contains(".meta")).ToList();
		}
		if (filteredScenes == null){
			filteredScenes = sceneNames;
		}

		if (EditorApplication.timeSinceStartup > delay){
			Event e = Event.current;
		   	if (e.keyCode == KeyCode.Return){
				delay = (float)EditorApplication.timeSinceStartup + 1f;
				if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()){
					EditorSceneManager.OpenScene(filteredScenes[selectedIndex], OpenSceneMode.Single);
					window.Close();
				}
			}
			else if (e.keyCode == KeyCode.Escape){
				delay = (float)EditorApplication.timeSinceStartup + .7f;
				window.Close();
			}
		}

		tempText = "";
		GUI.SetNextControlName("InputField");
		tempText = GUILayout.TextField(filterText);

		if (justOpened){
			GUI.FocusControl("InputField");
			justOpened = false;
		}
		if (!tempText.Equals(filterText)){
			filterText = tempText;
			string[] tokens = filterText.Split(' ').Where(token => !string.IsNullOrEmpty(token)).ToArray();
			filteredScenes = sceneNames.Where(sceneName => tokens.All(token => sceneName.ToLower().Contains(token))).ToList();
		}

		if (selectedIndex >= filteredScenes.Count){
			selectedIndex = filteredScenes.Count;
		}

		EditorGUILayout.BeginVertical(EditorStyles.textArea);
		for (int i = 0; i < filteredScenes.Count; i++){
			if (i == selectedIndex){
				cachedColor = GUI.color;
				GUI.color = Color.green;
			}

			GUILayout.Label(filteredScenes[i].Split('/')[2].Replace(".unity", ""), EditorStyles.boldLabel);

			if (i == selectedIndex){
				GUI.color = cachedColor;
			}
		}
		EditorGUILayout.EndVertical();
	}

	public void Update(){
		//Repaint();
	}

}
