using UnityEngine;
using System.Collections;

public class MovePlayer : MonoBehaviour {

	Vector3 moveVector;
	float rY;
	CharacterController controller;

	public float movespeed = 10f;
	public float sensitivity = 10f;
	public float minY = -70f;
	public float maxY = 90f;
	public float gravity = -10f;

	// Use this for initialization
	void Awake () {
		controller = GetComponent<CharacterController>();
		Screen.lockCursor = true;
	}
	
	// Update is called once per frame
	void Update () {
		moveVector = Vector3.zero;

		if (Input.GetMouseButtonDown(0)) Screen.lockCursor = true;

		moveVector += Input.GetAxis("Vertical") * transform.forward + Input.GetAxis("Horizontal") * transform.right;

		transform.Rotate( 0f, Input.GetAxis( "Mouse X" ) * Time.deltaTime * sensitivity, 0f );

		rY += Input.GetAxis( "Mouse Y" ) * Time.deltaTime * sensitivity * -1f;
		rY = Mathf.Clamp( rY, minY, maxY );
		Camera.main.transform.localRotation = Quaternion.Euler( rY, Camera.main.transform.localEulerAngles.y, Camera.main.transform.localEulerAngles.z );

		moveVector += Vector3.up * gravity;
		controller.Move(moveVector.normalized * movespeed * Time.deltaTime);

	}

	void OnControllerColliderHit(ControllerColliderHit hit){
		Rigidbody r = hit.rigidbody;

		if (r == null || r.isKinematic) { return; }
		if (hit.moveDirection.y < -0.3) { return; }

		Vector3 push = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

		r.velocity = push * 2f;
	}
	
}
