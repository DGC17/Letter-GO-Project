using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageRotation : MonoBehaviour {

	public float rate;
	private float rotation;

	// Use this for initialization
	void Start () {
		rotation = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		rotation += rate;
		if ((rotation == 360.0f) || (rotation == -360.0f))
			rotation = 0.0f;
		transform.rotation = Quaternion.Euler(0, 0, rotation);
	}
}
