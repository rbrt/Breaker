using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;

public class PlayerController : MonoBehaviour {

	static PlayerController instance;

	[SerializeField] protected Shield shield;

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

			this.StartSafeCoroutine(WaitThenQuitToTitle());	
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

	IEnumerator WaitThenQuitToTitle(){
		yield return new WaitForSeconds(2);
		SceneManager.LoadScene("Title", LoadSceneMode.Single);
	}
}
