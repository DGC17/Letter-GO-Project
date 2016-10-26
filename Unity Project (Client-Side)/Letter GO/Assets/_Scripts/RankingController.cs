using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class RankingController : MonoBehaviour {

	private APIController apiController;
	public GameObject element;
	public GameObject grid;

	// Use this for initialization
	void Start () {
		apiController = GameObject.Find ("APIController").GetComponent<APIController> ();
		string[] userTop = apiController.getTop10();
		if (userTop != null) {
			foreach (string row in userTop) {
				addButton (row);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void addButton(string row) {
		GameObject addElement = Instantiate (element, element.transform.position, element.transform.rotation) as GameObject;
		addElement.GetComponentInChildren<Text> ().text = row;
		addElement.GetComponent<Image> ().material.mainTexture = Texture2D.whiteTexture;
		addElement.transform.SetParent (grid.transform, false);
	}

	public void returnGame() {
		SceneManager.LoadScene (1);
	}
}
