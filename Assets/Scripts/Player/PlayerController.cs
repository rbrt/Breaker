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
		 dead = false,
		 damaged = false;

	Vector3 forceVector,
			jumpVector;

	float moveSpeed = 15f,
		  currentMoveSpeed = 0,
		  moveAcceleration = 50f,
		  moveDeceleration = 40f,
		  gravity = 15f,
		  jumpSpeed = 40f,
		  currentJumpSpeed = 0,
		  jumpAcceleration = 3f,
		  jumpDecay = 8f,
		  damageLength = 1f;

	Collider lastHit;

	PlayerAttributes playerAttributes;

	SafeCoroutine jumpingCoroutine,
				  fallTestCoroutine;

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
		instance = this;
	}

	void Update () {
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
		bool temp = pressedRight;
		pressedRight = false;

		#if UNITY_EDITOR
		return Input.GetKeyDown(KeyCode.D) || temp;
		#else
		return temp;
		#endif
	}

	bool PressedLeft(){
		bool temp = pressedLeft;
		pressedLeft = false;

		#if UNITY_EDITOR
		return Input.GetKeyDown(KeyCode.A) || temp;
		#else
		return temp;
		#endif
	}

	bool PressedUp(){
		bool temp = pressedUp;
		pressedUp = false;

		#if UNITY_EDITOR
		return Input.GetKeyDown(KeyCode.W) || temp;
		#else
		return temp;
		#endif
	}

	bool PressedDown(){
		bool temp = pressedDown;
		pressedDown = false;

		#if UNITY_EDITOR
		return Input.GetKeyDown(KeyCode.S) || temp;
		#else
		return temp;
		#endif
	}

	bool ReleasedRight(){
		bool temp = releasedRight;
		releasedRight = false;

		#if UNITY_EDITOR
		return Input.GetKeyUp(KeyCode.D) || temp;
		#else
		return temp;
		#endif
	}

	bool ReleasedLeft(){
		bool temp = releasedLeft;
		releasedLeft = false;

		#if UNITY_EDITOR
		return Input.GetKeyUp(KeyCode.A) || temp;
		#else
		return temp;
		#endif
	}

	bool ReleasedUp(){
		bool temp = releasedUp;
		releasedUp = false;

		#if UNITY_EDITOR
		return Input.GetKeyUp(KeyCode.W) || temp;
		#else
		return temp;
		#endif
	}

	bool ReleasedDown(){
		bool temp = releasedDown;
		releasedDown = false;

		#if UNITY_EDITOR
		return Input.GetKeyUp(KeyCode.S) || temp;
		#else
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

	public void FillHealth(){
		playerAttributes.FillHealth();
	}

	public void FillShield(){
		shield.FillShield();
	}

	void MovePlayer(){
		forceVector = Vector3.zero;

		if (movingRight){
			currentMoveSpeed = Mathf.Min(currentMoveSpeed + moveAcceleration * Time.smoothDeltaTime, moveSpeed);
		}
		else if (movingLeft){
			currentMoveSpeed = Mathf.Max(currentMoveSpeed - moveAcceleration * Time.smoothDeltaTime, -moveSpeed);
		}
		else{
			if (currentMoveSpeed < 0){
				currentMoveSpeed = Mathf.Min(currentMoveSpeed + moveDeceleration * Time.smoothDeltaTime, 0);
			}
			else{
				currentMoveSpeed = Mathf.Max(currentMoveSpeed - moveDeceleration * Time.smoothDeltaTime, 0);
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
			TakeDamage(enemy.ContactDamage);
		}
	}

	void OnTriggerEnter(Collider other){
		var shot = other.gameObject.GetComponent<Shot>();
		if (shot != null && !shielding){
			if (!shot.Destroyed){
				shot.HitPlayer();
				TakeDamage(-1);
			}
		}
	}

	void OnCollisionExit(Collision other){
		if (other.collider == lastHit){
			lastHit = null;
			fallTestCoroutine = this.StartSafeCoroutine(WaitBeforeFalling(other));
		}
	}

	void TakeDamage(int damage){
		if (!damaged){
			playerAttributes.AffectHealth(damage);
			playerAnimatorController.TakeDamage();
			this.StartSafeCoroutine(WaitForDamage());
		}
	}

	IEnumerator WaitBeforeFalling(Collision other){
		yield return null;
		onGround = false;
	}

	IEnumerator WaitOnPlayerDeathThenQuitToTitle(){
		yield return this.StartSafeCoroutine(PlayerDeath());
		EndOfRoundMenu.Instance.ShowEndOfRoundMenu(Enums.EndOfRoundStates.Death);
	}

	IEnumerator PlayerDeath(){
		deathParticles.gameObject.SetActive(true);
		playerAnimatorController.PlayerDeath();
		while (deathParticles.IsAlive()){
			yield return null;
		}
	}

	IEnumerator WaitForDamage(){
		damaged = true;
		yield return new WaitForSeconds(damageLength);
		damaged = false;
	}

	void KeepCompilerWarningsAway(){
		Debug.Log(releasedUp +
				  "" + releasedDown +
				  "" + releasedLeft +
				  "" + releasedRight +
				  "" + pressedUp +
				  "" + pressedDown +
				  "" + pressedLeft +
				  "" + pressedRight);
	}
}
