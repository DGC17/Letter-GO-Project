using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Class responsible of controlling the Timer.
public class TimerController : MonoBehaviour {

	// Wrapper for all the Timer elements.
	private GameObject timer;

	// Button: (Start Timer)
	private GameObject timer_button;

	// Timer UI Elements. 
	private Image timer_filler;
	private Text timer_text;

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
		timer = GameObject.FindGameObjectWithTag ("LetterTimer");
		timer_button = GameObject.FindGameObjectWithTag ("Timer_Button");
		timer_filler = GameObject.FindGameObjectWithTag ("Timer_Image").GetComponent<Image>();
		timer_text = GameObject.FindGameObjectWithTag ("Timer_Text").GetComponent<Text>();

		// Default Values.
		startTimer = false;
		isTimerOn = false;
		stopTimer = false;
		timer.SetActive (false);

	}

	// Update is called once per frame.
	void Update () {

		// Event 1: While the Timer is running.
		if (isTimerOn) {

			// Access and Reduce remaining time. 
			float aux = timer_filler.fillAmount;
			aux -= timerReduction;

			// If there isn't more time... 
			if (aux <= 0) {
				// Launch Event 3.
				stopTimer = true;
			} else {
				// Assign remaining time. 
				timer_filler.fillAmount = aux;
			}
		}

		// Event 2: When the Timer is started. 
		if (startTimer) {
			
			// Initialicing start Timer parameters.
			timer.SetActive (true);
			timer_button.SetActive (false);
			timer_filler.fillAmount = 1.0f;
			timer_text.text = generateLetter ();

			// Finishing Event 2. 
			startTimer = false;

			// Launch Event 1.
			isTimerOn = true;

		}

		// Event 3: When the Timer is stoped. 
		if (stopTimer) {

			//Resetting default Timer parameters. 
			timer.SetActive (false);
			timer_button.SetActive (true);
			timer_filler.fillAmount = 0.0f;
			timer_text.text = "";

			//Finishing Events 1 & 3.
			stopTimer = false;
			isTimerOn = false;

		}

	}

	// Generates the Letter.
	// TODO: Random generation of the letter.
	// Called on the Event 2. 
	private string generateLetter () {
		letter = "A";
		return letter;
	}

	// Gets the Letter.
	// TODO: This is not used for the moment, but it will be used by the Image Controller. 
	public string getLetter() {
		return letter;
	}

	// Forces the Timer Interruption. 
	// Called when the users takes a picture (See Camera Controller). 
	public void interruptTimer() {
		stopTimer = true;
	}

	// Starts the Timer (Launch Event 2).
	// Called when the user clicks on the button "(Start Timer)".
	public void Play_onClick () {
		startTimer = true;
	}
}
