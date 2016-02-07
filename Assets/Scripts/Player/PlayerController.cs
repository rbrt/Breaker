using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	static PlayerController instance;

	public static PlayerController Instance {
		get {
			return instance;
		}
	}

	bool jumping = false,
		 movingLeft = false,
		 movingRight = false,
		 dashing = false,
		 shielding = false,
		 onGround = false;

	Vector3 forceVector;

	float moveSpeed = 2.5f,
		  gravity = 3;

	void Awake () {
		forceVector = Vector3.zero;
		instance = this;
	}

	void Update () {
		HandleInput();
		MovePlayer();
	}

	void HandleInput(){
		if (Input.GetKeyDown(KeyCode.W)){
			jumping = true;
		}
		if (Input.GetKeyDown(KeyCode.A)){
			movingLeft = true;
		}
		if (Input.GetKeyDown(KeyCode.S)){
			shielding = true;
		}
		if (Input.GetKeyDown(KeyCode.D)){
			movingRight = true;
		}

		if (Input.GetKeyUp(KeyCode.W)){
			jumping = false;
		}
		if (Input.GetKeyUp(KeyCode.A)){
			movingLeft = false;
		}
		if (Input.GetKeyUp(KeyCode.S)){
			shielding = false;
		}
		if (Input.GetKeyUp(KeyCode.D)){
			movingRight = false;
		}
	}

	void MovePlayer(){
		if (movingRight){
			forceVector = Vector3.right * moveSpeed * 2 * Time.smoothDeltaTime;
		}
		else if (movingLeft){
			forceVector = Vector3.right * moveSpeed * .5f * Time.smoothDeltaTime;
		}
		else{
			forceVector = Vector3.right * moveSpeed * Time.smoothDeltaTime;
		}

		if (!onGround){
			forceVector += Vector3.down * gravity * Time.smoothDeltaTime;
		}


		transform.position = Vector3.MoveTowards(transform.position, transform.position + forceVector, .5f);

	}

	public void HitGround(){
		onGround = true;
	}
}
