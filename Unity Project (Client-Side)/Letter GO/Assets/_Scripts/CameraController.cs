using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class CameraController : MonoBehaviour {

	//External References.
	private sharedVariables sharedVariables;
	private TimerController timerController;
	private ImageController imageController;
	public GameObject mainCamera;
	public GameObject ARCamera;

	// Button: Take Picture. 
	private GameObject takePictureButton;

	// UI Elements. 
	private GameObject cross;
	private GameObject generalInterface;
	private GameObject imageInterface;
	private Image image;
	private Text userName;
	private Text userScore;

	// Variables to Control Events.
	private bool ARCameraActive;
	private bool changeInterface;

	// Variable to store the captured picture temporally. 
	// TODO: Maybe this will have a future use. 
	private byte[] pictureCaptured;

	// Functions executed on the intilization of the application. 
	void Start () {

		// Assignations.
		timerController = GameObject.Find ("TimerController").GetComponent<TimerController> ();
		imageController = GameObject.Find ("ImageController").GetComponent<ImageController> ();
		sharedVariables = GameObject.Find ("sharedVariables").GetComponent<sharedVariables> (); 

		imageInterface = GameObject.Find ("ImageInterface");
		generalInterface = GameObject.Find ("GeneralInterface");

		takePictureButton = GameObject.Find ("GI.TakePicture");
		image = GameObject.Find ("II.Image").GetComponent<Image>();
		cross = GameObject.Find ("GI.EnableCamera.Cross");
		userName = GameObject.Find ("GI.UserName").GetComponent<Text>();
		userScore = GameObject.Find ("GI.UserScore").GetComponent<Text>();

		// Default Values.
		ARCameraActive = false;
		changeInterface = false;
		ARCamera.SetActive (false);
		cross.SetActive (false);
		takePictureButton.SetActive (false);
		imageInterface.SetActive (false);

		userName.text = "Welcome " + sharedVariables.getUsername ();
		userScore.text = "Score: " + sharedVariables.getScore().ToString();
	}

	// Update is called once per frame.
	void Update () {

		userScore.text = "Score: " + sharedVariables.getScore().ToString();

		// Event 1: When we want to change to the Image Interface. 
		if (changeInterface) {
		
			// We create the texture for the Image using the bit-stream of the captured picture. 
			Texture2D texture = new Texture2D (Screen.width, Screen.height);
			texture.LoadImage (pictureCaptured);
			texture.Apply ();
			image.material.mainTexture = texture;

			// Changing Interfaces. 
			imageInterface.SetActive (true);
			generalInterface.SetActive (false);

			// Interruptin the timer because we already captured the Letter. 
			timerController.interruptTimer (false);

			// Changing from AR Camera to Main Camera. 
			mainCamera.SetActive (true);
			ARCamera.SetActive (false);

			// Finishing Event 1. 
			changeInterface = false;
			imageController.managingImage = true;
		}
	}

	// Switches the Cameras and controls the visibility of the Take Picture Button. 
	// Called when the user clicks on the button "(EnableDisableCamera)". 
	public void EnableDisableCamera() {
		if (ARCameraActive) {
			mainCamera.SetActive (true);
			ARCamera.SetActive (false);
			cross.SetActive (false);
			takePictureButton.SetActive (false);
		} else {
			ARCamera.SetActive (true);
			mainCamera.SetActive (false);
			cross.SetActive (true);
			takePictureButton.SetActive (true);
		}

		ARCameraActive = !ARCameraActive;
	}

	// Assigns the value of the Captured picture.
	// Called when the user takes the photo (See WebCam Behaviour). 
	public void PhotoTaked(byte[] picture) {

		pictureCaptured = picture;

		// Launch Event 1. 
		changeInterface = true;

	}
}
