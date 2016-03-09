using UnityEngine;
using System.Collections;

public class EndOfLevel : MonoBehaviour {

	static EndOfLevel instance;
	ParticleSystem portalParticles;

	void Awake(){
		instance = this;
		portalParticles = GetComponentInChildren<ParticleSystem>();
	}

	public static Vector3 GetPortalTarget(){
		return instance.portalParticles.transform.position;
	}
}
