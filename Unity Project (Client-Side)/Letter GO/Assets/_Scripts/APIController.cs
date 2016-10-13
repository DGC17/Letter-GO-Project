using UnityEngine;
using System.Collections;
using System.IO;

public class APIController : MonoBehaviour {

	private static readonly string COMMON_PATH = "/lettergo/functions/";

	private sharedVariables sharedVariables;

	void Awake() {
		DontDestroyOnLoad (this);
	}

	// Use this for initialization
	void Start () {
		sharedVariables = GameObject.Find ("sharedVariables").GetComponent<sharedVariables> ();
	}

	public bool login(string username, string password) {
		string url = sharedVariables.getIPPort() + COMMON_PATH + "loginUser";
		WWWForm form = new WWWForm ();
		form.AddField ("username", username);
		form.AddField ("password", password);
		WWW www = new WWW (url, form);
		while (!www.isDone) {}
		if (www.error == null) {
			ScoreItem item = JsonUtility.FromJson<ScoreItem> (www.text);
			sharedVariables.setScore (item.score);
			return true;
		} else {
			return false;
		}
	}

	public bool register(string username, string password) { 
		string url = sharedVariables.getIPPort() + COMMON_PATH + "addUser";
		WWWForm form = new WWWForm ();
		form.AddField ("username", username);
		form.AddField ("password", password);
		WWW www = new WWW (url, form);
		while (!www.isDone) {}
		return (www.error == null);
	}

	public string generateLetter() {
		string url = sharedVariables.getIPPort() + COMMON_PATH + "generateLetter";
		string response = "";
		WWW www = new WWW(url);
		while (!www.isDone) {}
		if (www.error == null) {
			response = www.text;
		}
		LetterItem item = JsonUtility.FromJson<LetterItem> (response);
		return item.letter;
	}

	public double sendResults(string letter, string image) {
		string url = sharedVariables.getIPPort() + COMMON_PATH + "sendResults";
		WWWForm form = new WWWForm ();
		form.AddField ("username", sharedVariables.getUsername());
		form.AddField ("letter", letter);
		form.AddField ("image", image);
		WWW www = new WWW (url, form);
		while (!www.isDone) {}
		if (www.error == null) {
			ScoreItem item = JsonUtility.FromJson<ScoreItem> (www.text);
			return item.score;
		} else {
			return 100d;
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
