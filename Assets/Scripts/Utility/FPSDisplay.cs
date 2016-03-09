using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FPSDisplay : MonoBehaviour {

	[SerializeField] protected Text text;

	float frameCount = 0;
	float dt = 0.0f;
	float fps = 0.0f;
	float updateRate = 2.0f;  // 4 updates per sec.

	void Update(){
		frameCount++;
		dt += Time.deltaTime;
		if (dt > 1.0/updateRate){
			 fps = frameCount / dt ;
			 frameCount = 0;
			 dt -= 1f/updateRate;
		}

		if (ShowFPS){
			text.text = (int)fps + " fps";
		}
		else{
			text.text = "";
		}
	}

	void Awake(){
		ShowFPS = false;
	}

	public static bool ShowFPS{
		get; set;
	}
}
