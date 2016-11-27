using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TipController : MonoBehaviour {

	// External References.
	private APIController apiController;
	private soundPlayer soundPlayer;

	private Text textTip;

	// Use this for initialization
	void Start () {	
		// Assignations.
		apiController = GameObject.Find ("APIController").GetComponent<APIController> ();
		soundPlayer = GameObject.Find ("soundPlayer").GetComponent<soundPlayer> ();
		textTip = GameObject.Find ("GI.Tip.Text").GetComponent<Text>();

		textTip.text = apiController.getTip();
	}

	public void nextTip() {
		soundPlayer.playSound ("select");
		string newTip = apiController.getTip();
		string oldTip = textTip.text;
		while (oldTip.Contains(newTip)) newTip = apiController.getTip();
		textTip.text = newTip;
	}
}
