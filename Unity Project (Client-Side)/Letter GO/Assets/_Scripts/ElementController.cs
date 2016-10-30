using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ElementController : MonoBehaviour {

	private APIController apiController;

	private string title;
	private string author;
	private float rate;

	// Use this for initialization
	void Start () {
		apiController = GameObject.Find ("APIController").GetComponent<APIController> ();
	}

	public string getTitle() {
		return title;
	}
	public void setTitle(string newtitle) {
		title = newtitle;
	}

	public void openElement() {
		apiController.getAlbumElement (title);
		SceneManager.LoadScene (4);
	}
}
