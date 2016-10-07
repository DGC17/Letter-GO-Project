using UnityEngine;
using System.Collections;

public class ResultController : MonoBehaviour {

	//External References.
	private TimerController timerController;
	private ImageController imageController;
	public GameObject resultInterface;
	public GameObject generalInterface;
	public GameObject mainCamera;
	public GameObject ARCamera;

	// Variables to Control Events.
	private bool returnPrev = false;

	// Functions executed on the intilization of the application. 
	void Start () {

		// Assignations. 
		timerController = GameObject.Find ("TimerController").GetComponent<TimerController>();
		imageController = GameObject.Find ("ImageController").GetComponent<ImageController>();

		returnPrev = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (returnPrev) {

			generalInterface.SetActive (true);
			resultInterface.SetActive (false);

			timerController.interruptTimer (true);
			// Changing from Main Camera to AR Camera. 
			ARCamera.SetActive (true);
			mainCamera.SetActive (false);
			imageController.setDefaultValues ();

			returnPrev = false;
		}
	}

	public void returnGeneral() {
		returnPrev = true;
	}
}
