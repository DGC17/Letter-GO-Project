using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Class to control the timer. 
public class TimerController : MonoBehaviour {

	// External References.
	private APIController apiController;
	private soundPlayer soundPlayer;

	// Wrapper for all the Timer elements.
	private GameObject timer;

	// Button: (Start Timer)
	private GameObject timerButton;

	// Button: Take Picture
	private Button takePictureButton;

	// Timer UI Elements. 
	private Image timerFiller;
	private Text timerText;

	// Variables to Control Events. 
	private bool startTimer;
	private bool isTimerOn;
	private bool stopTimer;

	// Stores the generated Letter.
	private string letter;

	// Specifies the rate of the "Reduction" of the time. 
	public float timerReduction;

	// Functions executed on the intilization of the application. 
	void Start () {

		// Assignations.
		apiController = GameObject.Find ("APIController").GetComponent<APIController> ();
		soundPlayer = GameObject.Find ("soundPlayer").GetComponent<soundPlayer> ();

		timer = GameObject.Find ("GI.LT.LetterTimer");
		timerFiller = GameObject.Find ("GI.LT.LetterTimer.Image").GetComponent<Image>();
		timerText = GameObject.Find ("GI.LT.LetterTimer.Text").GetComponent<Text>();
		timerButton = GameObject.Find ("GI.LT.Button");
		takePictureButton = GameObject.Find ("GI.TakePicture").GetComponent<Button>();

		// Default Values.
		startTimer = false;
		isTimerOn = false;
		stopTimer = false;
		timer.SetActive (false);
		takePictureButton.interactable = false;

	}

	// Update is called once per frame.
	void Update () {

		// Event 1: While the Timer is running.
		if (isTimerOn) {

			// Access and Reduce remaining time. 
			float aux = timerFiller.fillAmount;
			aux -= timerReduction;

			// If there isn"t more time... 
			if (aux <= 0) {
				// Launch Event 3.
				stopTimer = true;
			} else {
				// Assign remaining time. 
				timerFiller.fillAmount = aux;
			}
		}

		// Event 2: When the Timer is started. 
		if (startTimer) {
			
			// Initialicing start Timer parameters.
			timer.SetActive (true);
			timerButton.SetActive (false);
			timerFiller.fillAmount = 1.0f;

			letter = apiController.generateLetter();
			timerText.text = letter; 

			takePictureButton.interactable = true;

			// Finishing Event 2. 
			startTimer = false;

			// Launch Event 1.
			isTimerOn = true;
		}

		// Event 3: When the Timer is stoped. 
		if (stopTimer) {

			//Resetting default Timer parameters. 
			timer.SetActive (false);
			timerButton.SetActive (true);
			timerFiller.fillAmount = 0.0f;
			timerText.text = "";

			takePictureButton.interactable = false;

			//Finishing Events 1 & 3.
			stopTimer = false;
			isTimerOn = false;

		}

	}

	// Gets the Letter.
	public string getLetter() {
		return letter;
	}

	// Forces the Timer Interruption. 
	// Called when the user takes a picture (See Camera Controller). 
	public void interruptTimer(bool isStop) {
		isTimerOn = false;
		stopTimer = isStop;
	}

	// Forces the Timer Reanudation. 
	// Called when the user clicks on the button "Return". 
	public void reanudateTimer() {
		isTimerOn = true;
	}

	// Starts the Timer (Launch Event 2).
	// Called when the user clicks on the button "(Start Timer)".
	public void Play_onClick () {
		soundPlayer.playSound ("select");
		startTimer = true;
	}
}
