using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System;

public class CameraController : MonoBehaviour {

	//External References.
	private sharedVariables sharedVariables;
	private soundPlayer soundPlayer;
	private TimerController timerController;
	private ResultController resultController;
	private APIController apiController;
	private TipController tipController;
	public GameObject mainCamera;
	public GameObject ARCamera;

	// Button: Take Picture. 
	private GameObject takePictureButton;

	// UI Elements. 
	private GameObject PowerOn;
	private GameObject PowerOff;
	private GameObject resultInterface;
	private GameObject generalInterface;
	private GameObject selector;
	private Image image;
	private Text userName;
	private Text userScore;

	// Variables to Control Events.
	private bool ARCameraActive;
	private bool changeInterface;

	private AndroidJavaObject classifier;

	// Functions executed on the intilization of the application. 
	void Start () {

		// Assignations.
		sharedVariables = GameObject.Find ("sharedVariables").GetComponent<sharedVariables> (); 
		soundPlayer = GameObject.Find ("soundPlayer").GetComponent<soundPlayer> ();
		timerController = GameObject.Find ("TimerController").GetComponent<TimerController> ();
		resultController = GameObject.Find ("ResultController").GetComponent<ResultController> ();
		apiController = GameObject.Find ("APIController").GetComponent<APIController> ();
		tipController = GameObject.Find ("TipController").GetComponent<TipController> ();

		generalInterface = GameObject.Find ("GeneralInterface");
		resultInterface = GameObject.Find ("ResultInterface");

		takePictureButton = GameObject.Find ("GI.TakePicture");
		image = GameObject.Find ("RI.Image").GetComponent<Image>();
		PowerOn = GameObject.Find ("GI.PowerOn");
		PowerOff = GameObject.Find ("GI.PowerOff");
		selector = GameObject.Find ("GI.Selector");
		userName = GameObject.Find ("GI.UserName").GetComponent<Text>();
		userScore = GameObject.Find ("GI.UserScore").GetComponent<Text>();

		// Default Values.
		ARCameraActive = false;
		changeInterface = false;
		ARCamera.SetActive (false);
		PowerOn.SetActive (true);
		PowerOff.SetActive (false);
		selector.SetActive (false);
		takePictureButton.SetActive (false);

		//JAVA CLASSIFIER
		classifier = new AndroidJavaObject ("wrappers.Classifier");
		classifier.Call ("loadDefaultModel");

		userName.text = "Welcome " + sharedVariables.getUsername ();
		userScore.text = "Score: " + sharedVariables.getScore().ToString();

		resultInterface.SetActive (false);
	}

	// Update is called once per frame.
	void Update () {

		userScore.text = "Score: " + sharedVariables.getScore().ToString();

		// Event 1: When we want to change to the Image Interface. 
		if (changeInterface) {

			// Changing Interfaces. 
			resultInterface.SetActive (true);
			generalInterface.SetActive (false);

			// Interruptin the timer because we already captured the Letter. 
			timerController.interruptTimer (true);

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
		soundPlayer.playSound ("select");
		if (ARCameraActive) {
			mainCamera.SetActive (true);
			ARCamera.SetActive (false);
			PowerOn.SetActive (true);
			PowerOff.SetActive (false);
			selector.SetActive (false);
			takePictureButton.SetActive (false);
		} else {
			ARCamera.SetActive (true);
			mainCamera.SetActive (false);
			PowerOn.SetActive (false);
			PowerOff.SetActive (true);
			selector.SetActive (true);
			takePictureButton.SetActive (true);
		}
		tipController.showTip (ARCameraActive);	
		ARCameraActive = !ARCameraActive;
	}

	// Assigns the value of the Captured picture.
	// Called when the user takes the photo (See WebCam Behaviour). 
	public void PhotoTaked(byte[] picture, int w, int h) {

		// Gets the letter. 
		string letter = timerController.getLetter ();

		string recognized = recognizeLetter(letter, picture, w, h);

		// We convert the image to Base64, so we can send it through a JSON.
		string imageb64 = Convert.ToBase64String (picture);

		// We call the API method to send the results. 
		double score = apiController.sendResults (letter, imageb64, recognized);

		// When we have the results, we set them. 
		resultController.setTextandScore (letter, score, recognized);

		// We create a new texture to load our image in the Result Interface. 
		Texture2D texture = new Texture2D (w, h);
		texture.LoadImage (picture);
		texture.Apply ();

		image.material.mainTexture = texture;

		// Launch Event 1. 
		changeInterface = true;
	}

	public void openRanking() {
		soundPlayer.playSound ("select");
		SceneManager.LoadScene (2);
	}

	public void openAlbum() {
		soundPlayer.playSound ("select");
		SceneManager.LoadScene (3);
	}

	public void openConfiguration() {
		soundPlayer.playSound ("select");
		SceneManager.LoadScene (5);
	}

	private string recognizeLetter(string letter, byte[] picture, int w, int h) {
		bool response = classifier.Call<bool>("recognizeLetter", new object[] { letter, picture, w, h });
		if (response) {
			return "true";
		} else {
			return "false";
		}
	}
}
