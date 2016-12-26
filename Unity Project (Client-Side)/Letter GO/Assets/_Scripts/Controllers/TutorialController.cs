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
		dialogs.Add (new DialogItem ("Hello there, my new friend!"));
		dialogs.Add (new DialogItem ("My name is Proffesor Alphabet and this is a direct transmission from the future!"));
		dialogs.Add (new DialogItem ("If you are watching this, it means that you have in your hands a packet containg a tool that could save your future."));
		dialogs.Add (new DialogItem ("But before opening it, let me explain you what is happening here."));
		dialogs.Add (new DialogItem ("In the future a malicious magic have appeared that is taking away all the letters from the world."));
		dialogs.Add (new DialogItem ("That means that years and years of knowledge are dissapering from ancient texts."));
		dialogs.Add (new DialogItem ("It is your task to recover this letters so we can recover this texts!"));
		dialogs.Add (new DialogItem ("To do it, you will use the tool in that packet, the Alphabet Scanner!"));
		dialogs.Add (new DialogItem ("Use it is very simple, you must activate it, then generate a letter and then search this letter in your world."));
		dialogs.Add (new DialogItem ("The letters will be scanned and stored inside the device and you will use them to fill your album of ancient texts."));
		dialogs.Add (new DialogItem ("Don't lose any more time, let's begin your journey. And don't forget..."));
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
