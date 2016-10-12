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
		sharedVariables.setScore (100d);
		return true;
	}

	public bool register(string username, string password) { 
		return true;
	}

	public string generateLetter() {
		string url = sharedVariables.getIPPort() + COMMON_PATH + "generateLetter";
		string response = "{'letter':'A'}";
		WWW www = new WWW(url);
		while (!www.isDone) {}
		if (www.error == null) {
			response = www.text;
		}
		LetterItem item = JsonUtility.FromJson<LetterItem> (response);
		return item.letter;
	}

	public double sendResults(string letter, string image) {
		return 100d;
	}

	//AUXILIAR CLASSES

	public class LetterItem
	{
		public string letter;
	}
}
