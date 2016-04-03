using UnityEngine;
using System.Collections;

public class EndOfLevel : Singleton<EndOfLevel> {

	ParticleSystem portalParticles;

	protected override void Startup(){
		portalParticles = GetComponentInChildren<ParticleSystem>();
	}

	public static Vector3 GetPortalTarget(){
		return Instance.portalParticles.transform.position;
	}
}
