using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AlbumElementController : MonoBehaviour {

	private APIController apiController;
	private sharedVariables sharedVariables;

	private Text Title;
	private Text Author;
	private Text Rate;
	private Text AlbumElementText;

	private Dropdown Letters;
	private Button FillLetter;

	private Image Background;

	private GameObject Dialog;

	// Use this for initialization
	void Start () {
		apiController = GameObject.Find ("APIController").GetComponent<APIController> ();
		sharedVariables = GameObject.Find ("sharedVariables").GetComponent<sharedVariables> ();

		Title = GameObject.Find ("Title").GetComponent<Text> ();
		Author = GameObject.Find ("Author").GetComponent<Text> ();
		Rate = GameObject.Find ("Rate").GetComponent<Text> ();
		AlbumElementText = GameObject.Find ("AlbumElementText").GetComponent<Text> ();

		Letters = GameObject.Find ("Letters").GetComponent<Dropdown> ();
		FillLetter = GameObject.Find ("FillLetter").GetComponent<Button> ();

		Background = GameObject.Find ("Background").GetComponent<Image> ();

		Dialog = GameObject.Find ("Dialog");
		Dialog.SetActive (false);

		LoadInfo ();
		Background.material.mainTexture = Texture2D.whiteTexture;
	}
	
	// Update is called once per frame
	void Update () {
		Title.text = sharedVariables.getAlbumElementSelected ().title.ToUpper();
		Author.text = sharedVariables.getAlbumElementSelected ().author;
		Rate.text = (sharedVariables.getAlbumElementSelected ().rate + "%");
		AlbumElementText.text = sharedVariables.getAlbumElementSelected ().text;
	}

	public void returnAlbum() {
		SceneManager.LoadScene (3);
	}

	public void AddLetter() {
		Dialog.SetActive (true);
		string letter = Letters.captionText.text;
		bool added = apiController.fillLetterAlbumElement (letter);
		if (added) {
			Dialog.GetComponentInChildren<Text> ().text = "Added Letter " + letter + " succesfully!";
			LoadInfo ();
		} else {
			Dialog.GetComponentInChildren<Text> ().text = "The letter " + letter + " wasn't in the Album Element!";
		}
	}

	private void LoadInfo() {
		float rate = sharedVariables.getAlbumElementSelected ().rate;

		Title.text = sharedVariables.getAlbumElementSelected ().title.ToUpper();
		Author.text = sharedVariables.getAlbumElementSelected ().author;
		Rate.text = (rate + "%");
		AlbumElementText.text = sharedVariables.getAlbumElementSelected ().text;

		List<string> list = apiController.getLetters ();

		if (list.Count == 0) {
			Letters.interactable = false;
			FillLetter.interactable = false;
			Dialog.SetActive (true);
			Dialog.GetComponentInChildren<Text> ().text = "You don't have any letter!";
		} else {
			Letters.ClearOptions ();
			Letters.AddOptions (list);

			if (rate == 100.0f) {
				FillLetter.interactable = false;
				Dialog.SetActive (true);
				Dialog.GetComponentInChildren<Text> ().text = "COMPLETED!";
			}
		}


	}
}
