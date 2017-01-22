using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loverBatHandler : MonoBehaviour {

	[SerializeField] private GameObject cinemationPosition;

	private bool cinemationRunning = false;
	private bool flyToTarget = false;
	private Vector2 target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (cinemationRunning) {
			Vector2 currentPosition = transform.position;

			if (flyToTarget) {
//				target = new Vector2(
//					Mathf.Lerp (currentPosition.x, cinemationPosition.transform.position.x, 0.3f * Time.deltaTime),
//					Mathf.Lerp (currentPosition.y, cinemationPosition.transform.position.y, 0.3f * Time.deltaTime));
				target = cinemationPosition.transform.position;
			} else {
				float x = Mathf.Sin( 2.0f * Time.fixedTime * 1.0f ) * ( Mathf.Sin (0.01f * Time.fixedTime * 1.0f) * 250);
				float y = Mathf.Cos( 2.0f * Time.fixedTime * 1.0f ) * ( Mathf.Sin (0.01f * Time.fixedTime * 1.0f) * 250);
				target = new Vector2 (currentPosition.x + x,currentPosition.y + y);// + height / 2);
			}
				
			transform.position = new Vector3 (
				Mathf.Lerp (currentPosition.x, target.x, 0.15f * Time.deltaTime),
				Mathf.Lerp (currentPosition.y, target.y, 0.15f * Time.deltaTime),
				-10);
		}
	}

	public void statEndCinemation () {
		cinemationRunning = true;
		Debug.Log ("running cinemation");
		StartCoroutine (startEndingMovement ());
	}

	IEnumerator startEndingMovement() {
		yield return new WaitForSeconds(10f);
		flyToTarget = true;
	}
}
