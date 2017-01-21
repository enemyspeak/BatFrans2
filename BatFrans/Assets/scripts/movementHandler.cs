using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementHandler : MonoBehaviour {
	private bool flipped = false;

	[SerializeField] private float normal_acceleration = 0.025f;
	[SerializeField] private float drag_active = 0.9f;
	[SerializeField] private float drag_passive = 0.96f;
	[SerializeField] private int max_speed = 1;
	[SerializeField] private float deadZone = 0.005f;
	[SerializeField] private Vector2 velocity = new Vector2(0,0);

	private int max_speed_sq;

	private Animator anim;
	private SpriteRenderer srenderer;

	void Start () {
		srenderer = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
		anim.Play("idleStand");
	}

	void OnTriggerEnter2D (Collider2D c) {
		if(c.CompareTag("Room")) {
			
		}
	}

	void OnTriggerExit2D (Collider2D c) {
		//		if(c.CompareTag("OneWayPlatform") && touchingPlatforms-- == 1) {
		//			jumping = true;
		//			//	isBackground = false;
		//			srenderer.sortingOrder = 0;
		//			//  jumpVel = 0;
		//		}
	}

//	void OnTriggerEnter2D (Collider2D c) {
//		if(c.CompareTag("OneWayPlatform")) {
//			touchingPlatforms++;
//			//	var maxDistance = (c.transform.localScale.y + transform.localScale.y) / 2;
//			if(velocity.y < 0 && transform.position.y - c.transform.position.y * Time.fixedDeltaTime > deadZone) {
//				jumping = false;
//				//	isBackground = true;
//				srenderer.sortingOrder = -2;
//				jumpVel = 0;
//				velocity = new Vector2(velocity.x,0);
//				Vector2 pos = transform.position;
//				pos.y = c.transform.position.y + 0.344f + deadZone;
//				transform.position = pos;
//			}
//		}
//	}

//	void OnTriggerExit2D (Collider2D c) {
//		if(c.CompareTag("OneWayPlatform") && touchingPlatforms-- == 1) {
//			jumping = true;
//			//	isBackground = false;
//			srenderer.sortingOrder = 0;
//			//  jumpVel = 0;
//		}
//	}

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

	// Update is called once per frame
	void FixedUpdate () {
		max_speed_sq = max_speed ^ 2;

		bool x_has_input = false;
		bool y_has_input = false;

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

		Vector2 currentPosition = transform.position;
		transform.position = new Vector2(currentPosition.x + velocity.x, currentPosition.y + velocity.y);

//		previousVelocity = velocity;

		doAnimation();
	}

	void flip() {
		flipped = !flipped;
		Vector3 s = transform.localScale;
		s.x *= -1;
		transform.localScale = s;

		anim.Play("turnsmear");
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
}
