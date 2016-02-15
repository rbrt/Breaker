using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	static CameraController instance;

	public static CameraController Instance{
		get {
			return instance;
		}
	}

	static float cameraSpeed = 3f;

	public static float CameraSpeed{
		get {
			return cameraSpeed;
		}
	}

	public static Vector3 ScreenPoint(Vector3 target){
		return instance.targetCamera.WorldToScreenPoint(target);
	}

	PlayerController player;
	Camera targetCamera;

	float horizontalThreshold = 15f;

	void Awake(){
		if (instance == null){
			instance = this;
		}
	}

	void Start () {
		targetCamera = GetComponent<Camera>();
		player = PlayerController.Instance;
	}

	void Update () {
		Vector3 playerPos = targetCamera.WorldToScreenPoint(player.transform.position);

		if (playerPos.x - horizontalThreshold < 0){

		}
		else if (playerPos.x + horizontalThreshold > targetCamera.pixelWidth){

		}

		transform.position = Vector3.MoveTowards(
								transform.position,
								transform.position + Vector3.right * cameraSpeed * Time.smoothDeltaTime,
								5f
							 );
	}
}
