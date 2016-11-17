using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class RankingController : MonoBehaviour {

	private APIController apiController;
	private soundPlayer soundPlayer;
	public GameObject element;
	public GameObject grid;

	// Use this for initialization
	void Start () {
		apiController = GameObject.Find ("APIController").GetComponent<APIController> ();
		soundPlayer = GameObject.Find ("soundPlayer").GetComponent<soundPlayer> ();
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
		row = row.Trim ();
		string[] Partnumber = row.Split (']');
		char number = Partnumber [0] [1];
		string username = Partnumber [1].Split (':') [0];
		string points = Partnumber [1].Split (':') [1];
		addElement.transform.Find ("Number").GetComponent<Text>().text = number.ToString();
		addElement.transform.Find ("UserName").GetComponent<Text>().text = username.ToUpper();
		addElement.transform.Find ("Points").GetComponent<Text> ().text = points;
		addElement.transform.SetParent (grid.transform, false);
	}

	public void returnGame() {
		soundPlayer.playSound ("select");
		SceneManager.LoadScene (1);
	}
}
