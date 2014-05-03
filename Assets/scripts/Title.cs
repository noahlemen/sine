using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour {

	public MouseLook[] mouseScripts;
	public FirstPersonDrifter drifter;
	public GUIText reticule;

	public GameObject firstTut;

	private bool DONE = false;

	// Use this for initialization
	void Start () {
		reticule.enabled = false;
		foreach (MouseLook l in mouseScripts){
			l.enabled = false;
		}
		drifter.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (DONE) return;

		if (Time.timeSinceLevelLoad < 2f){
			guiText.enabled = true;


		}else{
			if (Input.anyKey || Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f)
				StartCoroutine(fadeText());

		}

		if (PauseMenu.paused)
			guiText.enabled = false;
		else
			guiText.enabled = true;

	}

	IEnumerator fadeText(){
		foreach (MouseLook l in mouseScripts){
			l.enabled = true;
		}
		drifter.enabled = true;
		float t = 1f;
		while (t > 0f){
			guiText.color = new Color(guiText.color.r, guiText.color.g, guiText.color.b, t);
			t -= Time.deltaTime*.65f;
			yield return 0;
		}
		guiText.enabled = false;
		DONE = true;
		yield return new WaitForSeconds(.5f);
		Instantiate(firstTut);
		reticule.enabled = true;
		Destroy(gameObject);
	}
}
