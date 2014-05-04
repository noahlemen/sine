using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour {

	Vector3 lastScale;
	Sine sine;
	AudioSource emitter;
	bool isQuitting;
	float targetVolume;

	float tremPhase;

	public AudioClip makecube, removecube;

	public AnimationCurve envelope;
	public GameObject destroyparticles;

	public Color mincolor, maxcolor;

	// Use this for initialization
	void Start () {
		Camera.main.audio.PlayOneShot(makecube, .25f);
		lastScale = Vector3.zero;
		sine = GetComponent<Sine>();
		sine.gain = 1f;
		emitter = GetComponent<AudioSource>();
		tremPhase = Random.Range(0f,1f);
		targetVolume = 0f;
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

		emitter.volume = Mathf.Lerp(emitter.volume, targetVolume, .1f);
		
		Color flashcolor = Color.Lerp(new Color(1f,1f,1f,0f), Color.white, emitter.volume);
		transform.GetChild(0).renderer.material.color = flashcolor;

		if (targetVolume == .1f){
			emitter.volume += Mathf.Sin((Time.time+tremPhase)*10f)*.01f;
		}

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
		//return HSBColor.ToColor(HSBColor.Lerp(new HSBColor(.17f,1f,1f), new HSBColor(.83f, 1f, 1f), (transform.localScale.x - .25f)/1.25f));
		return HSBColor.ToColor(HSBColor.Lerp(HSBColor.FromColor(mincolor), HSBColor.FromColor(maxcolor), (transform.localScale.x - .25f)/1.25f));

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
			emitter.volume = Mathf.Lerp(.5f,1f,rigidbody.velocity.magnitude/14f);
			targetVolume = .1f;
		}
	}

	void OnTriggerEnter(Collider c){
		if (c.gameObject.tag == "Player"){
			emitter.volume = 1f;
		}

	}

	void OnCollisionStay(Collision c){
		if (c.gameObject.tag == "Level"){
			targetVolume = .1f;
		}
	}

	void OnCollisionExit(Collision c){
		if (c.gameObject.tag != "Player"){
			targetVolume = 0f;
		}
	}

	void OnApplicationQuit(){
		isQuitting = true;
	}

	void OnDestroy(){
		if (isQuitting || PauseMenu.resetting) return;
		Camera.main.audio.PlayOneShot(removecube, .25f);
		GameObject particles = Instantiate(destroyparticles, transform.position, destroyparticles.transform.rotation) as GameObject;
		particles.transform.localScale = transform.localScale;
		particles.particleSystem.startSize = transform.localScale.x/5f;
	}

}
