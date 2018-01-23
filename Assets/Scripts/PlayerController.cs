using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	float sensitivity = 1.7f; 
	public Camera playerCamera;
	CharacterController characterController;
	Vector3 rotationX, rotationY, verticalMovement, horizontalMovement;
	Vector3 characterVelocity;
	const float GRAVITY = 9.81f;
	const float CHARACTER_SPEED = 3.14f;
	const float JUMP_AMOUNT = 4f;
	void Start () {
		characterController = GetComponent<CharacterController> ();
	}

	void Update () {
		rotationY = new Vector3 (0f, Input.GetAxisRaw ("Mouse X"), 0f) * sensitivity;
		rotationX = new Vector3 (Input.GetAxisRaw ("Mouse Y"), 0f, 0f) * -sensitivity;
		verticalMovement = Input.GetAxisRaw ("Vertical") * transform.forward;
		horizontalMovement = Input.GetAxisRaw ("Horizontal") * transform.right;


		playerCamera.transform.Rotate (rotationX);
		gameObject.transform.Rotate (rotationY);

		characterVelocity.x = (verticalMovement.x + horizontalMovement.x) * CHARACTER_SPEED;
		characterVelocity.z = (verticalMovement.z + horizontalMovement.z) * CHARACTER_SPEED;


		if (Input.GetKeyDown (KeyCode.Space)) {
			characterVelocity.y = JUMP_AMOUNT;
		} else {
			characterVelocity.y -= GRAVITY * Time.deltaTime;
		}

		characterController.Move (characterVelocity * Time.deltaTime); 
	}
	void FixedUpdate () {

	}
}
