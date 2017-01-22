﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wavesHandler : MonoBehaviour {
	
	public static GameObject[] shaderCams;

	// Use this for initialization
	void Start () {
		shaderCams = GameObject.FindGameObjectsWithTag("ShaderCamera");
	}
	// Update is called once per frame
	void Update () {
		Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

		foreach (GameObject camera in shaderCams) {
			MeshRenderer mrenderer = camera.GetComponentInChildren<MeshRenderer> ();
			mrenderer.material.SetVector("SourcePosition", new Vector4( mousePosition.x,mousePosition.y,0.0f,0.0f));
			mrenderer.material.SetInt("FrameCount", Time.frameCount);
		}
	}
}
