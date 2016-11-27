using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class TutorialController : MonoBehaviour {

	private soundPlayer soundPlayer;
	private sharedVariables sharedVariables;

	private Text DialogText;

	private int dialogShown;
	private int numDialogs;
	private List<DialogItem> dialogs;

	// Use this for initialization
	void Start () {

		soundPlayer = GameObject.Find ("soundPlayer").GetComponent<soundPlayer> ();
		sharedVariables = GameObject.Find ("sharedVariables").GetComponent<sharedVariables> ();

		DialogText = GameObject.Find ("DialogText").GetComponent<Text> ();
		dialogs = new List<DialogItem> ();

		initiliazeDialogs ();

		dialogShown = 0;
		numDialogs = dialogs.Count;
		showDialog ();
	}

	private void initiliazeDialogs() {
		dialogs.Add (new DialogItem ("Hello there!"));
		dialogs.Add (new DialogItem ("This is only a provisional text..."));
		dialogs.Add (new DialogItem ("In the future here will be included the tutorial and the story..."));
		dialogs.Add (new DialogItem ("Have fun!"));
	}

	private void showDialog() {
		DialogText.text = dialogs [dialogShown].getText();
	}

	public void nextDialog() {
		dialogShown++;
		if (dialogShown == numDialogs) {
			finishTutorial ();
		} else {
			soundPlayer.playSound ("select");
			showDialog ();
		}
	}

	public void finishTutorial() {
		soundPlayer.playSound ("select");
		sharedVariables.openScene (1);
	}

	public class DialogItem {
		string text;
		public DialogItem(string text0) {
			text = text0;
		}

		public string getText() {
			return text;
		}
	}
}
