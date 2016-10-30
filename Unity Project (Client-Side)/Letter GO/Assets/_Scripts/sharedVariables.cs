using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Class that stores shared variables within the different scenes.
public class sharedVariables : MonoBehaviour {

	// Username of the acutal User.
	private string username;

	// Password of the acutal User. 
	private double score;

	// IP and Port of the API, necessary to make all the API calls.  
	private string IPPort;

	// List of Album Elements in the list in the Album Screen. 
	private List<AlbumElementInList> albumElementsInList = new List<AlbumElementInList>();

	private AlbumElementSelected albumElementSelected;

	// Method called on the awake phase of the GameObject related to this script. 
	void Awake() {
		//We ensure that this script isn't destroyed when we load other scenes.
		DontDestroyOnLoad (this);
	}

	// Use this for initialization
	void Start () {
		username = "";
		score = 0d;
	}

	// Gets the Username.
	public string getUsername() {
		return username;
	}

	// Sets the Username.
	public void setUsername(string newusername) {
		username = newusername;
	}

	// Gets the Score.
	public double getScore() {
		return score;
	}

	// Sets the Score.
	public void setScore(double newscore) {
		score = newscore;
	}

	// Gets the IP and Port.
	public string getIPPort() {
		return IPPort;
	}

	// Sets the IP and Port.
	public void setIPPort(string newIPPort) {
		IPPort = newIPPort;
	}

	//Gets the AlbumElementsInList
	public List<AlbumElementInList> getAlbumElementsInList() {
		return albumElementsInList;
	}

	//Sets the AlbumElementsInList
	public void setAlbumElementsInList(List<AlbumElementInList> a) {
		albumElementsInList = a;
	}

	//Gets the Album Element Selected
	public AlbumElementSelected getAlbumElementSelected() {
		return albumElementSelected;
	}

	//Sets the Album Element Selected
	public void setAlbumElementSelected(AlbumElementSelected a) {
		albumElementSelected = a;
	}

	//Add an elements to the AlbumElementsInList
	public void addElementInAlbumElementsInList(string title, string author, float rate) {
		albumElementsInList.Add(new AlbumElementInList(title, author, rate));
	}

	//Remove all the elements of the AlbumElementsInList
	public void removeElementsInAlbumElementsInList() {
		albumElementsInList.Clear ();
	}

	public class AlbumElementInList {
		public string title;
		public string author;
		public float rate;

		public AlbumElementInList(string title0, string author0, float rate0) {
			title = title0;
			author = author0;
			rate = rate0;
		}
	}

	public class AlbumElementSelected {
		public string title;
		public string author;
		public string text;
		public string type;
		public float rate;

		public AlbumElementSelected(
			string title0, string author0, string text0, string type0, float rate0) {
			title = title0;
			author = author0;
			text = text0;
			type = type0;
			rate = rate0;
		}
	}
}
