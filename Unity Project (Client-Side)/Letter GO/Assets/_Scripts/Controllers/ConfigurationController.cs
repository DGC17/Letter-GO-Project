using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ConfigurationController : MonoBehaviour {

	private sharedVariables sharedVariables;
	private soundPlayer soundPlayer;

	private Button ReturnGame;
	private Button RepeatTutorial;

	private InputField IPPort_Input;

	// Use this for initialization
	void Start () {
		sharedVariables = GameObject.Find ("sharedVariables").GetComponent<sharedVariables> ();
		soundPlayer = GameObject.Find ("soundPlayer").GetComponent<soundPlayer> ();
		ReturnGame = GameObject.Find ("ReturnGame").GetComponent<Button> ();
		RepeatTutorial = GameObject.Find ("RepeatTutorial").GetComponent<Button> ();
		IPPort_Input = GameObject.Find ("IPPort_Input").GetComponent<InputField> ();

		if (sharedVariables.getUsername ().Length == 0) {
			ReturnGame.interactable = false;
			RepeatTutorial.interactable = false;
		}

		IPPort_Input.text = sharedVariables.getIPPort ();
	}

	private string getIPPortString(string IP, string port) {
		return "http://" + IP + ":" + port;
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void applyChanges() {
		sharedVariables.setIPPort (IPPort_Input.text);
	}

	public void returnToLogin() {
		soundPlayer.playSound ("select");
		sharedVariables.setUsername ("");
		sharedVariables.setScore (0d);
		applyChanges ();
		SceneManager.LoadScene (0);
	}

	public void returnToGame() {
		soundPlayer.playSound ("select");
		applyChanges ();
		SceneManager.LoadScene (1);
	}

	public void repeatTutorial() {
		soundPlayer.playSound ("select");
		applyChanges ();
		SceneManager.LoadScene (6);
	}
}
