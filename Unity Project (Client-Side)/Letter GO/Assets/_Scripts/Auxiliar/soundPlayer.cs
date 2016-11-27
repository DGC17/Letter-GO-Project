using UnityEngine;
using System.Collections;

public class soundPlayer : MonoBehaviour {

	public AudioClip selectSound;
	private AudioSource audioSource;

	// Method called on the awake phase of the GameObject related to this script. 
	void Awake() {
		//We ensure that this script isn't destroyed when we load other scenes.
		DontDestroyOnLoad (this);

		if ((FindObjectsOfType (GetType ())).Length > 1)
			Destroy (gameObject);
	}

	void Start() {

		audioSource = this.GetComponent<AudioSource> ();
		audioSource.Stop ();
		if (!audioSource.isPlaying) audioSource.Play ();

	}

	public bool switchBackgroundMusic(bool isSwitch) {
		if (audioSource.isPlaying) { 
			if (isSwitch) {
				audioSource.Stop ();
				return false;
			} else {
				return true;
			}
		} else {
			if (isSwitch) {
				audioSource.Play ();
				return true;
			} else {
				return false;
			}
		}

	}

	public void playSound(string sound) {
		if (sound.Equals("select")) audioSource.PlayOneShot(selectSound);
	}
}
