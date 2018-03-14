using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
 
public class PlayerController : MonoBehaviour { 
	public Camera playerCamera; 
	[SerializeField] private AudioClip jumpSound; 
	[SerializeField] private AudioSource audioSource; 
	private CharacterController characterController; 
	private Player[] scoreboardData;
	private Vector3 rotationY, verticalMovement, horizontalMovement; 
	private float rotationX = 0;
	private Vector3 characterVelocity; 
	private float characterSpeed;
	private const float GRAVITY = 12f; 
	private const float JUMP_AMOUNT = 4f; 
	private const float SENSITIVITY = 1.2f; 
	private const float ADS_SENSITIVITY = 0.7f;
	private const float SCOPED_SENSITIVITY = 0.4f;
	private float currentSensitivity;
	private const int RUN_SPEED = 3;
	private int isSprinting; 
	private bool atCart;
	private Teams userTeam;
	private Character player;
	private PlayerGUI gui;
	private GameController gameController;
	Texture2D pixel;
	Color pixelColor;

	void Start() { 
		characterController = GetComponent<CharacterController>(); 
		Screen.lockCursor = true; 
		Cursor.visible = false;
		characterSpeed = gameObject.GetComponent<Character>().getSpeed();
		player = gameObject.GetComponent<Character> ();
		gui = gameObject.GetComponentInChildren<PlayerGUI> ();
		gameController = GameObject.FindWithTag("Control").GetComponent<GameController>();
		pixelColor = Color.black;
		pixelColor.a = 0.5f;
		pixel = new Texture2D (1, 1);
		pixel.SetPixel (0, 0, pixelColor);
		pixel.Apply ();
		currentSensitivity = SENSITIVITY;
	}
 
	void Update() { 
		rotationY = new Vector3(0f, Input.GetAxisRaw("Mouse X"), 0f) * SENSITIVITY; 
		rotationX -= Input.GetAxis ("Mouse Y") * SENSITIVITY;
		verticalMovement = Input.GetAxisRaw("Vertical") * transform.forward; 
		horizontalMovement = Input.GetAxisRaw("Horizontal") * transform.right; 
		if (Input.GetKey(KeyCode.LeftShift)) { 
			isSprinting = 1; 
		}	else { isSprinting = 0; }  

		gameObject.transform.Rotate(rotationY); 
		characterVelocity.x = (verticalMovement.x + horizontalMovement.x) * (characterSpeed + isSprinting * RUN_SPEED); 
		characterVelocity.z = (verticalMovement.z + horizontalMovement.z) * (characterSpeed + isSprinting * RUN_SPEED); 

		// Clamp camera angle
 		rotationX = Mathf.Clamp (rotationX, -60.0f, 80.0f);
    	Camera.main.transform.localRotation = Quaternion.Euler (rotationX, 0, 0);
			 
		if (characterController.isGrounded) { 
			characterVelocity.y = 0; 
		} 
 
		if (Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded) { 
				characterVelocity.y = JUMP_AMOUNT; 
				audioSource.PlayOneShot(jumpSound); 
		} 
		else { 
			characterVelocity.y -= GRAVITY * Time.deltaTime; 
		} 
 
		characterController.Move(characterVelocity * Time.deltaTime); 

	}

	public void setSensitivity(int state) {
		switch (state) {
		case 0:
			currentSensitivity = SENSITIVITY;
			break;
		case 1:
			currentSensitivity = ADS_SENSITIVITY;
			break;
		case 2:
			currentSensitivity = SCOPED_SENSITIVITY;
			break;
		}
	}

	private void OnGUI() {
		if (Input.GetKey (KeyCode.Tab)){
			userTeam = gameController.getUserTeam (player.getUserId());
			scoreboardData = userTeam.getTeamStats ();
		    float screenWidth = Screen.width;
		    float screenHeight = Screen.height;
			int playerCount = 0;
			string playerCell;
			GUI.DrawTexture (new Rect (screenWidth/8 - screenWidth/100, screenHeight/4, 3*screenWidth/4 + screenWidth/100, screenHeight/2), pixel);
			foreach (Player statLine in scoreboardData) {
				if (statLine == null) {
					playerCell = "";
				} else {
					playerCell = Global.CHARACTER_NAMES [playerCount] + "\n" + statLine.getUsername () + "\nKills: "	+ statLine.getStatLine ().kills + "\nAssists: " + statLine.getStatLine ().assists + "\nDeaths: " + statLine.getStatLine ().deaths + "\nGold Stolen" + statLine.getStatLine ().goldStolen;
				}
				GUI.Label(new Rect ((screenWidth/8) + (3*screenWidth/20 * playerCount), screenHeight/4, 3*screenWidth/20, screenHeight/2), playerCell);
		        playerCount++;
	      	}
	    }
  	}

	public void setSpeed (float speed) {
		characterSpeed = speed;
	}
} 
