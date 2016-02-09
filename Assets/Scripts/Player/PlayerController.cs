using UnityEngine;
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
		 dashing = false,
		 shielding = false,
		 onGround = false,
		 lastShield = false;

	Vector3 forceVector,
			jumpVector;

	float moveSpeed = 12f,
		  gravity = 15f,
		  jumpSpeed = .75f,
		  jumpDecay = 2.5f;

	Collider lastHit;

	SafeCoroutine jumpingCoroutine;

	void Awake () {
		forceVector = Vector3.zero;
		jumpVector = Vector3.zero;
		instance = this;
	}

	void Update () {
		HandleInput();
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
			forceVector = Vector3.right * moveSpeed * Time.smoothDeltaTime;
		}
		else if (movingLeft){
			forceVector = Vector3.right * moveSpeed * -1 * Time.smoothDeltaTime;
		}
		else{
			forceVector = Vector3.right * (CameraController.CameraSpeed * .7f) * Time.smoothDeltaTime;
		}

		if (!onGround){
			forceVector += Vector3.down * gravity * Time.smoothDeltaTime;
		}

		if (onGround && jumping && (jumpingCoroutine == null || !jumpingCoroutine.IsRunning)){
			jumpingCoroutine = this.StartSafeCoroutine(Jump());
			onGround = false;
		}

		forceVector += jumpVector;
		transform.position = Vector3.MoveTowards(transform.position, transform.position + forceVector, .5f);
	}

	void HandleShields(){
		if (shielding){
			if (!lastShield){
				shield.RaiseShield();
			}
		}
		else{
			shield.LowerShield();
		}
	}

	public void HitGround(){
		onGround = true;
		jumping = false;
		if (jumpingCoroutine != null && jumpingCoroutine.IsRunning){
			jumpingCoroutine.Stop();
			jumpVector = Vector3.zero;
		}
	}

	IEnumerator Jump(){
		jumpVector = Vector3.up * jumpSpeed;

		while (jumpVector.y > 0){
			yield return null;
			jumpVector -= Vector3.up * jumpDecay * Time.smoothDeltaTime;
		}

		jumpVector = Vector3.zero;
	}

	void OnCollisionEnter(Collision other){
		var ground = other.gameObject.GetComponent<Ground>();
		var platform = other.gameObject.GetComponent<Platform>();

		if (lastHit != other.collider){
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
		else{
			Debug.Log(other.gameObject + " FUCK");
		}

		var shot = other.gameObject.GetComponent<Shot>();
		if (shot != null){
			if (!shielding){
				// hit
			}

		}
	}

	void OnCollisionExit(Collision other){
		if (other.collider == lastHit){
			if (other.gameObject.GetComponent<Ground>() != null){
				onGround = false;
				lastHit = null;
			}
			else if (other.gameObject.GetComponent<Platform>() != null){
				onGround = false;
				lastHit = null;
			}
		}
	}
}
