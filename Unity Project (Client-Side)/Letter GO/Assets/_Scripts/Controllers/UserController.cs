using UnityEngine;
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
	private Image Panel;

	private bool isLogin;

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

		sendButton = GameObject.Find ("Send").GetComponent<Button> ();
		Panel = GameObject.Find ("Panel").GetComponent<Image> ();

		// Defaults values. 
		errorDialog.SetActive (false);
		sendButton.interactable = false;
		isLogin = true;
	}

	// Update is called once per frame.
	void Update () {
		// If the fields of the Username or the Password are empty we doesn't let 
		// the user interact with the send button. 
		if ((username_Input.text.Length < 1) || (password_Input.text.Length < 1)) {
			sendButton.interactable = false;
			sendButton.GetComponentInChildren<Text> ().color = Color.gray;
		} else {
			sendButton.interactable = true;
			sendButton.GetComponentInChildren<Text> ().color = Color.black;
		}
	}

	public void changeToLogin() {
		Color32 c = new Color32 (0, 90, 140, 100);
		Panel.color = c;
		sendButton.GetComponentInChildren<Text> ().text = "LOGIN";
		isLogin = true;
	}

	public void changeToRegister() {
		Color32 c = new Color32 (140, 100, 0, 100);
		Panel.color = c;
		sendButton.GetComponentInChildren<Text> ().text = "REGISTER";
		isLogin = false;
	}

	// Method called when the user clicks in the Send/Register button. 
	public void sendRequest() {

		soundPlayer.playSound ("select");

		string username = username_Input.text;
		string password = password_Input.text;
		int scene = 0;
		bool success = false;

		// We control if it's a login or a register. 
		if (isLogin) {
			if (!apiController.login (username, password)) {
				errorDialog.SetActive (true);
				errorMessage.text = "Username or Password are incorrect!";
			} else {
				scene = 1;
				success = true;
			}
		} else {
			if (!apiController.register (username, password)) {
				errorDialog.SetActive (true);
				errorMessage.text = "Username already exists!";
			} else {
				scene = 6;
				success = true;
			}
		}

		if (success) {
			sharedVariables.setUsername (username);
			sharedVariables.openScene (scene);
		}
	}

	public void openConfiguration() {
		soundPlayer.playSound ("select");
		sharedVariables.openScene (5);
	}
}