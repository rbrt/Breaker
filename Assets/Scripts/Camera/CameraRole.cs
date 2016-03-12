using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraRole : MonoBehaviour {

	public Enums.CameraRoles role;

	void Start(){
		CameraManager.RegisterForRole(GetComponent<Camera>(), role);
	}
}
