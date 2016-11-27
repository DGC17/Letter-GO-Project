using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AlbumElementController : MonoBehaviour {

	private APIController apiController;
	private sharedVariables sharedVariables;
	private soundPlayer soundPlayer;

	private Text Title;
	private Text Author;
	private Text Rate;
	private Text AlbumElementText;

	private Dropdown Letters;
	private Button FillLetter;

	private GameObject Dialog;

	// Use this for initialization
	void Start () {
		apiController = GameObject.Find ("APIController").GetComponent<APIController> ();
		sharedVariables = GameObject.Find ("sharedVariables").GetComponent<sharedVariables> ();
		soundPlayer = GameObject.Find ("soundPlayer").GetComponent<soundPlayer> ();

		Title = GameObject.Find ("Title").GetComponent<Text> ();
		Author = GameObject.Find ("Author").GetComponent<Text> ();
		Rate = GameObject.Find ("Rate").GetComponent<Text> ();
		AlbumElementText = GameObject.Find ("AlbumElementText").GetComponent<Text> ();

		Letters = GameObject.Find ("Letters").GetComponent<Dropdown> ();
		FillLetter = GameObject.Find ("FillLetter").GetComponent<Button> ();

		Dialog = GameObject.Find ("Dialog");
		Dialog.GetComponentInChildren<Text> ().text = "You can add your stored letters in this text by selecting them and pushing the Add button!\nBe careful, you will lose the letter if it isn't in the text!";

		LoadInfo ();
	}
	
	// Update is called once per frame
	void Update () {
		Title.text = sharedVariables.getAlbumElementSelected ().title.ToUpper();
		Author.text = sharedVariables.getAlbumElementSelected ().author;
		Rate.text = (sharedVariables.getAlbumElementSelected ().rate + "%");
		AlbumElementText.text = sharedVariables.getAlbumElementSelected ().text;
	}

	public void returnAlbum() {
		soundPlayer.playSound ("select");
		SceneManager.LoadScene (3);
	}

	public void AddLetter() {
		soundPlayer.playSound ("select");
		string letter = Letters.captionText.text;
		bool added = apiController.fillLetterAlbumElement (letter);
		if (added) {
			if (sharedVariables.isShowGlobalAlbum ()) {
				Dialog.GetComponentInChildren<Text> ().text = "You added the letter " + letter + " succesfully!\n (+150 Score)";
			} else {
				Dialog.GetComponentInChildren<Text> ().text = "You added the letter " + letter + " succesfully!\nKeep working hard!";
			}

			LoadInfo ();
		} else {
			Dialog.GetComponentInChildren<Text> ().text = "The letter " + letter + " doesn't fit here!\nTry another one...";
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
			Dialog.GetComponentInChildren<Text> ().text = "You don't have any letter stored!\nIt's time to find more!";
		} else {
			Letters.ClearOptions ();
			Letters.AddOptions (list);

			if (rate == 100.0f) {
				FillLetter.interactable = false;
				Dialog.GetComponentInChildren<Text> ().text = "Congratulations! This text is COMPLETED!\n(+ 1000 Score)";
			}
		}


	}
}
