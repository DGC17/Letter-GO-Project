using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AlbumController : MonoBehaviour {

	private APIController apiController;
	private sharedVariables sharedVariables;
	private soundPlayer soundPlayer;

	public GameObject element;
	public GameObject grid;

	public Button Individual;
	public Button Global;

	// Use this for initialization
	void Start () {

		apiController = GameObject.Find ("APIController").GetComponent<APIController> ();
		sharedVariables = GameObject.Find ("sharedVariables").GetComponent<sharedVariables> ();
		soundPlayer = GameObject.Find ("soundPlayer").GetComponent<soundPlayer> ();

		Individual = GameObject.Find ("Individual").GetComponent<Button> ();
		Global = GameObject.Find ("Global").GetComponent<Button> ();

		Individual.interactable = sharedVariables.isShowGlobalAlbum ();
		Global.interactable = !sharedVariables.isShowGlobalAlbum ();

		refreshAlbum (true);
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
		sharedVariables.openScene (1);
	}

	private void refreshAlbum(bool start) {

		if (!start) {
			int child = 0;
			foreach (sharedVariables.AlbumElementInList a in sharedVariables.getAlbumElementsInList()) {
				Destroy (grid.transform.GetChild (child).gameObject);
				child++;
			}
		}

		apiController.getAlbum();

		foreach (sharedVariables.AlbumElementInList a in sharedVariables.getAlbumElementsInList()) {
			addButton (a.title, a.author, a.rate);
		}
	}

	public void switchView () {
		sharedVariables.setShowGlobalAlbum (!sharedVariables.isShowGlobalAlbum ());
		Individual.interactable = sharedVariables.isShowGlobalAlbum ();
		Global.interactable = !sharedVariables.isShowGlobalAlbum ();

		refreshAlbum (false);
	}
}
