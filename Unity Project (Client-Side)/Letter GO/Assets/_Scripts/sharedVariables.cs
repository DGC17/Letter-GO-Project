using UnityEngine;
using System.Collections;

public class sharedVariables : MonoBehaviour {

	private string username;
	private double score;
	private string IPPort;

	void Awake() {
		DontDestroyOnLoad (this);
	}

	// Use this for initialization
	void Start () {
		username = "";
		score = 0d;
	}

	public string getUsername() {
		return username;
	}

	public void setUsername(string newusername) {
		username = newusername;
	}

	public double getScore() {
		return score;
	}

	public void setScore(double newscore) {
		score = newscore;
	}

	public string getIPPort() {
		return IPPort;
	}

	public void setIPPort(string newIPPort) {
		IPPort = newIPPort;
	}
}
