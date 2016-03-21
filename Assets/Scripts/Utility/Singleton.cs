using UnityEngine;
using System.Collections;

public class Singleton : MonoBehaviour {

	static Singleton instance;

	public static Singleton Instance {
		get {
			return instance;
		}
	}

	void Awake(){
		if (instance == null){
			instance = this;
			Startup();
		}
		else{
			Debug.Log("Destroyed duplicate instance of " + this.GetType().ToString());
			Cleanup();
			Destroy(this.gameObject);
		}
	}

	public virtual void Cleanup(){

	}

	public virtual void Startup(){

	}

}
