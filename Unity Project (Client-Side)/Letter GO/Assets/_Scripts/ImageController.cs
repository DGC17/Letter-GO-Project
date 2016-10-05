using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Class responsible of controlling the interface events in the Image Interface. 
public class ImageController : MonoBehaviour {

	// External References.
	private TimerController timerController;
	private GameObject generalInterface;
	private GameObject imageInterface;
	public GameObject mainCamera;
	public GameObject ARCamera;

	// Button: Recognize. 
	private Button recognizeButton;

	// Button: Select.
	private Button selectButton;

	// Buttons: (+ and -)
	private Button increaseButton;
	private Button decreaseButton;

	// Image: Selector.
	private Image selector;

	// Image: Background Image. 
	public Image background;

	// Controls if we are managing 
	public bool managingImage;

	// Functions executed on the intilization of the application. 
	void Start () {

		// Assignations. 
		timerController = GameObject.FindGameObjectWithTag("TimerController").GetComponent<TimerController>();
		imageInterface = GameObject.FindGameObjectWithTag ("ImageInterface");
		generalInterface = GameObject.FindGameObjectWithTag ("GeneralInterface");
		recognizeButton = GameObject.FindGameObjectWithTag ("Recognize").GetComponent<Button> ();
		selectButton = GameObject.FindGameObjectWithTag ("Select").GetComponent<Button> ();
		decreaseButton = GameObject.FindGameObjectWithTag ("Increase").GetComponent<Button> ();
		increaseButton = GameObject.FindGameObjectWithTag ("Decrease").GetComponent<Button> ();
		selector = GameObject.FindGameObjectWithTag("Selector").GetComponent<Image>();

		// Default Values.
		recognizeButton.interactable = false;

		managingImage = false;
	}

	// Update is called once per frame.
	void Update () {
		if (managingImage) {
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

		// Changing from Main Camera to AR Camera. 
		ARCamera.SetActive (true);
		mainCamera.SetActive (false);

		// Changing Interfaces. 
		generalInterface.SetActive (true);
		imageInterface.SetActive (false);

		// Reanudating the Timer... 
		timerController.reanudateTimer();

		managingImage = false;
		selectButton.interactable = true;
		recognizeButton.interactable = false;
		increaseButton.interactable = true;
		decreaseButton.interactable = true;
	}

	public void setSelector () {
		if (managingImage) {
			managingImage = false;
			selector.color = Color.green;
			selectButton.GetComponentInChildren<Text> ().text = "Unselect";
			recognizeButton.interactable = true; 
			increaseButton.interactable = false;
			decreaseButton.interactable = false;
		} else {
			managingImage = true;
			selector.color = Color.red;
			selectButton.GetComponentInChildren<Text>().text = "Select";
			recognizeButton.interactable = false; 
			increaseButton.interactable = true;
			decreaseButton.interactable = true;
		}
	}

	// Recognizes the letter selected by the user. 
	// TODO: Not Implemented.
	// Called when the user clicks on the button "Recognize". 
	public void recognizeLetter () {
		returnPrevious ();
	}
}
