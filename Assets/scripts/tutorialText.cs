using UnityEngine;
using System.Collections;

public class tutorialText : MonoBehaviour {

	public string text;
	public KeyCode endtrigger;
	public string axis;
	public GameObject nextTut;

	private bool ending = false;

	// Use this for initialization
	void Start () {
		guiText.text = text;
		StartCoroutine(fadeInText());
	}
	
	// Update is called once per frame
	void Update () {
		if (PauseMenu.paused){
			guiText.enabled = false;
			return;
		}else{
			guiText.enabled = true;
		}
		if (Input.GetKeyDown(endtrigger) && !ending){
			StartCoroutine(fadeText());
		}
		if (axis != ""){
			if (Input.GetAxis(axis) != 0f && !ending){
				StartCoroutine(fadeText());
			}
			if (Input.GetAxis("Alt Scroll") != 0f && !ending){
				StartCoroutine(fadeText());
			}
		}

	}

	IEnumerator fadeInText(){
		float t = 0f;
		while (t < 1f){
			guiText.color = new Color(guiText.color.r, guiText.color.g, guiText.color.b, t);
			t += Time.deltaTime * 3f;
			yield return 0;
		}
	}

	IEnumerator fadeText(){
		ending = true;
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
