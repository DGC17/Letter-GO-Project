package lettergo.data;

import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStream;
import java.io.Serializable;
import java.util.ArrayList;

import javax.json.Json;
import javax.json.JsonArray;
import javax.json.JsonObject;
import javax.json.JsonReader;

/**
 * Class that represents an user of the application.
 * 
 * @author David Garcia Centelles
 */
public final class User  implements Serializable {

	private static final long serialVersionUID = 1L;
	
	/**
	 * Path to JSON File. 
	 */
	private static final String JSON_FILE = "album.json";
	
	/**
	 * Username of the user.
	 */
	private String username;
	
	/**
	 * Password of the user.
	 */
	private String password;
	
	/**
	 * Points (Score) of the user.
	 */
	private double points;
	
	/**
	 * List of Album Elements. 
	 */
	private ArrayList<AlbumElement> album;
	
	/**
	 * List of letters captured by the user. 
	 */
	private ArrayList<String> letters;
	
	/**
	 * Constructor.
	 * @param username0 Username.
	 * @param password0 Password.
	 */
	public User(final String username0, final String password0) {
		super();
		this.username = username0;
		this.password = password0;
		this.points = 0;
		this.album = new ArrayList<AlbumElement>();
		this.letters = new ArrayList<String>();
		
		loadAlbum();
	}

	/**
	 * Gets the Username.
	 * @return The Username to get.
	 */
	public String getUsername() {
		return username;
	}
	
	/**
	 * Sets the Username.
	 * @param usernameNew The Username to set.
	 */
	public void setUsername(final String usernameNew) {
		this.username = usernameNew;
	}
	
	/**
	 * Gets the Password.
	 * @return The password to get.
	 */
	public String getPassword() {
		return password;
	}
	
	/**
	 * Sets the Password.
	 * @param passwordNew The Password to set.
	 */
	public void setPassword(final String passwordNew) {
		this.password = passwordNew;
	}
	
	/**
	 * Gets the Points (Score).
	 * @return The Points (Score) to get.
	 */
	public double getPoints() {
		return points;
	}
	
	/**
	 * Sets the Points (Score).
	 * @param pointsNew The Points (Score) to set. 
	 */
	public void setPoints(final double pointsNew) {
		this.points = pointsNew;
	}

	/**
	 * Gets the Album. 
	 * @return the album.
	 */
	public ArrayList<AlbumElement> getAlbum() {
		return album;
	}

	/**
	 * Sets the Album. 
	 * @param newalbum the album to set.
	 */
	public void setAlbum(final ArrayList<AlbumElement> newalbum) {
		this.album = newalbum;
	}
	
	/**
	 * Gets the Letters. 
	 * @return the letters
	 */
	public ArrayList<String> getLetters() {
		return letters;
	}

	/**
	 * Sets the Letters. 
	 * @param letters0 the letters to set
	 */
	public void setLetters(final ArrayList<String> letters0) {
		this.letters = letters0;
	}

	/**
	 * Finds a Letter index from the available letters of a user. 
	 * @param letter Letter to find. 
	 * @return Index.
	 */
	public int getLetterIndex(final String letter) {
		boolean found = false;
		int i = 0;
		while (!found && i < this.letters.size()) {
			if (this.letters.get(i).equals(letter)) {
				found = true;
			} else {
				i++;
			}
		}
		return i;
	}
	
	/**
	 * Finds an Album Element by its title.
	 * @param title Title of the album. 
	 * @return Index. 
	 */
	public int getAlbumElementIndexWithTitle(final String title) {
		boolean found = false;
		int i = 0;
		while (!found && i < this.album.size()) {
			if (this.album.get(i).getTitle().equals(title)) {
				found = true;
			} else {
				i++;
			}
		}
		return i;
	}
	
	/**
	 * Tries to fill a letter inside an Album Element. 
	 * If the letter isn't missing it informs the user about that. 
	 * @param letter Letter. 
	 * @param albumElementTitle Title of the Album. 
	 * @return True (Everything goes fine) / False (Letter isn't missing). 
	 */
	public double fillLetterInAlbumElement(
			final String letter, final String albumElementTitle) {
		char l = letter.toCharArray()[0];
		int ai = getAlbumElementIndexWithTitle(albumElementTitle);
		double score = -1d;
		
		if (this.album.get(ai).isLetterMissing(l)) {
			score = this.album.get(ai).fillLetterInText(l);
			this.points = this.points + score;
		}
		
		int li = getLetterIndex(letter);
		this.letters.remove(li);
		
		return score;
	}
	
	/**
	 * Loads the album from the file "album.json". 
	 */
	private void loadAlbum() {		
		try {
			InputStream fis = new FileInputStream(JSON_FILE);
			JsonReader jsonReader = Json.createReader(fis);
			JsonArray array = jsonReader.readArray();
			jsonReader.close();
			fis.close();			
			for (int i = 0; i < array.size(); i++) {
				JsonObject obj = array.getJsonObject(i);
				String title = obj.getString("title");
				String author = obj.getString("author");
				String type = obj.getString("type");
				String text = obj.getString("text");
				String incompletedtext = obj.getString("incompletedtext");
				this.album.add(new AlbumElement(
						title, text, incompletedtext, author, type));
			}		
		} catch (FileNotFoundException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		}
	}
}
