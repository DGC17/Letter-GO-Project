using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResultController : MonoBehaviour {

	//External References.
	private TimerController timerController;
	private sharedVariables sharedVariables;
	public GameObject resultInterface;
	public GameObject generalInterface;
	public GameObject mainCamera;
	public GameObject ARCamera;

	private Text textLetter;
	private Text textAchievedScore;
	private Text textActualScore;

	// Variables to Control Events.
	private bool returnPrev = false;

	// Functions executed on the intilization of the application. 
	void Start () {

		// Assignations. 
		timerController = GameObject.Find ("TimerController").GetComponent<TimerController> ();
		sharedVariables = GameObject.Find ("sharedVariables").GetComponent<sharedVariables> ();

		textLetter = GameObject.Find ("RI.Text.Letter").GetComponent<Text> ();
		textAchievedScore = GameObject.Find ("RI.Text.AchievedScore").GetComponent<Text> ();
		textActualScore = GameObject.Find ("RI.Text.ActualScore").GetComponent<Text> ();

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

			returnPrev = false;
		}
	}

	public void returnGeneral() {
		returnPrev = true;
	}

	public void setTextandScore(string letter, double score) {
		double newScore = (sharedVariables.getScore () + score);
		sharedVariables.setScore (newScore);

		textLetter.text = ("Letter: " + letter);
		textAchievedScore.text = ("Score: " + score);
		textActualScore.text = ("Total Score: " + newScore);
	}
}
