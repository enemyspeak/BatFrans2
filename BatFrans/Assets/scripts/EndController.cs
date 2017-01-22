using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndController : MonoBehaviour {

	private GameObject uiObject;

	void Start () {
		uiObject = GameObject.FindGameObjectWithTag("UI");
		uiObject.GetComponent<PlayMusic>().PlaySelectedMusic(0);
	}

	void Update () {
		if(Input.GetKeyUp(KeyCode.Space)){
			SceneManager.LoadScene(0);
			Destroy(uiObject);
		}
	}
}
