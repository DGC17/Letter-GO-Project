using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResultController : MonoBehaviour {

	//External References.
	private TimerController timerController;
	private sharedVariables sharedVariables;
	private soundPlayer soundPlayer;
	public GameObject resultInterface;
	public GameObject generalInterface;
	public GameObject mainCamera;
	public GameObject ARCamera;

	private Text textLetter;
	private Text textAchievedScore;
	private Text textActualScore;

	// Variables to Control Events.
	private bool returnPrev = false;

	private GameObject returnButton;
	private GameObject scanningInterface;

	// Functions executed on the intilization of the application. 
	void Start () {

		// Assignations. 
		timerController = GameObject.Find ("TimerController").GetComponent<TimerController> ();
		sharedVariables = GameObject.Find ("sharedVariables").GetComponent<sharedVariables> ();
		soundPlayer = GameObject.Find ("soundPlayer").GetComponent<soundPlayer> ();

		textLetter = GameObject.Find ("RI.Text.Letter").GetComponent<Text> ();
		textAchievedScore = GameObject.Find ("RI.Text.AchievedScore").GetComponent<Text> ();
		textActualScore = GameObject.Find ("RI.Text.ActualScore").GetComponent<Text> ();

		returnButton = GameObject.Find ("RI.Return");
		scanningInterface = GameObject.Find ("RI.Scanning");

		returnButton.SetActive (false);
		returnPrev = false;
	}
	
	// Update is called once per frame
	void Update () {

		scanningInterface.GetComponentInChildren<Text>().color = new Color(
			scanningInterface.GetComponentInChildren<Text>().color.r, 
			scanningInterface.GetComponentInChildren<Text>().color.g, 
			scanningInterface.GetComponentInChildren<Text>().color.b, 
			Mathf.PingPong(Time.time, 1));

		if (returnPrev) {

			scanningInterface.SetActive (true);
			returnButton.SetActive (false);
			textLetter.text = "";
			textAchievedScore.text = "";
			textActualScore.text = "";

			generalInterface.SetActive (true);
			resultInterface.SetActive (false);

			timerController.interruptTimer (true);
			// Changing from Main Camera to AR Camera. 
			ARCamera.SetActive (true);
			mainCamera.SetActive (false);

			returnPrev = false;
		}
	}

	public void returnGeneral() {
		soundPlayer.playSound ("select");
		returnPrev = true;
	}

	public void setTextandScore(string letter, double score, string recognitionResult) {

		scanningInterface.SetActive (false);
		returnButton.SetActive (true);

		double newScore = (sharedVariables.getScore () + score);
		sharedVariables.setScore (newScore);

		if (recognitionResult.Contains ("Recognized")) {
			textLetter.text = ("You scanned correctly the letter " + letter + "!");
			textAchievedScore.text = ("Score: " + score);
			textActualScore.text = ("Total Score: " + newScore);
		}

		if (recognitionResult.Contains ("NoLetter")) {
			textLetter.text = ("The scanner doesn't found any letter!");
			textAchievedScore.text = ("Score: " + score);
			textActualScore.text = ("Total Score: " + newScore);
		}

		if (recognitionResult.Contains ("AnotherLetter")) {
			textLetter.text = ("The scanner doesn't found the letter " + letter + ", but it knows that there's a letter there. Maybe next time!");
			textAchievedScore.text = ("Score: " + score);
			textActualScore.text = ("Total Score: " + newScore);
		}		
	}
}
