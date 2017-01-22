using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

	[SerializeField]
	private movementHandler player;
	[SerializeField]
	Image[] heartImages;

	void Start () {
		TurnOffAllhearts();
		StartCoroutine(InitializeHearts());
	}

	private IEnumerator InitializeHearts(){
		yield return new WaitForSeconds(0.6f);
		UpdateHeartCount(player.hearts);
	}

	public void UpdateHeartCount(int amount){
		TurnOffAllhearts();

		for (int h = 0; h < amount; h++){
			heartImages[h].enabled = true;
		}
	}

	private void TurnOffAllhearts(){
		for (int i = 0; i < heartImages.Length; i++) {
			heartImages[i].enabled = false;
		}
	}
}
