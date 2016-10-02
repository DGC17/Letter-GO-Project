using UnityEngine;
using System.Collections;

public class TouchTests : MonoBehaviour {

	public GameObject mainCamera;

	GameObject Testcube;

	Vector2 last_touch_position;
	Vector2 touch_position;
	Vector3 cube_position_3;
	Vector2 cube_position;


	void Start () {
		Testcube = GameObject.FindGameObjectWithTag ("Cube");
		cube_position_3 = mainCamera.GetComponent<Camera>().WorldToScreenPoint (Testcube.transform.position);
		cube_position = new Vector2 (cube_position_3.x, cube_position_3.y);
	}

	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0) {
			Debug.Log (cube_position);
			if (Input.GetTouch (0).phase == TouchPhase.Began) {
				last_touch_position = Input.GetTouch (0).position;
			} else {
				touch_position = Input.GetTouch (0).position;
				cube_position = cube_position + (touch_position - last_touch_position);
				Testcube.transform.position = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(new Vector3 (cube_position.x, cube_position.y, cube_position_3.z));
				last_touch_position = touch_position;
			}
		}
	}
}
