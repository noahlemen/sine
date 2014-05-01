using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour {

	Vector3 lastScale;
	Sine sine;
	AudioSource emitter;

	public AnimationCurve envelope;

	// Use this for initialization
	void Start () {
		lastScale = Vector3.zero;
		sine = GetComponent<Sine>();
		sine.gain = 1f;
		emitter = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () {
		if (lastScale != transform.localScale){
			LimitScale();
			SetColorByScale();
			SetFreqByScale();
			SetMassByScale();
		}
		lastScale = transform.localScale;

	}

	void LimitScale(){
		transform.localScale = new Vector3(
			Mathf.Clamp(transform.localScale.x, .25f, 1.5f),
			Mathf.Clamp(transform.localScale.y, .25f, 1.5f),
			Mathf.Clamp(transform.localScale.z, .25f, 1.5f)
			);
	}

	void SetColorByScale(){
		renderer.material.color = GetColorByScale();
	}

	Color GetColorByScale(){
		return HSBColor.ToColor(HSBColor.Lerp(new HSBColor(.17f,1f,1f), new HSBColor(.83f, 1f, 1f), (transform.localScale.x - .25f)/1.25f));
	}

	void SetFreqByScale(){
		sine.frequency = Mathf.Lerp (1000f, 100f, (transform.localScale.x - .25f)/1.25f);
	}

	void SetMassByScale(){
		rigidbody.mass = Mathf.Lerp(.1f, 10f, (transform.localScale.x - .25f)/1.25f);
	}

	void OnCollisionEnter(Collision c){
		if (c.gameObject.tag != "Player"){
			Debug.Log(rigidbody.velocity.magnitude);
			StartCoroutine(PulseSound());
			StartCoroutine(FlashWhite());
		}
	}

	IEnumerator PulseSound(){
		float t = 0;
		sine.gain = Mathf.Lerp(.5f,1f,rigidbody.velocity.magnitude/14f);
		while (t < 1f){
			emitter.volume = envelope.Evaluate(t);
			t += Time.deltaTime;
			yield return 0;
		}

	}

	IEnumerator FlashWhite(){
		Color c = new Color(1f,1f,1f,0f);
		Color flashcolor = Color.Lerp(c, Color.white, rigidbody.velocity.magnitude/14f);
		float t = 0f;
		transform.GetChild(0).renderer.material.color = flashcolor;
		while(t<1f){
			t += Time.deltaTime;
			transform.GetChild(0).renderer.material.color = Color.Lerp(flashcolor, c, t);
			yield return 0;
		}
	}

}
