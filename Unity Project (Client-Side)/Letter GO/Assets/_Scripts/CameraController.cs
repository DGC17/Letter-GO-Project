using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class CameraController : MonoBehaviour {

	private TimerController timerController;

	public GameObject mainCamera;
	public GameObject ARCamera;

	private GameObject enableCameraButton;
	private GameObject takeScreenShotButton;
	private GameObject cross;

	private GameObject interfaceMain;
	private GameObject generalInterface;
	private GameObject imageInterface;

	private string file_name;

	private bool ARCameraActive;
	private bool takescreenshot;
	private bool changeInterface;

	private Image image;

	void Start () {

		timerController = GameObject.FindGameObjectWithTag ("TimerController").GetComponent<TimerController>();

		enableCameraButton = GameObject.FindGameObjectWithTag ("EnableCamera");
		takeScreenShotButton = GameObject.FindGameObjectWithTag ("TakeScreenShot");
		cross = GameObject.FindGameObjectWithTag ("Cross");

		interfaceMain = GameObject.FindGameObjectWithTag ("Interface");
		imageInterface = GameObject.FindGameObjectWithTag ("ImageInterface");
		generalInterface = GameObject.FindGameObjectWithTag ("GeneralInterface");

		image = GameObject.FindGameObjectWithTag ("Image").GetComponent<Image>();

		file_name = "screen.png"; 

		ARCamera.SetActive (false);
		cross.SetActive (false);
		takeScreenShotButton.SetActive (false);

		imageInterface.SetActive (false);

		ARCameraActive = false;
		takescreenshot = false;
		changeInterface = false;
	}

	void Update () {

		if (changeInterface) {

			byte[] bytes = File.ReadAllBytes (Application.persistentDataPath + "/" + file_name);
			Texture2D tex = new Texture2D (Screen.width, Screen.height);
			tex.LoadImage (bytes);

			image.material.mainTexture = tex;
			//image.GetComponent<RectTransform> ().sizeDelta = new Vector2 (Screen.width, Screen.height);

			//No se porque tengo que incluir esto, sino no funciona... 
			interfaceMain.SetActive (true);

			timerController.interruptTimer ();

			generalInterface.SetActive (false);

			mainCamera.SetActive (true);
			ARCamera.SetActive (false);

			changeInterface = false;
		}

		if (takescreenshot) {
			Application.CaptureScreenshot (file_name);
			StartCoroutine ("waitUntilWrite");
			imageInterface.SetActive (true);
		}

	}

	private IEnumerator waitUntilWrite () {
		yield return null;
		changeInterface = true;
		takescreenshot = false;
	}

	public void EnableDisableCamera() {
		if (ARCameraActive) {
			mainCamera.SetActive (true);
			ARCamera.SetActive (false);
			cross.SetActive (false);
			takeScreenShotButton.SetActive (false);
		} else {
			ARCamera.SetActive (true);
			mainCamera.SetActive (false);
			cross.SetActive (true);
			takeScreenShotButton.SetActive (true);
		}

		ARCameraActive = !ARCameraActive;
	}

	public void Take_Screenshot() {
		interfaceMain.SetActive (false);
		takescreenshot = true;
	}
}
