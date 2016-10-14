using UnityEngine;
using System.Collections;

// Class that stores shared variables within the different scenes.
public class sharedVariables : MonoBehaviour {

	// Username of the acutal User.
	private string username;

	// Password of the acutal User. 
	private double score;

	// IP and Port of the API, necessary to make all the API calls.  
	private string IPPort;

	// Method called on the awake phase of the GameObject related to this script. 
	void Awake() {
		//We ensure that this script isn't destroyed when we load other scenes.
		DontDestroyOnLoad (this);
	}

	// Use this for initialization
	void Start () {
		username = "";
		score = 0d;
	}

	// Gets the Username.
	public string getUsername() {
		return username;
	}

	// Sets the Username.
	public void setUsername(string newusername) {
		username = newusername;
	}

	// Gets the Score.
	public double getScore() {
		return score;
	}

	// Sets the Score.
	public void setScore(double newscore) {
		score = newscore;
	}

	// Gets the IP and Port.
	public string getIPPort() {
		return IPPort;
	}

	// Sets the IP and Port.
	public void setIPPort(string newIPPort) {
		IPPort = newIPPort;
	}
}
