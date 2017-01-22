using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class movementHandler : MonoBehaviour {
	private bool flipped = false;

	[SerializeField] private float normal_acceleration = 0.5f;
	[SerializeField] private float drag_active = 0.9f;
	[SerializeField] private float drag_passive = 0.97f;
	[SerializeField] private int max_speed = 50;
//	[SerializeField] private float deadZone = 0.005f;
	[SerializeField] private Vector2 velocity = new Vector2(0,0);
	[SerializeField] private Animator gameOverWipe;
	[SerializeField] private PlayerUI ui;
	[SerializeField] GameObject loverBat;
	[SerializeField] ParticleSystem heartParticles;
	[SerializeField] ParticleSystem starParticles;

	private int max_speed_sq;
	private Vector2 roomPosition;
	private Vector2 lovePosition; 

	private bool tweening_rooms = false;
	private bool hit_stun = false;
	private bool ending_cinemation = false; 

	public int hearts = 3;

	private Animator anim;
	private SpriteRenderer srenderer;
	private AudioSource audio;

	[Header("AudioClips")]
	[SerializeField]
	private AudioClip hurt;
	[SerializeField]
	private AudioClip eat;
	[SerializeField]
	private AudioClip scream;
	[SerializeField]
	private AudioClip woohoo;

	void Start () {
		srenderer = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
		audio = GetComponent<AudioSource>();
		StartCoroutine(PlayHeartParticles());
		starParticles.Stop();
	}

	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.CompareTag ("Room")) {
			Vector2 currentPosition = transform.position;
			roomPosition = collider.gameObject.transform.position;
//			Debug.Log (roomPosition);
			Camera.main.gameObject.GetComponent<cameraHandler> ().panToWorldPosition (roomPosition);
//			Camera.main.GetComponent<CameraHandler>().panToWorldPosition (roomPosition);;
		
			velocity = new Vector2 (velocity.x * 0.5f, velocity.y * 0.5f); // reduce speed by half when changing rooms
		
//			float pushAngle = Mathf.Atan2 (currentPosition.y - roomPosition.y, currentPosition.x - roomPosition.x);
//
//			float d = (currentPosition.y - roomPosition.y) ^ 2 + (currentPosition.x - roomPosition.x) ^ 2;
//			d *= 0.1f;
			if ((currentPosition.x < roomPosition.x + Screen.width / 100 / 2) && (currentPosition.x > roomPosition.x - Screen.width / 100 / 2)) {
				roomPosition.x = currentPosition.x;
			} 
			if ((currentPosition.y < roomPosition.y + Screen.height / 100 / 2) && (currentPosition.y > roomPosition.y - Screen.height / 100 / 2)) {
				roomPosition.y = currentPosition.y;
			}

			tweening_rooms = true;
			StartCoroutine (disableControlsLockout ());
		} else if (collider.CompareTag ("Hurt")) {
			if (hit_stun) { // if already hit, don't do this again.
				return;
			}
			hit_stun = true;

			hearts--;

			if(hearts <= 0){
				GameOver();
			}

			ui.UpdateHeartCount(hearts);

			anim.SetTrigger("Hurt");
			audio.PlayOneShot(hurt);
			StartCoroutine(PlayStarParticles());

			velocity *= -0.5f;
			StartCoroutine (flashSprite ());
			StartCoroutine (disableControlsLockout ());
		} else if (collider.CompareTag ("Health")) {
			Destroy(collider.gameObject);
			hearts = hearts + collider.GetComponent<collectible>().healAmount;
			ui.UpdateHeartCount(hearts);
			audio.PlayOneShot(eat);
			StartCoroutine(PlayHeartParticles());
		} else if (collider.CompareTag ("End")) {
			Vector2 currentPosition = transform.position;
			Camera.main.gameObject.GetComponent<cameraHandler> ().panToWorldPosition (loverBat.transform.position);
			StartCoroutine (startEndingMovement ());
			StartCoroutine (goToCreditsYo ());
			ending_cinemation = true;
			audio.PlayOneShot(woohoo);
			GameObject uiObject = GameObject.FindGameObjectWithTag("UI");
			uiObject.GetComponent<PlayMusic>().PlayEndCinematicMusic();
		}
	}

	IEnumerator PlayStarParticles() {
		starParticles.Play();
		yield return new WaitForSeconds(1f);
		starParticles.Stop();
	}

	IEnumerator PlayHeartParticles() {
		heartParticles.Play();
		yield return new WaitForSeconds(2f);
		heartParticles.Stop();
	}

	IEnumerator goToCreditsYo() {
		yield return new WaitForSeconds(18);
//		loverBat.GetComponent<loverBatHandler> ().statEndCinemation ();
		SceneManager.LoadScene("Win");
	}

	IEnumerator startEndingMovement() {
		yield return new WaitForSeconds(0.8f);
		loverBat.GetComponent<loverBatHandler> ().statEndCinemation ();
	}

	IEnumerator flashSprite() {
		for(var n = 0; n < 3; n++)
		{
			srenderer.enabled = true;
			yield return new WaitForSeconds(0.05f);
			srenderer.enabled = false;
			yield return new WaitForSeconds(0.05f);
		}
		srenderer.enabled = true;
	}

	IEnumerator disableControlsLockout() {
		yield return new WaitForSeconds(1);
		tweening_rooms = false;
		hit_stun = false;
	}

	void OnTriggerExit2D (Collider2D c) {
		//		if(c.CompareTag("OneWayPlatform") && touchingPlatforms-- == 1) {
		//			jumping = true;
		//			//	isBackground = false;
		//			srenderer.sortingOrder = 0;
		//			//  jumpVel = 0;
		//		}
	}

	bool op_xor(bool a,bool b) {
		return ((a || b) && (!(a && b)));
	}

	private float magnitude_2d(float x,float y) {
		return Mathf.Sqrt(x*x + y*y);
	}

	private float magnitude_2d_sq (float x,float y) {
		return x*x + y*y;
	}

	private Vector2 normalize_2d (float x,float y) {
		float mag = x*x + y*y;
		if (mag == 0) {
			return new Vector2(0,0);
		} else {
			return new Vector2(x/mag, y/mag);
		}
	}

	void doEcholocate () {
		// send shader start pos and angle
		anim.SetTrigger("Scream");
		audio.PlayOneShot(scream);
	}

	// Update is called once per frame
	void Update () {
		Vector2 currentPosition = transform.position;
		if (hit_stun) {
			transform.position = new Vector2(currentPosition.x + velocity.x * Time.deltaTime, currentPosition.y + velocity.y * Time.deltaTime);

		} else if (tweening_rooms) {
			transform.position = new Vector2 (//Vector2.Lerp (transform.position, roomPosition, 1 );
				Mathf.Lerp (currentPosition.x, roomPosition.x, 0.3f * Time.deltaTime),
				Mathf.Lerp (currentPosition.y, roomPosition.y, 0.3f * Time.deltaTime)
			);				
		} else if (ending_cinemation) {
			lovePosition = loverBat.transform.position;
			StartCoroutine(PlayHeartParticles());
			transform.position = new Vector2 (//Vector2.Lerp (transform.position, roomPosition, 1 );
				Mathf.Lerp (currentPosition.x, lovePosition.x, 1.75f * Time.deltaTime),
				Mathf.Lerp (currentPosition.y, lovePosition.y, 1.75f * Time.deltaTime)
			);				
		} else {

			max_speed_sq = max_speed ^ 2;

			bool x_has_input = false;
			bool y_has_input = false;

			//if (Input.GetAxis("Jump") == 1) {
			//	doEcholocate();
			//}

			if (Input.GetKeyUp(KeyCode.Space)) {
				doEcholocate();
			}

			if (Input.GetAxis("Horizontal") != 0) 
			{
				x_has_input = true;
			}

			if (Input.GetAxis("Vertical") != 0) 
			{
				y_has_input = true;
			}

			float temp_x_accel = Input.GetAxis("Horizontal");
			float temp_y_accel = Input.GetAxis("Vertical");

			Vector2 temp_norm_accel = new Vector2(temp_x_accel,temp_y_accel);

			temp_x_accel = temp_norm_accel.x * normal_acceleration;
			temp_y_accel = temp_norm_accel.y * normal_acceleration;

			float temp_x_vel = velocity.x;
			float temp_y_vel = velocity.y;

			float cur_speed = magnitude_2d (velocity.x, velocity.y);

			if (normal_acceleration + cur_speed > max_speed) {

				float accel_magnitude = max_speed - cur_speed;
				if (accel_magnitude < 0) { 
					accel_magnitude = 0;
				}

				temp_x_accel = temp_norm_accel.x * accel_magnitude;
				temp_y_accel = temp_norm_accel.y * accel_magnitude;
			}

			temp_x_vel += temp_x_accel;
			temp_y_vel += temp_y_accel;

			float temp_vel = magnitude_2d_sq (temp_x_vel, temp_y_vel);
			if (Mathf.Abs (temp_vel) > max_speed_sq) {
				temp_x_vel = velocity.x;
				temp_y_vel = velocity.y;
			}


			float temp_drag = drag_passive;
			if (y_has_input || x_has_input) {
				temp_drag = drag_active;
			}
				
			velocity = new Vector2 (temp_x_vel * temp_drag, temp_y_vel * temp_drag);

			transform.position = new Vector2(currentPosition.x + velocity.x * Time.deltaTime, currentPosition.y + velocity.y * Time.deltaTime);
		}
		//doSineWave();
		doAnimation();
	}

	void flip() {
		flipped = !flipped;
		Vector3 s = transform.localScale;
		s.x *= -1;
		transform.localScale = s;

		anim.Play("turnsmear");
	}

	void doSineWave() {
		Vector2 currentPosition = transform.position;
		currentPosition.y += Mathf.Sin (Time.fixedTime*8f)/150;
		transform.position = currentPosition;
	}

	void doAnimation() {
		
//		if (barking) {
//			anim.Play("bark");
//			idleCounter = 0;
//		} else {
//			if (!anim.GetCurrentAnimatorStateInfo(0).IsName("bark")) {
//				if (jumping) {
//					anim.Play("jump");
//					idleCounter = 0;
//				} else if (walking) {
//					if (!anim.GetCurrentAnimatorStateInfo(0).IsName("turnsmear")) {
//						anim.Play("walk");	
//						idleCounter = 0;
//					}
//				} else {
//					idleCounter += Time.deltaTime;
//					if (idleCounter > idleTime) {
//						anim.Play("idleSit");
//					} else {
//						anim.Play("idleStand");	
//					}
//				}
//			}
//		}
	}

	Vector2 clipPlayer( float lowerX,float lowerY, float upperX, float upperY  ) {
		Vector2 target = transform.position;
		float indent = 0.2f;

		if (target.x < lowerX) {
			target.x = lowerX + indent;
			velocity = new Vector2(1,0);
		} else if (target.x > upperX) {
			target.x = upperX - indent;
			velocity = new Vector2(-1,0);
		}
		if (target.y < lowerY) {
			target.y = lowerY;
		} else if (target.y > upperY) {
			target.y = upperY;
		}

		return target;
	}

	void mouseControl() {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0;
		transform.position = mousePos;
	}

	private void GameOver(){
		anim.SetTrigger("Death");
		srenderer.sortingLayerName = "Foreground";
		gameOverWipe.SetTrigger("Wipe");
		StartCoroutine(ResetScene());
	}

	private IEnumerator ResetScene(){
		yield return new WaitForSeconds(3.0f);
		SceneManager.LoadScene("Cave");
	}
}
