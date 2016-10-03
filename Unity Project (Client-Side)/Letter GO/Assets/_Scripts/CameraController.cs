using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class CameraController : MonoBehaviour {

	//External References.
	private TimerController timerController;
	public GameObject mainCamera;
	public GameObject ARCamera;

	// Button: Take Picture. 
	private GameObject takeScreenShotButton;

	// UI Elements. 
	private GameObject cross;
	private GameObject generalInterface;
	private GameObject imageInterface;
	public Image image;

	// Variables to Control Events.
	private bool ARCameraActive;
	private bool changeInterface;

	// Variable to store the captured picture temporally. 
	// TODO: Maybe this will have a future use. 
	private byte[] pictureCaptured;

	// Functions executed on the intilization of the application. 
	void Start () {

		// Assignations.
		timerController = GameObject.FindGameObjectWithTag ("TimerController").GetComponent<TimerController>();
		takeScreenShotButton = GameObject.FindGameObjectWithTag ("TakeScreenShot");
		cross = GameObject.FindGameObjectWithTag ("Cross");
		imageInterface = GameObject.FindGameObjectWithTag ("ImageInterface");
		generalInterface = GameObject.FindGameObjectWithTag ("GeneralInterface");

		// Default Values.
		ARCameraActive = false;
		changeInterface = false;
		ARCamera.SetActive (false);
		cross.SetActive (false);
		takeScreenShotButton.SetActive (false);
		imageInterface.SetActive (false);
	}

	// Update is called once per frame.
	void Update () {

		// Event 1: When we want to change to the Image Interface. 
		if (changeInterface) {
		
			// We create the texture for the Image using the bit-stream of the captured picture. 
			Texture2D texture = new Texture2D (Screen.width, Screen.height);
			texture.LoadImage (pictureCaptured);
			image.material.mainTexture = texture;

			// Changing Interfaces. 
			imageInterface.SetActive (true);
			generalInterface.SetActive (false);

			// Interruptin the timer because we already captured the Letter. 
			timerController.interruptTimer ();

			// Changing from AR Camera to Main Camera. 
			mainCamera.SetActive (true);
			ARCamera.SetActive (false);

			// Finishing Event 1. 
			changeInterface = false;
		}
	}

	// Switches the Cameras and controls the visibility of the Take Picture Button. 
	// Called when the user clicks on the button "(EnableDisableCamera)". 
	public void EnableDisableCamera() {
		if (ARCameraActive) {
			mainCamera.SetActive (true);
			ARCamera.SetActive (false);
			cross.SetActive (false);
			takeScreenShotButton.SetActive (false);
		} else {
			ARCamera.SetActive (true);
			mainCamera.SetActive (false);
			cross.SetActive (true);
			takeScreenShotButton.SetActive (true);
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
