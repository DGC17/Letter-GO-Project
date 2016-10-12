using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class UserController : MonoBehaviour {

	private static readonly string DEFAULT_IPPORT = "localhost:8080";

	private APIController apiController;
	private sharedVariables sharedVariables;

	private InputField username_Input;
	private InputField password_Input;
	private InputField IPPort_Input;

	private GameObject errorDialog;
	private Text errorMessage;

	private Toggle newUser;

	private Button sendButton;

	// Use this for initialization
	void Start () {

		apiController = GameObject.Find ("APIController").GetComponent<APIController> ();
		sharedVariables = GameObject.Find ("sharedVariables").GetComponent<sharedVariables> ();

		username_Input = GameObject.Find ("Username_Input").GetComponent<InputField> ();
		password_Input = GameObject.Find ("Password_Input").GetComponent<InputField> ();
		IPPort_Input = GameObject.Find ("IPPort_Input").GetComponent<InputField> ();

		errorDialog = GameObject.Find ("Error_Dialog");
		errorMessage = GameObject.Find ("Error_Message").GetComponent<Text> ();

		newUser = GameObject.Find ("NewUser").GetComponent<Toggle> ();

		sendButton = GameObject.Find ("Send").GetComponent<Button> ();

		errorDialog.SetActive (false);
		sendButton.interactable = false;

		IPPort_Input.text = DEFAULT_IPPORT;
	}

	// Update is called once per frame.
	void Update () {
		if ((username_Input.text.Length < 1) || (password_Input.text.Length < 1)) {
			sendButton.interactable = false;
		} else {
			sendButton.interactable = true;
		}
	}

	public void sendRequest() {
		bool isOk = true;

		string username = username_Input.text;
		string password = password_Input.text;

		sharedVariables.setUsername (username);
		sharedVariables.setIPPort (getIPPort ());
		
		if (newUser.isOn) {
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

		if (isOk) SceneManager.LoadScene (1);
	}

	public string getIPPort() {
		if (IPPort_Input.text.Length < 1) {
			return DEFAULT_IPPORT;
		} else {
			return IPPort_Input.text;
		}
	}
}