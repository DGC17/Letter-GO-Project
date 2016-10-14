using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

// Class to control all the events inside the Login Scene. 
public class UserController : MonoBehaviour {

	// A default IP and Port if the user doesn't specifies it. 
	private static readonly string DEFAULT_IPPORT = "localhost:8080";

	// External References.
	private APIController apiController;
	private sharedVariables sharedVariables;

	// Input fields of the login. 
	private InputField username_Input;
	private InputField password_Input;
	private InputField IPPort_Input;

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

		username_Input = GameObject.Find ("Username_Input").GetComponent<InputField> ();
		password_Input = GameObject.Find ("Password_Input").GetComponent<InputField> ();
		IPPort_Input = GameObject.Find ("IPPort_Input").GetComponent<InputField> ();

		errorDialog = GameObject.Find ("Error_Dialog");
		errorMessage = GameObject.Find ("Error_Message").GetComponent<Text> ();

		newUser = GameObject.Find ("NewUser").GetComponent<Toggle> ();

		sendButton = GameObject.Find ("Send").GetComponent<Button> ();

		// Defaults values. 
		errorDialog.SetActive (false);
		sendButton.interactable = false;

		IPPort_Input.text = DEFAULT_IPPORT;
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
		bool isOk = true;

		string username = username_Input.text;
		string password = password_Input.text;

		sharedVariables.setUsername (username);
		sharedVariables.setIPPort (getIPPort ());

		// We control if it's a login or a register. 
		if (!newUser.isOn) {
			isOk = apiController.login (username, password);
			if (!isOk) {
				errorDialog.SetActive (true);
				errorMessage.text =  "Username or Password are incorrect!";
			}
		} else {
			isOk = apiController.register (username, password);
			if (!isOk) {
				errorDialog.SetActive (true);
				errorMessage.text =  "Username already exists!";
			}
		}

		// Load the Main Scene. 
		if (isOk) SceneManager.LoadScene (1);
	}

	// Gets the saved IP and Port. 
	// In case that there isn't any IP and Port saved, returns the default one. 
	public string getIPPort() {
		if (IPPort_Input.text.Length < 1) {
			return DEFAULT_IPPORT;
		} else {
			return IPPort_Input.text;
		}
	}
}