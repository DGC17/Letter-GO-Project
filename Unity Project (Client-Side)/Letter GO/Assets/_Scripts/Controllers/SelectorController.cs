using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectorController : MonoBehaviour {

	private sharedVariables sharedVariables;

	// Image: Selector
	private Image selector;

	// I take the background just to not touch it. 
	private Image background;

	private int maxSize = 500;
	private int minSize = 100;

	// Use this for initialization
	void Start () {
		
		// Assignations
		background = GameObject.Find ("GI.Background").GetComponent<Image>();
		selector = GameObject.Find ("GI.Selector").GetComponent<UnityEngine.UI.Image>();
		sharedVariables = GameObject.Find ("sharedVariables").GetComponent<sharedVariables> ();

		selector.rectTransform.sizeDelta = new Vector2 (300, 300);
	}
	
	// Update is called once per frame
	void Update () {
		if (!sharedVariables.isScanning ()) {
			if (Input.touchCount == 1) {
				Touch touch = Input.GetTouch (0);
				Vector2 position = touch.position;
				float h = background.rectTransform.rect.height;
				if (		(position.y >= (h + selector.rectTransform.sizeDelta.x/4)) 
						&& 	(position.y <= (Screen.height - h - selector.rectTransform.sizeDelta.x/4)) 
						&& 	(position.x >= (selector.rectTransform.sizeDelta.x/3)) 
						&& 	(position.x <= (Screen.width - (selector.rectTransform.sizeDelta.x/3)))) {	
					selector.transform.position = position;
				}

			}

			if (Input.touchCount == 2) {
				Touch touchZero = Input.GetTouch(0);
				Touch touchOne = Input.GetTouch(1);
				Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
				Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
				float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
				float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
				float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

				int newsize = (int)(selector.rectTransform.sizeDelta.x - deltaMagnitudeDiff);

				//FIX to have only multiples of 2. 
				if (newsize % 2 != 0) newsize--;

				if (newsize > maxSize)	newsize = maxSize;
				if (newsize < minSize)	newsize = minSize;
				selector.rectTransform.sizeDelta = new Vector2 (newsize, newsize);
			}
		}
	}

}

