using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AlbumController : MonoBehaviour {

	private APIController apiController;
	private sharedVariables sharedVariables;
	private soundPlayer soundPlayer;

	public GameObject element;
	public GameObject grid;

	// Use this for initialization
	void Start () {

		apiController = GameObject.Find ("APIController").GetComponent<APIController> ();
		sharedVariables = GameObject.Find ("sharedVariables").GetComponent<sharedVariables> ();
		soundPlayer = GameObject.Find ("soundPlayer").GetComponent<soundPlayer> ();
		apiController.getAlbum();

		foreach (sharedVariables.AlbumElementInList a in sharedVariables.getAlbumElementsInList()) {
			addButton (a.title, a.author, a.rate);
		}
	}

	public void addButton(string title, string author, float rate) {
		GameObject addElement = Instantiate (element, element.transform.position, element.transform.rotation) as GameObject;
		addElement.GetComponent<ElementController> ().setTitle (title);
		addElement.transform.Find ("Title").GetComponent<Text>().text = title.ToUpper ();
		addElement.transform.Find ("Rate").GetComponent<Text> ().text = rate + " %";
		addElement.transform.Find ("Author").GetComponent<Text> ().text = author;
		addElement.transform.SetParent (grid.transform, false);
	}

	public void returnGame() {
		soundPlayer.playSound ("select");
		SceneManager.LoadScene (1);
	}
}
