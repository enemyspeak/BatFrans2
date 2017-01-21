using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wavesHandler : MonoBehaviour {
	public Shader waveEffect;
	public static GameObject[] shaderCams;

	// Use this for initialization
	void Start () {
//		shaderCams = GameObject.FindGameObjectsWithTag("ShaderCamera");
	}
	public void Awake() {
		if (waveEffect)
			transform.GetComponent<Camera>().SetReplacementShader(waveEffect,null);
	}
	// Update is called once per frame
	void Update () {
//		foreach (GameObject camera in shaderCams) {
//			camera.RenderWithShader(waveEffect);
//		}
	}
}
