using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DetailsController : MonoBehaviour {

	private soundPlayer soundPlayer;

	// Use this for initialization
	void Start () {
		soundPlayer = GameObject.Find ("soundPlayer").GetComponent<soundPlayer> ();
	}

	public void returnConfiguration() {
		soundPlayer.playSound ("select");
		SceneManager.LoadScene (5);
	}
}
