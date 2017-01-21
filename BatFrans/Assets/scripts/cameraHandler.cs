using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraHandler : MonoBehaviour {
	public GameObject player;

	private float verticalOffset;
	private float initialOffset;

	private Vector2 playerPosition;
	private Vector2 target;

	[SerializeField] private bool followPlayer = false;


	// Use this for initialization
	void Start () {
		playerPosition = player.transform.position;
		target = transform.position;
		transform.position = new Vector2(target.x,target.y + verticalOffset);
		target.y -= verticalOffset;

		initialOffset = verticalOffset;
	}
		
	void OnTriggerEnter2D (Collider2D c) {
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
	}

	void OnTriggerExit2D (Collider2D c) {
//		if(c.CompareTag("OneWayPlatform") && touchingPlatforms-- == 1) {
//			jumping = true;
//			//	isBackground = false;
//			srenderer.sortingOrder = 0;
//			//  jumpVel = 0;
//		}
	}

	// Update is called once per frame
	void Update () {
		if (followPlayer) {
			playerPosition = player.transform.position;
			Vector2 currentPosition = target;
			target = new Vector2 (
				Mathf.Lerp (currentPosition.x, playerPosition.x, 3.0f * Time.deltaTime),
				Mathf.Lerp (currentPosition.y, playerPosition.y, 3.0f * Time.deltaTime));

			currentPosition = transform.position;
			currentPosition.y -= verticalOffset;

			//		target = clipCamera( -2.8f,-0.8f,rightHorizontal,1.8f - verticalOffset );

			float currentOffset = verticalOffset;
			verticalOffset = Mathf.Lerp (currentOffset, initialOffset, 1.0f * Time.deltaTime);

			transform.position = new Vector3 (
				Mathf.Lerp (currentPosition.x, target.x, 3.0f * Time.deltaTime),
				Mathf.Lerp (currentPosition.y, target.y, 3.0f * Time.deltaTime) + verticalOffset,
				-10);
		}
	}

//	Vector2 clipCamera ( float lowerX,float lowerY, float upperX, float upperY ) {
//		if (target.x < lowerX) {
//			target.x = lowerX;
//		} else if (target.x > upperX) {
//			target.x = upperX;
//		}
//		if (target.y < lowerY) {
//			target.y = lowerY;
//		} else if (target.y > upperY) {
//			target.y = upperY;
//		}
//
//		return target;
//	}
}