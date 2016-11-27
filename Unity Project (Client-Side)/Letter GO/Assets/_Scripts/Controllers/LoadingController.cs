using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingController : MonoBehaviour {

	private Text loadingText;
	private bool loading;

	// Method called on the awake phase of the GameObject related to this script. 
	void Awake() {
		//We ensure that this script isn't destroyed when we load other scenes.
		DontDestroyOnLoad (this);

		if ((FindObjectsOfType (GetType ())).Length > 1)
			Destroy (gameObject);
	}

	// Use this for initialization
	void Start () {
		loadingText = GameObject.Find ("Loading_Text").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));
	}

	public void openScene(int scene) {
		gameObject.SetActive (true);
		StartCoroutine(LoadNewScene(scene));
	}


	IEnumerator LoadNewScene(int scene) {
		// This line waits for 3 seconds before executing the next line in the coroutine.
		yield return new WaitForSeconds(1);

		// Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
		AsyncOperation async = SceneManager.LoadSceneAsync(scene);

		// While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
		while (!async.isDone) {
			yield return null;
		}

		gameObject.SetActive (false);
	}
}
