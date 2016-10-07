using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerController : MonoBehaviour {

	// Declaration of the array to store all the letters. 
	public static readonly string[] letters = {	"A","B","C","D","E","F","G","H","I","J","K","L","M",
												"N","O","P","Q","R","S","T","U","V","W","X","Y","Z"};

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
			timerText.text = generateLetter ();

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

	// Generates the Letter.
	// Called on the Event 2. 
	private string generateLetter () {
		float num = Random.Range (0.0f, 26.0f);
		int n = (int)num;
		letter = letters [n].ToString();
		return letter;
	}

	// Gets the Letter.
	// TODO: This is not used for the moment, but it will be used by the Image Controller. 
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
		startTimer = true;
	}
}
