using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wavesHandler2 : MonoBehaviour {

	Renderer renderer;

	// Use this for initialization
	void Start () {
		renderer = this.GetComponent<Renderer> ();
	}
	// Update is called once per frame
	void Update () {
		Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		renderer.material.SetInt("_FrameCount", (int)Time.frameCount);
		renderer.material.SetVector("_SourcePosition", new Vector4( mousePosition.x,mousePosition.y,0.0f,0.0f));//SetVector("Offset", new Vector4(-10, 50, 30, 40));
	}
}
