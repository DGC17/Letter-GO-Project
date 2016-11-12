using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ElementController : MonoBehaviour {

	private APIController apiController;
	private soundPlayer soundPlayer;

	private string title;
	private string author;
	private float rate;

	// Use this for initialization
	void Start () {
		apiController = GameObject.Find ("APIController").GetComponent<APIController> ();
		soundPlayer = GameObject.Find ("soundPlayer").GetComponent<soundPlayer> ();
	}

	public string getTitle() {
		return title;
	}
	public void setTitle(string newtitle) {
		title = newtitle;
	}

	public void openElement() {
		soundPlayer.playSound ("select");
		apiController.getAlbumElement (title);
		SceneManager.LoadScene (4);
	}
}
