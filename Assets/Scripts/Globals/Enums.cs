using UnityEngine;
using System.Collections;

public class Enums : MonoBehaviour {

	public enum EndOfRoundStates {
		Death,
		Victory
	}

	public enum CameraRoles {
		Viewport,
		Gameplay,
		Menu,
		GUI
	}

	public enum GUIRoles {
		Gameplay,
		Title,
		EndOfLevel
	}
}
