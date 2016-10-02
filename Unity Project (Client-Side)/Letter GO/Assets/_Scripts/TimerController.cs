using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerController : MonoBehaviour {

	private GameObject timer;
	private GameObject timer_button;
	private GameObject general_interface;
	private Image timer_filler;
	private Text timer_text;

	private bool startTimer;
	private bool isTimerOn;
	private bool stopTimer;

	private string letter;

	public float timerReduction;

	void Start () {

		timer = GameObject.FindGameObjectWithTag ("LetterTimer");
		timer_button = GameObject.FindGameObjectWithTag ("Timer_Button");
		timer_filler = GameObject.FindGameObjectWithTag ("Timer_Image").GetComponent<Image>();
		timer_text = GameObject.FindGameObjectWithTag ("Timer_Text").GetComponent<Text>();

		startTimer = false;
		isTimerOn = false;
		stopTimer = false;

		timer.SetActive (false);
		timer_button.SetActive (true);

	}

	void Update () {

		if (isTimerOn) {

			float aux = timer_filler.fillAmount;
			aux -= timerReduction;

			if (aux <= 0) {
				stopTimer = true;
			} else {
				timer_filler.fillAmount = aux;
			}
		}

		if (startTimer) {
			timer.SetActive (true);
			timer_button.SetActive (false);
			timer_filler.fillAmount = 1.0f;
			timer_text.text = generateLetter ();
			startTimer = false;
			isTimerOn = true;
		}

		if (stopTimer) {
			timer.SetActive (false);
			timer_button.SetActive (true);
			timer_filler.fillAmount = 0.0f;
			timer_text.text = "";
			stopTimer = false;
			isTimerOn = false;
		}
			

	}

	private string generateLetter () {
		letter = "A";
		return letter;
	}

	public string getLetter() {
		return letter;
	}

	public void interruptTimer() {
		stopTimer = true;
	}

	public void Play_onClick () {
		startTimer = true;
	}
}
