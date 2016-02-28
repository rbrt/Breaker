using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;

public class PlayerController : MonoBehaviour {

	enum TouchTypes {TouchLeft, TouchUp, TouchRight, TouchDown, None}

	static PlayerController instance;

	[SerializeField] protected Shield shield;
	[SerializeField] protected ParticleSystem deathParticles;
	[SerializeField] protected PlayerAnimatorController playerAnimatorController;

	const float yDeathValue = -6;

	public static PlayerController Instance {
		get {
			return instance;
		}
	}

	public bool Shielding {
		get {
			return shielding;
		}
	}

	public bool Dead {
		get {
			return dead;
		}
	}

	bool jumping = false,
		 movingLeft = false,
		 movingRight = false,
		 //dashing = false,
		 shielding = false,
		 onGround = false,
		 lastShield = false,
		 dead = false;

	Vector3 forceVector,
			jumpVector;

	float moveSpeed = 15f,
		  currentMoveSpeed = 0,
		  moveAcceleration = 2f,
		  moveDeceleration = 1.5f,
		  gravity = 15f,
		  jumpSpeed = 40f,
		  currentJumpSpeed = 0,
		  jumpAcceleration = 3f,
		  jumpDecay = 8f;

	Collider lastHit;

	PlayerAttributes playerAttributes;

	SafeCoroutine jumpingCoroutine,
				  fallTestCoroutine;

	Touch[] touches;
	TouchTypes[] touchTypes;

	bool pressedUp = false;
	bool pressedRight = false;
	bool pressedLeft = false;
	bool pressedDown = false;
	bool releasedUp = false;
	bool releasedRight = false;
	bool releasedLeft = false;
	bool releasedDown = false;

	void Awake () {
		forceVector = Vector3.zero;
		jumpVector = Vector3.zero;
		playerAttributes = GetComponent<PlayerAttributes>();
		touchTypes = new TouchTypes[5];

		for (int i = 0; i < 5; i++){
			touchTypes[i] = TouchTypes.None;
		}

		instance = this;
	}

	void Update () {
		touches = Input.touches;

		if (!dead){
			HandleInput();
		}

		MovePlayer();
		HandleShields();

		if (transform.position.y < yDeathValue){
			Die();
		}
	}

	bool PressedRight(){
		#if UNITY_EDITOR
		return Input.GetKeyDown(KeyCode.D);
		#else
		bool temp = pressedRight;
		pressedRight = false;
		return temp;
		#endif
	}

	bool PressedLeft(){
		#if UNITY_EDITOR
		return Input.GetKeyDown(KeyCode.A);
		#else
		bool temp = pressedLeft;
		pressedLeft = false;
		return temp;
		#endif
	}

	bool PressedUp(){
		#if UNITY_EDITOR
		return Input.GetKeyDown(KeyCode.W);
		#else
		bool temp = pressedUp;
		pressedUp = false;
		return temp;
		#endif
	}

	bool PressedDown(){
		#if UNITY_EDITOR
		return Input.GetKeyDown(KeyCode.S);
		#else
		bool temp = pressedDown;
		pressedDown = false;
		return temp;
		#endif
	}

	bool ReleasedRight(){
		#if UNITY_EDITOR
		return Input.GetKeyUp(KeyCode.D);
		#else
		bool temp = releasedRight;
		releasedRight = false;
		return temp;
		#endif
	}

	bool ReleasedLeft(){
		#if UNITY_EDITOR
		return Input.GetKeyUp(KeyCode.A);
		#else
		bool temp = releasedLeft;
		releasedLeft = false;
		return temp;
		#endif
	}

	bool ReleasedUp(){
		#if UNITY_EDITOR
		return Input.GetKeyUp(KeyCode.W);
		#else
		bool temp = releasedUp;
		releasedUp = false;
		return temp;
		#endif
	}

	bool ReleasedDown(){
		#if UNITY_EDITOR
		return Input.GetKeyUp(KeyCode.S);
		#else
		bool temp = releasedDown;
		releasedDown = false;
		return temp;
		#endif
	}

	void HandleInput(){
		if (PressedUp()){
			jumping = true;
			playerAnimatorController.StartJump();
		}
		if (PressedLeft()){
			movingLeft = true;
			playerAnimatorController.StartRun();
			playerAnimatorController.RotateCharacterToFaceLeft();
		}
		if (PressedDown()){
			shielding = true;
		}
		if (PressedRight()){
			movingRight = true;
			playerAnimatorController.StartRun();
			playerAnimatorController.RotateCharacterToFaceRight();
		}

		if (ReleasedUp()){
			jumping = false;
			if (jumping){
				playerAnimatorController.StopJump();
			}
		}
		if (ReleasedLeft()){
			movingLeft = false;
			if (!movingRight){
				playerAnimatorController.StopRun();
			}
			playerAnimatorController.RotateCharacterToFaceRight();
		}
		if (ReleasedDown()){
			shielding = false;
		}
		if (ReleasedRight()){
			movingRight = false;
			if (!movingLeft){
				playerAnimatorController.StopRun();
			}
		}
	}

	public void InputPressedLeft(){
		pressedLeft = true;
	}

	public void InputReleasedLeft(){
		releasedLeft = true;
	}

	public void InputPressedRight(){
		pressedRight = true;
	}

	public void InputReleasedRight(){
		releasedRight = true;
	}

	public void InputPressedUp(){
		pressedUp = true;
	}

	public void InputReleasedUp(){
		releasedUp = true;
	}

	public void InputPressedDown(){
		pressedDown = true;
	}

	public void InputReleasedDown(){
		releasedDown = true;
	}

	void HandleTouches(){
		//
		//
		// for (int i = 0; i < touches.Length; i++){
		// 	if (touches[i].phase == TouchPhase.Began){
		// 		if (touches[i].position.y > Screen.height * .5f){
		// 			pressedUp = true;
		// 			touchTypes[i] = TouchTypes.TouchUp;
		// 		}
		// 		else{
		// 			if (touches[i].position.x < Screen.width * .25f){
		// 				pressedLeft = true;
		// 				touchTypes[i] = TouchTypes.TouchLeft;
		// 			}
		// 			else if (touches[i].position.x > Screen.width * .75f){
		// 				pressedRight = true;
		// 				touchTypes[i] = TouchTypes.TouchRight;
		// 			}
		// 			else if (touches[i].position.y < Screen.height * .5f){
		// 				pressedDown = true;
		// 				touchTypes[i] = TouchTypes.TouchDown;
		// 			}
		// 		}
		// 	}
		// 	else if (touches[i].phase == TouchPhase.Canceled ||
		// 			 touches[i].phase == TouchPhase.Ended)
		// 	{
		// 		if (touchTypes[i] == TouchTypes.TouchUp){
		// 			releasedUp = true;
		// 		}
		// 		else if (touchTypes[i] == TouchTypes.TouchLeft){
		// 			releasedLeft = true;
		// 		}
		// 		else if (touchTypes[i] == TouchTypes.TouchRight){
		// 			releasedRight = true;
		// 		}
		// 		else if (touchTypes[i] == TouchTypes.TouchDown){
		// 			releasedDown = true;
		// 		}
		//
		// 		touchTypes[i] = TouchTypes.None;
		// 	}
		// }
		//
		// if (pressedUp){
		// 	jumping = true;
		// 	playerAnimatorController.StartJump();
		// }
		// if (pressedLeft){
		// 	movingLeft = true;
		// 	playerAnimatorController.StartRun();
		// 	playerAnimatorController.RotateCharacterToFaceLeft();
		// }
		// if (pressedDown){
		// 	shielding = true;
		// }
		// if (pressedRight){
		// 	movingRight = true;
		// 	playerAnimatorController.StartRun();
		// 	playerAnimatorController.RotateCharacterToFaceRight();
		// }
		//
		// if (releasedUp){
		// 	jumping = false;
		// 	if (jumping){
		// 		playerAnimatorController.StopJump();
		// 	}
		// }
		// if (releasedLeft){
		// 	movingLeft = false;
		// 	if (!movingRight){
		// 		playerAnimatorController.StopRun();
		// 	}
		// 	playerAnimatorController.RotateCharacterToFaceRight();
		// }
		// if (releasedDown){
		// 	shielding = false;
		// }
		// if (releasedRight){
		// 	movingRight = false;
		// 	if (!movingLeft){
		// 		playerAnimatorController.StopRun();
		// 	}
		// }
	}

	void MovePlayer(){
		forceVector = Vector3.zero;

		if (movingRight){
			currentMoveSpeed = Mathf.Min(currentMoveSpeed + moveAcceleration, moveSpeed);
		}
		else if (movingLeft){
			currentMoveSpeed = Mathf.Max(currentMoveSpeed - moveAcceleration, -moveSpeed);
		}
		else{
			if (currentMoveSpeed < 0){
				currentMoveSpeed = Mathf.Min(currentMoveSpeed + moveDeceleration, 0);
			}
			else{
				currentMoveSpeed = Mathf.Max(currentMoveSpeed - moveDeceleration, 0);
			}
		}

		forceVector = Vector3.right * currentMoveSpeed * Time.smoothDeltaTime;

		if (!onGround){
			forceVector += Vector3.down * gravity * Time.smoothDeltaTime;
		}

		if (onGround && jumping && (jumpingCoroutine == null || !jumpingCoroutine.IsRunning)){
			onGround = false;
			jumpingCoroutine = this.StartSafeCoroutine(Jump());
		}

		if (!jumping){
			if (jumpingCoroutine != null && jumpingCoroutine.IsRunning){
				jumpingCoroutine.Stop();
			}
			currentJumpSpeed = Mathf.Max(currentJumpSpeed - jumpDecay, 0);
		}

		jumpVector = Vector3.up * currentJumpSpeed * Time.smoothDeltaTime;

		forceVector += jumpVector;
		transform.position = Vector3.MoveTowards(transform.position, transform.position + forceVector, .5f);
	}

	void HandleShields(){
		if (shielding){
			if (!lastShield){
				shield.RaiseShield();
				lastShield = true;
			}
		}
		else{
			if (lastShield){
				shield.LowerShield();
				DisableShields();
			}
		}
	}

	void DisableMovement(){
		movingLeft = false;
		movingRight = false;
		jumping = false;
		shielding = false;
		if (jumpingCoroutine != null && jumpingCoroutine.IsRunning){
			jumpingCoroutine.Stop();
			jumpVector = Vector3.zero;
		}
	}

	public void DisableShields(){
		shielding = false;
		lastShield = false;
	}

	public void HitGround(){
		if (!onGround){
			playerAnimatorController.HitGround();
		}

		onGround = true;
		jumping = false;
		if (jumpingCoroutine != null && jumpingCoroutine.IsRunning){
			jumpingCoroutine.Stop();
			jumpVector = Vector3.zero;
		}
	}

	public void Die(){
		if (!dead){
			dead = true;
			DisableShields();
			DisableMovement();

			CameraController.Instance.StopScrollingCamera();

			this.StartSafeCoroutine(WaitOnPlayerDeathThenQuitToTitle());
		}
	}

	IEnumerator Jump(){
		currentJumpSpeed = 20;
		while (currentJumpSpeed < jumpSpeed){
			currentJumpSpeed = Mathf.Min(currentJumpSpeed + jumpAcceleration, jumpSpeed);
			yield return null;
		}

		while (jumpVector.y > 0){
			currentJumpSpeed = Mathf.Max(currentJumpSpeed - jumpDecay, 0);
			yield return null;
		}

		playerAnimatorController.StopJump();
	}

	void OnCollisionEnter(Collision other){
		var ground = other.gameObject.GetComponent<Ground>();
		var platform = other.gameObject.GetComponent<Platform>();

		if (lastHit != other.collider){
			if (fallTestCoroutine != null && fallTestCoroutine.IsRunning){
				fallTestCoroutine.Stop();
			}

			if (ground != null){
				if (other.contacts[0].normal == Vector3.up){
					lastHit = other.collider;
					HitGround();

					var temp = transform.position;
					temp.y = other.transform.localScale.y / 2 + other.transform.position.y + transform.localScale.y / 2;
					transform.position = temp;
				}
			}
			else if (platform != null){
				if (other.contacts[0].normal == Vector3.up){
					lastHit = other.collider;
					HitGround();

					var temp = transform.position;
					temp.y = other.transform.localScale.y / 2 + other.transform.position.y + transform.localScale.y / 2;
					transform.position = temp;
				}
			}
		}

		var enemy = other.gameObject.GetComponent<Enemy>();
		if (enemy != null){
			playerAttributes.AffectHealth(enemy.ContactDamage);
		}
	}

	void OnTriggerEnter(Collider other){
		var shot = other.gameObject.GetComponent<Shot>();
		if (shot != null && !shielding){
			if (!shot.Destroyed){
				shot.HitPlayer();
				playerAttributes.AffectHealth(-1);
			}
		}
	}

	void OnCollisionExit(Collision other){
		if (other.collider == lastHit){
			lastHit = null;
			fallTestCoroutine = this.StartSafeCoroutine(WaitBeforeFalling(other));
		}
	}

	IEnumerator WaitBeforeFalling(Collision other){
		yield return null;
		onGround = false;
	}

	IEnumerator WaitOnPlayerDeathThenQuitToTitle(){
		yield return this.StartSafeCoroutine(PlayerDeath());

		yield return new WaitForSeconds(1);
		SceneManager.LoadScene("Title", LoadSceneMode.Single);
	}

	IEnumerator PlayerDeath(){
		deathParticles.gameObject.SetActive(true);
		playerAnimatorController.PlayerDeath();
		while (deathParticles.IsAlive()){
			yield return null;
		}
	}
}
