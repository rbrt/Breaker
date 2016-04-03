using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T: MonoBehaviour {

	static T instance;

	const bool logVerbose = false;

	public static T Instance {
		get {
			return instance;
		}
	}

	void Awake(){
		if (instance == null || instance.gameObject == null){
			instance = GetComponent<T>();
			Startup();
		}
		else{
			if (logVerbose){
				Debug.Log("Destroyed duplicate instance of " + typeof(T).ToString());
			}
			
			Cleanup();
			Destroy(this.gameObject);
		}
	}

	protected virtual void Cleanup(){

	}

	protected virtual void Startup(){

	}

}
