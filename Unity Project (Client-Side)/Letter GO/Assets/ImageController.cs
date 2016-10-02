using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageController : MonoBehaviour {

	private GameObject generalInterface;
	private GameObject imageInterface;
	public GameObject mainCamera;
	public GameObject ARCamera;

	private Button recognize_button;

	void Start () {
		recognize_button = GameObject.FindGameObjectWithTag ("Recognize").GetComponent<Button> ();

		imageInterface = GameObject.FindGameObjectWithTag ("ImageInterface");
		generalInterface = GameObject.FindGameObjectWithTag ("GeneralInterface");

		recognize_button.interactable = false;
	}

	public void returnPrevious () {
		ARCamera.SetActive (true);
		mainCamera.SetActive (false);
		generalInterface.SetActive (true);
		imageInterface.SetActive (false);
	}

	public void recognizeLetter () {
		returnPrevious ();
	}
}
