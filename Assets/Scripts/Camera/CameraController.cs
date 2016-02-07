using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	PlayerController player;
	Camera camera;

	float horizontalThreshold = 15f,
		  cameraSpeed = 3f;

	void Start () {
		camera = GetComponent<Camera>();
		player = PlayerController.Instance;
	}

	void Update () {
		Vector3 playerPos = camera.WorldToScreenPoint(player.transform.position);

		if (playerPos.x - horizontalThreshold < 0){
		}
		else if (playerPos.x + horizontalThreshold > camera.pixelWidth){

		}

		transform.position = Vector3.MoveTowards(
								transform.position,
								transform.position + Vector3.right * cameraSpeed * Time.smoothDeltaTime,
								5f
							 );
	}
}
