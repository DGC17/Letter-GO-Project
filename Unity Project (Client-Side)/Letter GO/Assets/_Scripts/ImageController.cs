using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System;

public class ImageController : MonoBehaviour {

	// External References.
	private TimerController timerController;
	private ResultController resultController;
	private APIController apiController;
	private GameObject generalInterface;
	private GameObject imageInterface;
	private GameObject resultInterface;
	public GameObject mainCamera;
	public GameObject ARCamera;

	// Button: Recognize. 
	private Button recognizeButton;

	// Button: Select.
	private Button selectButton;

	// Image: Selector.
	private Image selector;

	// Image: Background Image. 
	private Image background;

	// Image: Result Image.
	private Image resultImage;

	// Controls the size of the selector.
	private Scrollbar sizeControl;

	// Controls if we are managing.
	public bool managingImage;
	public bool changeInterface;

	// Functions executed on the intilization of the application. 
	void Start () {

		// Assignations. 
		timerController = GameObject.Find ("TimerController").GetComponent<TimerController> ();
		resultController = GameObject.Find ("ResultController").GetComponent<ResultController> ();
		apiController = GameObject.Find ("APIController").GetComponent<APIController> ();

		imageInterface = GameObject.Find ("ImageInterface");
		generalInterface = GameObject.Find ("GeneralInterface");
		resultInterface = GameObject.Find ("ResultInterface");

		recognizeButton = GameObject.Find ("II.Recognize").GetComponent<Button> ();
		selectButton = GameObject.Find ("II.Select").GetComponent<Button> ();
		selector = GameObject.Find ("II.Image.Selector").GetComponent<Image>();
		background = GameObject.Find ("II.Background").GetComponent<Image>();
		sizeControl = GameObject.Find ("II.ControlSize").GetComponent<Scrollbar> ();

		resultImage = GameObject.Find ("RI.Image").GetComponent<Image>();

		// Default Values.
		recognizeButton.interactable = false;

		// Variables to Control Events.
		managingImage = false;
		changeInterface = false;
	}

	// Update is called once per frame.
	void Update () {

		// Event 1: When we want to change to the Result Interface.
		if (changeInterface) {

			imageInterface.SetActive (false);
			resultInterface.SetActive (true);

			changeInterface = false;
		}
			
		if (managingImage) {
			// Sets the size of the Selector.
			setSelectorSize ();

			// If theres a touch detected on the screen. 
			if (Input.touchCount == 1) {
				Vector2 touch_position = Input.GetTouch (0).position;
				float selector_width = selector.rectTransform.rect.width;
				float selector_height = selector.rectTransform.rect.height;
				float x = touch_position.x;
				float y = touch_position.y;

				if (y >= background.rectTransform.rect.height) {
					if ((x + (selector_width / 2)) > Screen.width)
						x = Screen.width - (selector_width / 2);
					if ((y + (selector_height / 2)) > Screen.height)
						y = Screen.height - (selector_height / 2);
					if ((x - (selector_width / 2)) < 0.0f)
						x = selector_width / 2;				
					selector.transform.position = new Vector2 (x, y);
				}
			}
		}
	}

	// Return the interface to the General Interface (Previous one). 
	// Called when the user clicks on the button "Return". 
	public void returnPrevious () {

		// Default values for some options...
		setDefaultValues();

		// Changing from Main Camera to AR Camera. 
		ARCamera.SetActive (true);
		mainCamera.SetActive (false);

		// Changing Interfaces. 
		generalInterface.SetActive (true);
		imageInterface.SetActive (false);

		// Reanudating the Timer... 
		timerController.reanudateTimer();
	}

	// Fixes the selected part of the image. 
	public void setSelector () {
		if (managingImage) {
			managingImage = false;
			selector.color = Color.green;
			selectButton.GetComponentInChildren<Text> ().text = "Unselect";
			recognizeButton.interactable = true; 
			sizeControl.interactable = false;
		} else {
			managingImage = true;
			selector.color = Color.red;
			selectButton.GetComponentInChildren<Text>().text = "Select";
			recognizeButton.interactable = false; 
			sizeControl.interactable = true;
		}
	}

	// Recognizes the letter selected by the user. 
	// Called when the user clicks on the button "Recognize". 
	public void recognizeLetter () {
		
		byte[] image = getImageSelected ();

		string letter = timerController.getLetter ();
		string imageb64 = Convert.ToBase64String (image);

		double score = apiController.sendResults (letter, imageb64);

		resultController.setTextandScore (letter, score);

		Texture2D texture = new Texture2D (
			(int)(selector.rectTransform.rect.width*0.85f), 
			(int)(selector.rectTransform.rect.height*0.85f));
		texture.LoadImage (image);
		texture.Apply ();

		resultImage.material.mainTexture = texture;
		changeInterface = true;
	}

	// Gets the Image Selected.
	private byte[] getImageSelected() {

		float w = selector.rectTransform.rect.width*0.85f;
		float h = selector.rectTransform.rect.height*0.85f;
		Vector2 pos = selector.rectTransform.position;

		Rect rect = new Rect();
		rect.xMin = (pos.x - w / 2); 
		rect.xMax = (pos.x + w / 2);
		rect.yMin = (pos.y - w / 2);
		rect.yMax = (pos.y + w / 2);
		Texture2D texture = new Texture2D((int)w, (int)h, TextureFormat.RGB24, false);
		texture.ReadPixels(rect, 0, 0);
		texture.Apply();
		byte[] bytes = texture.EncodeToPNG();
		Destroy (texture);

		// Just for debug purposes... 
		File.WriteAllBytes (Application.persistentDataPath + "/screen.png", bytes);

		return bytes;
	}

	// Sets the size of the Selector.
	private void setSelectorSize () {
		float size = (500.0f*sizeControl.value + 100.0f);
		selector.rectTransform.sizeDelta = new Vector2 (size, size);
	}

	// Sets the defaults values. 
	public void setDefaultValues () {
		managingImage = false;
		setSelector ();
		managingImage = false;
		recognizeButton.interactable = false;
		sizeControl.interactable = true;
	}
}
