using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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

		if ((FindObjectsOfType (GetType ())).Length > 1)
			Destroy (gameObject);
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
	// ASYNCHRONOUS
	// Used in:
	// - Image Controller. 
	public void sendResults(
		string letter, string image, string recognized, string location, double score, double time, string letters, string position, int size) {
		// Build the URL and make the API call. 
		string url = sharedVariables.getIPPort() + COMMON_PATH + "sendResults";
		WWWForm form = new WWWForm ();
		form.AddField ("username", sharedVariables.getUsername());
		form.AddField ("letter", letter);
		form.AddField ("image", image);
		form.AddField ("recognized", recognized);
		form.AddField ("location", location);
		form.AddField ("score", score.ToString());
		form.AddField ("time", time.ToString ());
		form.AddField ("letters", letters);
		form.AddField ("position", position);
		form.AddField ("size", size);
		WWW www = new WWW (url, form);
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
		if (www.error == null) {
			response = www.text;
			JSONNode items = JSONArray.Parse (response);
			string[] responses = new string[items.Count];
			int i;
			for (i = 0; i < items.Count; i++) {
				responses[i] = "[" + (i+1) + "] " + items[i]["username"] + " : " + items[i]["score"];
			}
			return responses;
		} else {
			return null;
		}
	}

	// Gets the Album of the user registered or the Global Album.
	// User in:
	// - Album Controller.
	public void getAlbum() {

		sharedVariables.removeElementsInAlbumElementsInList ();

		// Build the URL and make the API call. 
		string url = sharedVariables.getIPPort() + COMMON_PATH;
		WWW www;

		if (sharedVariables.isShowGlobalAlbum ()) {
			url += "getGlobalAlbum";
			www = new WWW (url);
		} else {
			url += "getAlbum";
			string username = sharedVariables.getUsername ();
			WWWForm form = new WWWForm ();
			form.AddField ("username", username);
			www = new WWW (url, form);
		}

		string response = "";
		// Waits until the API responds.
		while (!www.isDone) {}
		if (www.error == null) {
			response = www.text;
			JSONNode items = JSONArray.Parse (response);
			int i;
			for (i = 0; i < items.Count; i++) {
				sharedVariables.addElementInAlbumElementsInList (
					items [i] ["title"],
					items [i] ["author"],
					items [i] ["rate"].AsFloat);
			}
		}
	}

	// Get an Album Element knowing the user and the title or just the title.
	// User in:
	// - Element Controller.
	public void getAlbumElement(string elementTitle) {
		// Build the URL and make the API call. 
		string url = sharedVariables.getIPPort() + COMMON_PATH;
		WWWForm form;
		WWW www;

		if (sharedVariables.isShowGlobalAlbum ()) {
			url += "getGlobalAlbumElement";
			string title = elementTitle;
			form = new WWWForm ();
			form.AddField ("title", title);
			www = new WWW (url, form);
		} else {
			url += "getAlbumElement";
			string username = sharedVariables.getUsername ();
			string title = elementTitle;
			form = new WWWForm ();
			form.AddField ("username", username);
			form.AddField ("title", title);
			www = new WWW (url, form);
		}

		// Waits until the API responds.
		while (!www.isDone) {}
		if (www.error == null) {
			AlbumElement item = JsonUtility.FromJson<AlbumElement> (www.text);
			sharedVariables.setAlbumElementSelected (
				new sharedVariables.AlbumElementSelected (
					item.title, item.author, item.text, item.type, item.rate));
		}
	}

	// Gets the list of letters of the user.
	// User in:
	// - Album Element Controller.
	public List<string> getLetters() {
		List<string> list = new List<string> ();
		// Build the URL and make the API call. 
		string url = sharedVariables.getIPPort() + COMMON_PATH + "getLetters";
		string username = sharedVariables.getUsername ();
		WWWForm form = new WWWForm ();
		form.AddField ("username", username);
		WWW www = new WWW (url, form);
		// Waits until the API responds.
		while (!www.isDone) {}
		if (www.error == null) {
			LetterList item = JsonUtility.FromJson<LetterList> (www.text);
			for (int i = 0; i < item.letters.Length; i++) {
				list.Add (item.letters [i]);
			}
		} 
		return list;
	}

	// Tries to fill a Letter into an Album Element. 
	// User in:
	// - Album Element Controller. 
	public bool fillLetterAlbumElement(string letterSelected) {
		// Build the URL and make the API call. 

		string url = sharedVariables.getIPPort () + COMMON_PATH;
		if (sharedVariables.isShowGlobalAlbum ()) {
			url += "fillGlobalLetterAlbumElement";
		} else {
			url += "fillLetterAlbumElement";
		}

		string username = sharedVariables.getUsername ();
		string title = sharedVariables.getAlbumElementSelected ().title;
		string letter = letterSelected;
		WWWForm form = new WWWForm ();
		form.AddField ("letter", letter);
		form.AddField ("title", title);
		form.AddField ("username", username);
		WWW www = new WWW (url, form);
		// Waits until the API responds.
		while (!www.isDone) {}
		if (www.error == null) {
			AlbumElement item = JsonUtility.FromJson<AlbumElement> (www.text);
			sharedVariables.setAlbumElementSelected (
				new sharedVariables.AlbumElementSelected (
					item.title, item.author, item.text, item.type, item.rate));
			if (item.success != 0d) {
				sharedVariables.setScore (sharedVariables.getScore () + item.success);
			}
			return true;
		} else {
			return false;
		}
	}

	// Get a random tip. 
	// Used in:
	// - Tip Controller.  
	public string getTip() {
		// Build the URL and make the API call. 
		string url = sharedVariables.getIPPort() + COMMON_PATH + "getTip";
		string response = "";
		WWW www = new WWW(url);
		// Waits until the API responds.
		while (!www.isDone) {}
		// Build the JSON of the response and get the letter. 
		if (www.error == null) {
			response = www.text;
			TipItem item = JsonUtility.FromJson<TipItem> (response);
			return item.tip;
			// If there was some error, we send always an A. 
		} else {
			return "No tips for today!";
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

	public class TipItem
	{
		public string tip;
	}

	public class AlbumElement
	{
		public string title;
		public string author;
		public string text;
		public string type;
		public float rate;
		public double success;
	}

	public class LetterList
	{
		public string[] letters;
	}
}
