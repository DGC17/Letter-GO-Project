using UnityEngine;
using System.Collections;

// TODO: Test Class
// Class to control the "Spin" effect for the Test Cube.
public class Spin : MonoBehaviour {
	
	// Update is called once per frame.
	void Update () {
		
		// Rotate at 90 degrees per second.
		transform.Rotate(Vector3.up * Time.deltaTime*90);

	}
}
