using UnityEngine;
using System.Collections;
using System.IO;
using SimpleJSON;

// Class that implements all the necessary methods to acess the API of the server. 
public class APIController : MonoBehaviour {

	// Common path for all the API calls. 
	private static readonly string COMMON_PATH = "/lettergo/functions/";

	// Class that stores shared variables within the different scenes.
	private sharedVariables sharedVariables;


	// Method called on the awake phase of the GameObject related to this script.  
	void Awake() {
		// We ensure that this script isn't destroyed when we load other scenes. 
		DontDestroyOnLoad (this);
	}

	// Use this for initialization
	void Start () {
		sharedVariables = GameObject.Find ("sharedVariables").GetComponent<sharedVariables> ();
	}

	// Login the user inside of the application. 
	// Used in:
	// - User Controller. 
	public bool login(string username, string password) {
		// Build the URL, the data for the POST and make the API call. 
		string url = sharedVariables.getIPPort() + COMMON_PATH + "loginUser";
		WWWForm form = new WWWForm ();
		form.AddField ("username", username);
		form.AddField ("password", password);
		WWW www = new WWW (url, form);
		// Waits until the API responds. 
		while (!www.isDone) {}
		if (www.error == null) {
			// Build the JSON of the response and set the new score. 
			ScoreItem item = JsonUtility.FromJson<ScoreItem> (www.text);
			sharedVariables.setScore (item.score);
			return true;
		} else {
			return false;
		}
	}

	// Registers the user in the application. 
	// Used in:
	// - User Controller. 
	public bool register(string username, string password) { 
		// Build the URL, the data for the POST and make the API call. 
		string url = sharedVariables.getIPPort() + COMMON_PATH + "addUser";
		WWWForm form = new WWWForm ();
		form.AddField ("username", username);
		form.AddField ("password", password);
		WWW www = new WWW (url, form);
		// Waits until the API responds.
		while (!www.isDone) {}
		// Returns true if the register has gone fine. 
		return (www.error == null);
	}

	// Generates a new letter. 
	// Used in:
	// - Timer Controller. 
	public string generateLetter() {
		// Build the URL and make the API call. 
		string url = sharedVariables.getIPPort() + COMMON_PATH + "generateLetter";
		string response = "";
		WWW www = new WWW(url);
		// Waits until the API responds.
		while (!www.isDone) {}
		// Build the JSON of the response and get the letter. 
		if (www.error == null) {
			response = www.text;
			LetterItem item = JsonUtility.FromJson<LetterItem> (response);
			return item.letter;
		// If there was some error, we send always an A. 
		} else {
			return "A";
		}
	}

	// Send Results from the client to the server, 
	// including the username, the letter and the image.
	// Used in:
	// - Image Controller. 
	public double sendResults(string letter, string image) {
		// Build the URL and make the API call. 
		string url = sharedVariables.getIPPort() + COMMON_PATH + "sendResults";
		WWWForm form = new WWWForm ();
		form.AddField ("username", sharedVariables.getUsername());
		form.AddField ("letter", letter);
		form.AddField ("image", image);
		WWW www = new WWW (url, form);
		// Waits until the API responds.
		while (!www.isDone) {}
		// Results the score calculated by the server. 
		if (www.error == null) {
			ScoreItem item = JsonUtility.FromJson<ScoreItem> (www.text);
			return item.score;
		// If there was some mistake we always send 100d. 
		} else {
			return 100d;
		}
	}

	// Gets the TOP 10 for the users with best score.
	// User in:
	// - Ranking Controller.
	public string[] getTop10() {
		// Build the URL and make the API call. 
		string url = sharedVariables.getIPPort() + COMMON_PATH + "getTop10";
		string response = "";
		WWW www = new WWW(url);
		// Waits until the API responds.
		while (!www.isDone) {}
		// Build the JSON of the response and get the letter. 
		if (www.error == null) {
			response = www.text;
			JSONNode items = JSONArray.Parse (response);
			string[] responses = new string[items.Count];
			int i;
			for (i = 0; i < items.Count; i++) {
				responses[i] = "[" + (i+1) + "] " + items[i]["username"] + " : " + items[i]["score"];
			}
			return responses;
			// If there was some error, we send always an A. 
		} else {
			return null;
		}
	}

	//AUXILIAR CLASSES

	public class LetterItem
	{
		public string letter;
	}

	public class ScoreItem
	{
		public double score;
	}
}
