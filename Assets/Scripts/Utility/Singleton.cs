using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour {

	static T instance;

	public static T Instance {
		get {
			return instance;
		}
	}

	void Awake(){
		if (instance == null){
			instance = GetComponent<T>();
			Startup();
		}
		else{
			Debug.Log("Destroyed duplicate instance of " + typeof(T).ToString());
			Cleanup();
			Destroy(this.gameObject);
		}
	}

	protected virtual void Cleanup(){

	}

	protected virtual void Startup(){

	}

}
