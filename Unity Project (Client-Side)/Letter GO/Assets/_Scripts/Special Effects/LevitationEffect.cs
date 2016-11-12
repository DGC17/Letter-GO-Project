using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevitationEffect : MonoBehaviour {

	public float range;
	public float rate;

	private Vector3 actualPosition;
	private Vector3 maxPosition;
	private Vector3 minPosition;
	private bool up;

	// Use this for initialization
	void Start () {
		up = true;
		actualPosition = this.transform.position;
		maxPosition = new Vector3 (actualPosition.x, actualPosition.y + range, actualPosition.y);
		minPosition = new Vector3 (actualPosition.x, actualPosition.y - range, actualPosition.y);
	}
	
	// Update is called once per frame
	void Update () {
		if (up) {
			actualPosition = new Vector3 (actualPosition.x, actualPosition.y + rate, actualPosition.y);
		} else {
			actualPosition = new Vector3 (actualPosition.x, actualPosition.y - rate, actualPosition.y);
		}
		this.GetComponent<Button> ().transform.position = actualPosition;
		if (actualPosition.y == maxPosition.y) up = false;
		if (actualPosition.y == minPosition.y) up = true;
	}
}
