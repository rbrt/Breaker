using UnityEngine;
using System.Collections;

public class CameraController : Singleton<CameraController> {

	static float cameraSpeed = 3f;

	public static float CameraSpeed{
		get {
			return cameraSpeed;
		}
	}

	public static Vector3 ScreenPoint(Vector3 target){
		return Instance.targetCamera.WorldToScreenPoint(target);
	}

	public static Vector3 OffsetPastRightScreenEdge(float offset){
		return Instance.targetCamera.ScreenToWorldPoint(new Vector3(Instance.targetCamera.pixelWidth + offset,
																	0,
																	10));
	}

	public static bool IsOnscreen(Vector3 testPosition){
		var test = ScreenPoint(testPosition);
		return test.x > 0 && test.x < Instance.targetCamera.pixelWidth;
	}

	Camera targetCamera;

	bool cameraActive = false;
	bool panning = false;

	protected override void Startup(){
		cameraActive = true;
	}

	void Start () {
		targetCamera = GetComponent<Camera>();
	}

	void Update () {
		if (!cameraActive || LevelController.Instance.RoundOver){
			return;
		}

		LevelController.Instance.DistanceTravelled += cameraSpeed * Time.smoothDeltaTime;

		transform.position = Vector3.MoveTowards(
								transform.position,
								transform.position + Vector3.right * cameraSpeed * Time.smoothDeltaTime,
								5f
							 );
	}

	public void PanToPortal(){
		if (!panning){
			this.StartSafeCoroutine(PanToTarget(EndOfLevel.GetPortalTarget()));
		}
	}

	public void StopScrollingCamera(){
		cameraActive = false;
	}

	IEnumerator PanToTarget(Vector3 position){
		while (ScreenPoint(position).x > targetCamera.pixelWidth / 2){
			transform.position = Vector3.MoveTowards(
									transform.position,
									transform.position + Vector3.right * cameraSpeed * Time.smoothDeltaTime,
									5f
								 );
			yield return null;
		}

		panning = false;
	}
}
