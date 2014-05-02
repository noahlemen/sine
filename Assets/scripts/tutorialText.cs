using UnityEngine;
using System.Collections;

public class tutorialText : MonoBehaviour {

	public string text;
	public KeyCode endtrigger;
	public string axis;
	public GameObject nextTut;

	// Use this for initialization
	void Start () {
		guiText.text = text;
	}
	
	// Update is called once per frame
	void Update () {
		if (PauseMenu.paused){
			guiText.enabled = false;
			return;
		}else{
			guiText.enabled = true;
		}
		if (Input.GetKeyDown(endtrigger)){
			StartCoroutine(fadeText());
		}
		if (axis != ""){
			if (Input.GetAxis(axis) != 0f){
				StartCoroutine(fadeText());
			}
			if (Input.GetAxis("Alt Scroll") != 0f){
				StartCoroutine(fadeText());
			}
		}

	}

	IEnumerator fadeText(){
		float t = 1f;
		while (t > 0f){
			guiText.color = new Color(guiText.color.r, guiText.color.g, guiText.color.b, t);
			t -= Time.deltaTime;
			yield return 0;
		}
		guiText.enabled = false;
		yield return new WaitForSeconds(.25f);
		if (nextTut != null) Instantiate(nextTut);
		Destroy(gameObject);
	}
}
