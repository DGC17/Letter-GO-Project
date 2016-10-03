using UnityEngine;
using System.Collections;

// TODO: Test Class.
// Class to control the user "touch" interaction. 
public class TouchTests : MonoBehaviour {

	// External References.
	public GameObject mainCamera;

	// Test Cube.
	GameObject Testcube;

	// Vectors to control the Touch and Cube positions. 
	Vector2 last_touch_position;
	Vector2 touch_position;
	Vector3 cube_position_3;
	Vector2 cube_position;

	// Functions executed on the intilization of the application. 
	void Start () {

		// Assignations. 
		Testcube = GameObject.FindGameObjectWithTag ("Cube");
		cube_position_3 = mainCamera.GetComponent<Camera>().WorldToScreenPoint (Testcube.transform.position);
		cube_position = new Vector2 (cube_position_3.x, cube_position_3.y);
	}

	// Update is called once per frame
	void Update () {

		//If there's a touch detected on the screen. 
		if (Input.touchCount > 0) {

			//If the touch is beggining, we assign this touch postion as the last touch position. 
			if (Input.GetTouch (0).phase == TouchPhase.Began) {
				last_touch_position = Input.GetTouch (0).position;
			//Else, we take the touch position and redifine the Test Cube position depeding in the difference between the actual and the previous touch positions.
			} else {
				touch_position = Input.GetTouch (0).position;
				cube_position = cube_position + (touch_position - last_touch_position);
				Testcube.transform.position = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(new Vector3 (cube_position.x, cube_position.y, cube_position_3.z));
				last_touch_position = touch_position;
			}
		}
	}
}
