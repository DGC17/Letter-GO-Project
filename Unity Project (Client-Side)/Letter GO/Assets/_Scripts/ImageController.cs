using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Class responsible of controlling the interface events in the Image Interface. 
public class ImageController : MonoBehaviour {

	// External References.
	private GameObject generalInterface;
	private GameObject imageInterface;
	public GameObject mainCamera;
	public GameObject ARCamera;

	// Button: Recognize. 
	private Button recognize_button;

	// Functions executed on the intilization of the application. 
	void Start () {

		// Assignations. 
		imageInterface = GameObject.FindGameObjectWithTag ("ImageInterface");
		generalInterface = GameObject.FindGameObjectWithTag ("GeneralInterface");
		recognize_button = GameObject.FindGameObjectWithTag ("Recognize").GetComponent<Button> ();

		// Default Values.
		recognize_button.interactable = false; 	// TODO: This button will be interactable in future versions. 

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

	}

	// Recognizes the letter selected by the user. 
	// TODO: Not Implemented.
	// Called when the user clicks on the button "Recognize". 
	public void recognizeLetter () {
		returnPrevious ();
	}
}
