using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ConfigurationController : MonoBehaviour {

	private sharedVariables sharedVariables;
	private soundPlayer soundPlayer;

	private Button ReturnGame;
	private Button RepeatTutorial;
	private GameObject SoundON;
	private GameObject SoundOFF;

	private InputField IPPort_Input;

	// Use this for initialization
	void Start () {
		sharedVariables = GameObject.Find ("sharedVariables").GetComponent<sharedVariables> ();
		soundPlayer = GameObject.Find ("soundPlayer").GetComponent<soundPlayer> ();
		ReturnGame = GameObject.Find ("ReturnGame").GetComponent<Button> ();
		RepeatTutorial = GameObject.Find ("RepeatTutorial").GetComponent<Button> ();
		SoundON = GameObject.Find ("Sound ON");
		SoundOFF = GameObject.Find ("Sound OFF");
		IPPort_Input = GameObject.Find ("IPPort_Input").GetComponent<InputField> ();

		SoundOFF.SetActive (!soundPlayer.switchBackgroundMusic (false));
		SoundON.SetActive (soundPlayer.switchBackgroundMusic (false));

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
		sharedVariables.setUsername ("");
		sharedVariables.setScore (0d);
		exitConfiguration (0);
	}

	public void returnToGame() {
		exitConfiguration (1);
	}

	public void repeatTutorial() {
		exitConfiguration(6);
	}

	public void switchBackgroundMusic() {
		bool isPlaying = soundPlayer.switchBackgroundMusic (true);
		if (isPlaying) {
			SoundON.SetActive (true);
			SoundOFF.SetActive (false);
		} else {
			SoundOFF.SetActive (true);
			SoundON.SetActive (false);
		}
	}

	private void exitConfiguration(int scene) {
		soundPlayer.playSound ("select");
		applyChanges ();
		sharedVariables.openScene (scene);
	}

	public void openDetails() {
		soundPlayer.playSound ("select");
		SceneManager.LoadScene (7);
	}
}
