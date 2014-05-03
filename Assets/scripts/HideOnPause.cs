using UnityEngine;
using System.Collections;

public class HideOnPause : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (PauseMenu.paused){
			guiText.enabled = false;
		}else{
			guiText.enabled = true;
		}
	}
}
