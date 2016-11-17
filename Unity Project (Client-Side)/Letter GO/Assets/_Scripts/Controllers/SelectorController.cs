using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectorController : MonoBehaviour {

	// Image: Selector
	private Image selector;

	// I take the background just to not touch it. 
	private Image background;

	// Event Control Variables
	private bool maintain;
	private bool begin;

	// Use this for initialization
	void Start () {
		
		// Assignations
		background = GameObject.Find ("GI.Background").GetComponent<Image>();
		selector = GameObject.Find ("GI.Selector").GetComponent<UnityEngine.UI.Image>();

		// Defauls values. 
		maintain = false;
		begin = false;
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 position = new Vector2(0,0);
		if (Input.touchCount == 1) {
			Touch touch = Input.GetTouch (0);
			position = touch.position;
			switch (touch.phase) {
			case TouchPhase.Began:
				maintain = true;
				begin = true;

				break;

			case TouchPhase.Ended:
				maintain = false;
				break;		
			}
		}

		if (begin) {
			begin = false;
		}

		if (maintain) {
			float h = background.rectTransform.rect.height;
			if ((position.y >= h) && (position.y <= (Screen.height - h))) {	
				selector.transform.position = position;
			}
				
		}
	}

}

