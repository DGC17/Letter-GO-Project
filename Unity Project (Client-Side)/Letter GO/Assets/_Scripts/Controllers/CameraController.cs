using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Threading;

using Vuforia;
using com.youvisio;

public class CameraController : MonoBehaviour {

	//Available Cameras
	public GameObject mainCamera;
	public GameObject ARCamera;

	//External References.
	private sharedVariables sharedVariables;
	private soundPlayer soundPlayer;
	private TimerController timerController;
	private ResultController resultController;
	private APIController apiController;
	private TipController tipController;

	// Views.
	private GameObject resultInterface;
	private GameObject generalInterface;

	//IU Elements.
	private GameObject ScanOnInterface;
	private GameObject ScanOffInterface;
	private GameObject selector;
	private UnityEngine.UI.Image image;
	private Text userName;
	private Text userScore;

	private Button letterTimerButton;
	private Button takePictureButton;

	// Variables to Control Events.
	private bool ARCameraActive;
	private bool changeInterface;

	// External Objects
	private AndroidJavaObject classifier;

	private BackgroundWorker _backgroundWorker;

	// Functions executed on the intilization of the application. 
	void Start () {

		// Assignations.
		sharedVariables = GameObject.Find ("sharedVariables").GetComponent<sharedVariables> (); 
		soundPlayer = GameObject.Find ("soundPlayer").GetComponent<soundPlayer> ();
		timerController = GameObject.Find ("TimerController").GetComponent<TimerController> ();
		resultController = GameObject.Find ("ResultController").GetComponent<ResultController> ();
		apiController = GameObject.Find ("APIController").GetComponent<APIController> ();

		generalInterface = GameObject.Find ("GeneralInterface");
		resultInterface = GameObject.Find ("ResultInterface");

		ScanOnInterface = GameObject.Find ("ScanOnInterface");
		ScanOffInterface = GameObject.Find ("ScanOffInterface");
		selector = GameObject.Find ("GI.Selector");
		image = GameObject.Find ("RI.Image").GetComponent<UnityEngine.UI.Image>();
		userName = GameObject.Find ("GI.UserName").GetComponent<Text>();
		userScore = GameObject.Find ("GI.UserScore").GetComponent<Text>();
		letterTimerButton = GameObject.Find ("GI.LT.Button").GetComponent<Button> ();
		takePictureButton = GameObject.Find ("GI.TakePicture").GetComponent<Button> ();

		// Default Values.
		ARCameraActive = false;
		changeInterface = false;
		ARCamera.SetActive (false);
		ScanOffInterface.SetActive (true);
		ScanOnInterface.SetActive (false);
		letterTimerButton.interactable = false;

		userName.text = "Welcome " + sharedVariables.getUsername ();
		userScore.text = "Score: " + sharedVariables.getScore().ToString();

		resultInterface.SetActive (false);

		// Start service before querying location
		Input.location.Start();

		//JAVA CLASSIFIER INITIALIZATION
		classifier = new AndroidJavaObject ("wrappers.Classifier");
		classifier.Call ("loadDefaultModel");
	}

	// Update is called once per frame.
	void Update () {

		userScore.text = "Score: " + sharedVariables.getScore().ToString();

		if (_backgroundWorker != null) _backgroundWorker.Update();

		// Event 1: When we want to change to the Image Interface. 
		if (changeInterface) {

			sharedVariables.setScanning (false);

			// Changing Interfaces. 
			resultInterface.SetActive (true);
			generalInterface.SetActive (false);

			// Interrupting the timer because we already captured the Letter. 
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

		GameObject g = GameObject.Find ("ScanTip");
		GameObject g2 = GameObject.Find ("ScanTip2");
		if (g != null) g.SetActive (false);
		if (g2 != null) g2.SetActive (false);

		soundPlayer.playSound ("select");
		if (ARCameraActive) {
			letterTimerButton.interactable = false;
			timerController.interruptTimer (true);
			mainCamera.SetActive (true);
			ARCamera.SetActive (false);
			ScanOnInterface.SetActive (false);
			ScanOffInterface.SetActive (true);
		} else {
			letterTimerButton.interactable = true;
			ARCamera.SetActive (true);
			mainCamera.SetActive (false);
			ScanOffInterface.SetActive (false);
			ScanOnInterface.SetActive (true);
		}

		ARCameraActive = !ARCameraActive;
	}

	public void openRanking() {
		soundPlayer.playSound ("select");
		sharedVariables.openScene (2);
	}

	public void openAlbum() {
		soundPlayer.playSound ("select");
		sharedVariables.openScene (3);
	}

	public void openConfiguration() {
		soundPlayer.playSound ("select"); 
		sharedVariables.openScene (5);
	}

	private string recognizeLetter(string letter, byte[] picture, int w, int h) {
		
		string response;
		try {
			response = classifier.Call<string>("recognizeLetter", new object[] { letter, picture, w, h });
		} catch (Exception e) {
			Debug.Log ("CLASSIFIER ERROR: " + e.Message);
			response = "Recognized";
		}

		return response;
	}

	private BytesArrays applyTransformations(Vuforia.Image img, float radiusCircle, Vector2 pos) {

		BytesArrays results = new BytesArrays();

		// Size of the image taked by Vuforia. 
		int w = img.Width;
		int h = img.Height;

		// Defintion of different Textures representing the different steps of the picture transformation.
		Texture2D tex = new Texture2D (w, h, TextureFormat.RGB24, false);
		Texture2D tex_rotated = new Texture2D (h, w, TextureFormat.RGB24, false);
		Texture2D tex_inverted = new Texture2D (h, w, TextureFormat.RGB24, false);
		Texture2D tex_final = new Texture2D (h, w, TextureFormat.RGB24, false);

		// STEP 1: Original format of the picture taked by the AR Camera. 
		img.CopyToTexture (tex);
		tex.Apply();

		// STEP 2: Rotation of the picture. 
		for (int j = 0; j < h; j++) for (int i = 0; i < w; i++) tex_rotated.SetPixel (j, i, tex.GetPixel(i,j));
		tex_rotated.Apply();

		// STEP 3: Apply mirror effect to the picture. 
		for (int j = 0; j < w; j++) for (int i = 0; i < h; i++) tex_inverted.SetPixel ((h - 1 - i), (w - 1 - j), tex_rotated.GetPixel(i,j));
		tex_inverted.Apply();

		// STEP 4: Reescaling the image. 
		tex_final = Instantiate (tex_inverted);
		TextureScale.Bilinear (tex_final, Screen.width, Screen.height);

		// STEP 5: Cropping the image.
		Color[] pix = tex_final.GetPixels ((int)(pos.x - radiusCircle/2), (int)(pos.y - radiusCircle/2), (int)radiusCircle, (int)radiusCircle);
		Texture2D tex_selected = new Texture2D ((int)radiusCircle, (int)radiusCircle);
		tex_selected.SetPixels (pix);
		tex_selected.Apply ();

		byte[] bytesUnselected = tex_final.EncodeToPNG();
		byte[] bytes = tex_selected.EncodeToPNG ();

		Destroy (tex);
		Destroy (tex_rotated);
		Destroy (tex_inverted);
		Destroy (tex_final);
		Destroy (tex_selected);

		results.complete = bytesUnselected;
		results.selected = bytes;

		return results;
	}

	private string getDeviceLocation() {

		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed)
		{
			return "Location Not Found or Hidden";
		}
		else
		{
			// Access granted and location value could be retrieved
			return (Input.location.lastData.latitude + "-" + Input.location.lastData.longitude + "-" + Input.location.lastData.altitude + "-" + Input.location.lastData.horizontalAccuracy + "-" + Input.location.lastData.timestamp);
		}
	}

	public void scanActualImage(Vuforia.Image img) {

		soundPlayer.playSound ("select");

		sharedVariables.setScanning (true);

		DateTime datestart = DateTime.Now;

		//Selector parameters
		float radiusCircle = selector.GetComponent<UnityEngine.UI.Image>().rectTransform.rect.width;
		Vector2 pos = selector.GetComponent<UnityEngine.UI.Image>().rectTransform.position;

		BytesArrays results = applyTransformations(img, radiusCircle, pos);
		byte[] bytesUnselected = results.complete;
		byte[] bytes = results.selected;

		// We create a new texture to load our image in the Result Interface. 
		Texture2D texture = new Texture2D ((int)radiusCircle, (int)radiusCircle);
		texture.LoadImage (bytes);
		texture.Apply ();

		image.material.mainTexture = texture;

		// Gets the letter. 
		string letter = timerController.getLetter ();
		// Launch Event 1.
		changeInterface = true;

		string output = recognizeLetter(letter, bytes, (int)radiusCircle, (int)radiusCircle);

		//BACKGROUND PROCESSING

		if (_backgroundWorker != null) _backgroundWorker.CancelAsync();
		_backgroundWorker = new BackgroundWorker();

		_backgroundWorker.DoWork += (o, a) =>
		{
			string recognitionResult = "";
			if (output.Length == 0) {
				recognitionResult = "NoLetter";
				output = "...";
			} else {
				recognitionResult = "AnotherLetter";
				if (output.Contains(letter)) recognitionResult = "Recognized";
			}

			string imageb64;
			// We convert the image to Base64, so we can send it through a JSON.
			if (recognitionResult.Contains ("NoLetter")) {
				imageb64 = "null";
			} else {
				imageb64 = Convert.ToBase64String (bytesUnselected);
			}

			string location = getDeviceLocation();

			double score = getScore(letter, recognitionResult);

			DateTime dateend = DateTime.Now;
			double time = (dateend - datestart).TotalMilliseconds;

			string position = "(" + pos.x + "," + pos.y + ")";

			// When we have the results, we set them. 
			resultController.setTextandScore (letter, score, recognitionResult);

			// We call the API method to send the results. (ASYNCHRONOUS!)
			apiController.sendResults (letter, imageb64, recognitionResult, location, score, time, output, position, (int)radiusCircle);
		};

		_backgroundWorker.RunWorkerCompleted += (o, a) => {};
		_backgroundWorker.RunWorkerAsync("A1");
	}

	public class BytesArrays {
		public byte[] selected;
		public byte[] complete;
	}

	private double getScore(string letter, string recResult) {

		Dictionary<string, int> weights = new Dictionary<string, int>
		{
			{ "A", 9 }, { "B", 2 }, { "C", 3 }, { "D", 5 },
			{ "E", 13 }, { "F", 3 }, { "G", 3 }, { "H", 7 },
			{ "I", 7 }, { "J", 1 }, { "K", 1 }, { "L", 5 },
			{ "M", 3 }, { "N", 7 }, { "O", 8 }, { "P", 2 },
			{ "Q", 1 }, { "R", 6 }, { "S", 7 }, { "T", 10 },
			{ "U", 3 }, { "V", 1 }, { "W", 3 }, { "X", 1 },
			{ "Y", 2 }, { "Z", 1 }
		};

		int weight = weights [letter];

		double score = 150d - (weight*10);
		if (recResult == "NoLetter") score = 10d; 
		if (recResult == "AnotherLetter") score = score/2; 

		return score;
	}
}
