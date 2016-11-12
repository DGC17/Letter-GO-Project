using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

// Class to control all the events inside the Login Scene. 
public class UserController : MonoBehaviour {

	// External References.
	private APIController apiController;
	private sharedVariables sharedVariables;
	private soundPlayer soundPlayer;

	// Input fields of the login. 
	private InputField username_Input;
	private InputField password_Input;

	// GameObject and Text to show the errors derived from the API calls. 
	private GameObject errorDialog;
	private Text errorMessage;

	// Toggle that controls if we are calling a login or a register. 
	private Toggle newUser;

	// Button: Send/Register. 
	private Button sendButton;

	// Use this for initialization
	void Start () {

		// Assignations.
		apiController = GameObject.Find ("APIController").GetComponent<APIController> ();
		sharedVariables = GameObject.Find ("sharedVariables").GetComponent<sharedVariables> ();
		soundPlayer = GameObject.Find ("soundPlayer").GetComponent<soundPlayer> ();

		username_Input = GameObject.Find ("Username_Input").GetComponent<InputField> ();
		password_Input = GameObject.Find ("Password_Input").GetComponent<InputField> ();

		errorDialog = GameObject.Find ("Error_Dialog");
		errorMessage = GameObject.Find ("Error_Message").GetComponent<Text> ();

		newUser = GameObject.Find ("NewUser").GetComponent<Toggle> ();

		sendButton = GameObject.Find ("Send").GetComponent<Button> ();

		// Defaults values. 
		errorDialog.SetActive (false);
		sendButton.interactable = false;
	}

	// Update is called once per frame.
	void Update () {
		// If the fields of the Username or the Password are empty we doesn't let 
		// the user interact with the send button. 
		if ((username_Input.text.Length < 1) || (password_Input.text.Length < 1)) {
			sendButton.interactable = false;
		} else {
			sendButton.interactable = true;
		}
	}

	// Method called when the user clicks in the Send/Register button. 
	public void sendRequest() {

		soundPlayer.playSound ("select");

		string username = username_Input.text;
		string password = password_Input.text;

		// We control if it's a login or a register. 
		if (!newUser.isOn) {
			if (!apiController.login (username, password)) {
				errorDialog.SetActive (true);
				errorMessage.text = "Username or Password are incorrect!";
			} else {
				sharedVariables.setUsername (username);
				SceneManager.LoadScene (1);
			}
		} else {
			if (!apiController.register (username, password)) {
				errorDialog.SetActive (true);
				errorMessage.text = "Username already exists!";
			} else {
				sharedVariables.setUsername (username);
				SceneManager.LoadScene (6);
			}
		}
	}

	public void openConfiguration() {
		soundPlayer.playSound ("select");
		SceneManager.LoadScene (5);
	}
}