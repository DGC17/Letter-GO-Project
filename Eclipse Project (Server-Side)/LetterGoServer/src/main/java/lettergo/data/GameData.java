package lettergo.data;

import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;

import javax.json.Json;
import javax.json.JsonArray;
import javax.json.JsonObject;
import javax.json.JsonReader;

import lettergo.server.Main;

/**
 * Class that represents the DB for the application.
 * 
 * @author David Garcia Centelles
 *
 */
public final class GameData {

	/**
	 * Path to JSON File. 
	 */
	private static final String JSON_FILE = "tips.json";
	
	/**
	 * Path to JSON File. 
	 */
	private static final String JSON_FILE2 = "globalalbum.json";
	
	/**
	 * Max Score.
	 */
	private static final double MAX_SCORE = 150d;

	/**
	 * Allowed letters inside the game.
	 */
	private static final String[] LETTERS = {	
			"A", "B", "C", "D", "E", "F", "G", 
			"H", "I", "J", "K", "L", "M", "N", 
			"O", "P", "Q", "R", "S", "T", "U", 
			"V", "W", "X", "Y", "Z"};
	
	/**
	 * Weight of every letter.
	 */
	private static final HashMap<String, Integer> WEIGHTS = 
			new HashMap<String, Integer>() {
		private static final long serialVersionUID = 1L;
		{
			put("A", 9); put("B", 2); put("C", 3); put("D", 5);
			put("E", 13); put("F", 3); put("G", 3); put("H", 7);
			put("I", 7); put("J", 1); put("K", 1); put("L", 5);
			put("M", 3); put("N", 7); put("O", 8); put("P", 2);
			put("Q", 1); put("R", 6); put("S", 7); put("T", 10);
			put("U", 3); put("V", 1); put("W", 3); put("X", 1);
			put("Y", 2); put("Z", 1);
		}
	};
	
	/**
	 * List of Users of the application. 
	 */
	private ArrayList<User> users;
	
	/**
	 * List of letters with their respective counts. 
	 */
	private HashMap<String, Double> letterCount;
	
	/**
	 * History.
	 */
	private ArrayList<HistoryElement> history;
	
	/**
	 * List of tips for the user. 
	 */
	private static ArrayList<String> tips;
	
	/**
	 * List of Album Elements. 
	 */
	private static ArrayList<AlbumElement> globalalbum;
	
	/**
	 * Constructor. 
	 */
	public GameData() {	
		initiliazeGameData();
	}
	
	/**
	 * Initializes the GameData.
	 * Necessary when the DB is empty. 
	 */
	private void initiliazeGameData() {
		users = new ArrayList<User>();
		history = new ArrayList<HistoryElement>();
		letterCount = new HashMap<String, Double>();
		for (String letter : LETTERS) {
			letterCount.put(letter, 0d);
		}
		tips = new ArrayList<String>();
		loadTips();
		globalalbum = new ArrayList<AlbumElement>();
		loadGlobalAlbum();
	}
	
	/**
	 * Gets the list of users of the application.
	 * @return The list of users of the application to get.
	 */
	public ArrayList<User> getUsers() {
		return users;
	}
	
	/**
	 * Gets the count of a letter.
 	 * @param letter The letter whose count we want to get.
	 * @return The count of the letter.
	 */
	public double getLetterCount(final String letter) {
		return letterCount.get(letter);
	}
	
	/**
	 * Sets the count of a letter.
	 * @param letter The letter whose count we want to set.
	 * @param count The count of the letter.
	 */
	public void setLetterCount(final String letter, final double count) {
		letterCount.put(letter, count);
	}
	
	/**
	 * @return the history
	 */
	public ArrayList<HistoryElement> getHistory() {
		return history;
	}

	/**
	 * @param historyNew the history to set
	 */
	public void setHistory(final ArrayList<HistoryElement> historyNew) {
		this.history = historyNew;
	}

	/**
	 * @return the tips.
	 */
	public static ArrayList<String> getTips() {
		return tips;
	}
	
	/**
	 * @return the global album.
	 */
	public static ArrayList<AlbumElement> getGlobalAlbum() {
		return globalalbum;
	}
	
	/**
	 * Adds an User to the application.
	 * @param username User to add.
	 * @param password Password of the user.
	 */
	public void addUser(final String username, final String password) {
		User user = new User(username, password);
		users.add(user);
	}
	
	/**
	 * Add History Element.
	 * @param username0 
	 * @param letter0 
	 * @param imagePath0 
	 * @param recognized0 
	 * @param dateRecognition0 
	 * @param locationGPS0 
	 * @param score0 
	 * @param time0 
	 * @param letters0 
	 * @param letterPosition0 
	 * @param imageLetterSize0 
	 */
	public void addHistoryElement(final String username0, final String letter0, 
			final String imagePath0, final String recognized0, 
			final Date dateRecognition0, final String locationGPS0, 
			final double score0, final double time0, final String letters0,
			final String letterPosition0, final int imageLetterSize0) {
		HistoryElement he = new HistoryElement(username0, letter0, 
				imagePath0, recognized0, dateRecognition0, locationGPS0,
				score0, time0, letters0, letterPosition0, imageLetterSize0);
		history.add(he);
	}

	/**
	 * Checks if the letter is valid in the application.
	 * @param letter The letter to check. 
	 * @return True or False. 
	 */
	public boolean isValidLetter(final String letter) {
		boolean found = false;
		int i = 0;
		while (!found && i < LETTERS.length) {
			if (LETTERS[i].contentEquals(letter)) {
				found = true;
			} else {
				i++;
			}
		}
		return found;
	}
	
	/**
	 * Checks if an Username is already registered in the application.
	 * @param username Username to check.
	 * @return True or False.
	 */
	public boolean isUsernameRegistered(final String username) {
		boolean found = false;
		int i = 0;
		while (!found && i < users.size()) { 
			if (users.get(i).getUsername().equals(username)) {
				found = true;
			} else {
				i++;
			}
		}
		return found;
	}
	
	/**
	 * Gets the User Index knowing his username.
	 * @param username Username whose index we want to know.
	 * @return Index of the User inside the list.
	 */
	public int getUserIndexWithName(final String username) {
		int i = 0;
		while (!users.get(i).getUsername().equals(username)) {
			i++;
		}
		return i;
	}
	
	/**
	 * Gets the Album from a User. 
	 * @param username User. 
	 * @return Album. 
	 */
	public ArrayList<AlbumElement> getAlbumOfUser(final String username) {
		int ui = getUserIndexWithName(username);
		return this.users.get(ui).getAlbum();
	}
	
	/**
	 * Gets an Album Element by his title, from the album of a user. 
	 * @param title Title of the Album Element. 
	 * @param username User. 
	 * @return Album Element. 
	 */
	public AlbumElement getAlbumElementFromAlbumOfUser(
			final String title, final String username) {
		int ui = getUserIndexWithName(username);
		int ai = this.users.get(ui).getAlbumElementIndexWithTitle(title);
		return this.users.get(ui).getAlbum().get(ai);
	}
	
	/**
	 * Gets an Album Element by his title, from the global album. 
	 * @param title Title of the Album Element. 
	 * @return Album Element. 
	 */
	public AlbumElement getAlbumElementFromGlobalAlbum(final String title) {
		
		int i = 0;
		while (!globalalbum.get(i).getTitle().equals(title)) {
			i++;
		}
		
		return globalalbum.get(i);
	}
	
	/**
	 * Gets the available letters of an user. 
	 * @param username User.
	 * @return List of available letters. 
	 */
	public ArrayList<String> getLettersOfUser(final String username) {
		int ui = getUserIndexWithName(username);
		return this.users.get(ui).getLetters();
	}
	
	/**
	 * Sets a new letter for the user. 
	 * @param letter Letter. 
	 * @param username User.
	 */
	public void addLetterToUser(final String letter, final String username) {
		int ui = getUserIndexWithName(username);
		this.users.get(ui).getLetters().add(letter);
	}
	
	/**
     * Updates the score of an user.
     * @param username The username whose score we want to update. 
     * @param score Score achieved from the recognition 
     */
    public void updateScore(final String username, final double score) {
    	
    	int i = Main.getGameData().getUserIndexWithName(username);
    	double actualScore = Main.getGameData().getUsers().get(i).getPoints();
    	double newScore = actualScore + score;
    	Main.getGameData().getUsers().get(i).setPoints(newScore);
    }
    
    /**
     * Updates the count of a letter.
     * @param letter The letter whose count we want to update.
     * @return The new count of the letter.
     */
    public double updateLetterCount(final String letter) {
    	double newLetterCount = Main.getGameData().getLetterCount(letter) + 1d;
    	Main.getGameData().setLetterCount(letter, newLetterCount);
    	return newLetterCount;
    }
	
    /** 
     * Tries to fill a letter in an album element of an user. 
     * @param letter Letter. 
     * @param albumElementTitle Title of the Album Element. 
     * @param username User. 
     * @return True (Everything goes fine) / False (That letter isn't missing). 
     */
	public double fillLetterInAlbumElementOfUser(
			final String letter, final String albumElementTitle,
			final String username) {
		int ui = getUserIndexWithName(username);
		return this.users.get(ui).
				fillLetterInAlbumElement(letter, albumElementTitle);
	}
	
    /** 
     * Tries to fill a letter in an album element the global album. 
     * @param letter Letter. 
     * @param albumElementTitle Title of the Album Element. 
     * @param username User. 
     * @return True (Everything goes fine) / False (That letter isn't missing). 
     */
	public double fillLetterInGlobalAlbumElement(
			final String letter, final String albumElementTitle,
			final String username) {
		
		int ui = getUserIndexWithName(username);
		char l = letter.toCharArray()[0];
		int i = 0;
		while (!globalalbum.get(i).getTitle().equals(albumElementTitle)) {
			i++;
		}
		double score = -1d;	
		
		if (globalalbum.get(i).isLetterMissing(l)) {
			score = globalalbum.get(i).fillLetterInText(l);
			score = MAX_SCORE;
			this.users.get(ui).setPoints(
					this.users.get(ui).getPoints() + score);
		}

		int li = this.users.get(ui).getLetterIndex(letter);
		this.users.get(ui).getLetters().remove(li);
		
		return score;
	}

	/**
	 * Gets the array of letters defined in this class.
	 * @return Array of letters. 
	 */
	public static String[] getLetters() {
		return LETTERS;
	}
	
	/**
	 * Gets the weights of all the letters.
	 * @return Dictionary of weights. 
	 */
	public static HashMap<String, Integer> getWeights() {
		return WEIGHTS;
	}
	
	/**
	 * Gets the top of the users with the best scores. 
	 * @param n Number of positions of the top. 
	 * @return The top.
	 */
	public ArrayList<User> getTop(final int n) {
		ArrayList<User> temp = new ArrayList<User>(users);
		ArrayList<User> top = new ArrayList<User>();
		int i = 0;
		int tempSize = temp.size();
		while ((i < n) && (i < tempSize)) {
			double max = temp.get(0).getPoints(); 
			int pos = temp.indexOf(temp.get(0));
			for (User u : temp) {
				if (u.getPoints() > max) {
					max = u.getPoints();
					pos = temp.indexOf(u);
				}
			}
			top.add(temp.get(pos));
			temp.remove(pos);
			i++;
		}
		return top;
	}
	
	/**
	 * Saves the data inside a file. 
	 */
	public void saveDataOnFile() {	
		
		//USER DATA
		try {
			FileOutputStream fos = new FileOutputStream(
					Main.getDbPath() + "/UserData", false);
			ObjectOutputStream oos = new ObjectOutputStream(fos);
			oos.writeObject(users);
			oos.close();
		} catch (FileNotFoundException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		}
		
		//LETTER DATA
		try {
			FileOutputStream fos2 = new FileOutputStream(
					Main.getDbPath() + "/LetterCountData", false);
			ObjectOutputStream oos2 = new ObjectOutputStream(fos2);
			oos2.writeObject(letterCount);
			oos2.close();
		} catch (FileNotFoundException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		}
		
		//HISTORY DATA
		try {
			FileOutputStream fos3 = new FileOutputStream(
					Main.getDbPath() + "/History", false);
			ObjectOutputStream oos3 = new ObjectOutputStream(fos3);
			oos3.writeObject(history);
			oos3.close();
		} catch (FileNotFoundException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		}
	}
	
	/**
	 * Loads the data from a file.
	 * @return True or False, depending if the data existed. 
	 */
	@SuppressWarnings("unchecked")
	public boolean loadDataFromFile() {
		boolean exists = true;
		//USER DATA
		try {
			FileInputStream fis = new FileInputStream(
					Main.getDbPath() + "/UserData");
			ObjectInputStream ois = new ObjectInputStream(fis);
			users = (ArrayList<User>) ois.readObject();
			ois.close();
		} catch (FileNotFoundException e) {
			exists = false;
		} catch (IOException e) {
		} catch (ClassNotFoundException e) {
			
		}
		
		//LETTER DATA
		try {
			FileInputStream fis2 = new FileInputStream(
					Main.getDbPath() + "/LetterCountData");
			ObjectInputStream ois2 = new ObjectInputStream(fis2);
			letterCount = (HashMap<String, Double>) ois2.readObject();
			ois2.close();
		} catch (FileNotFoundException e) {
			exists = false;
		} catch (IOException e) {
		} catch (ClassNotFoundException e) {
			
		}
		
		//HISTORY DATA
		try {
			FileInputStream fis3 = new FileInputStream(
					Main.getDbPath() + "/History");
			ObjectInputStream ois3 = new ObjectInputStream(fis3);
			history = (ArrayList<HistoryElement>) ois3.readObject();
			ois3.close();
		} catch (FileNotFoundException e) {
			exists = false;
		} catch (IOException e) {
		} catch (ClassNotFoundException e) {
			
		}
		
		return exists;
	}
	
	/**
	 * Loads the list of tips from the file "tips.json". 
	 */
	private void loadTips() {		
		try {
			InputStream fis = new FileInputStream(JSON_FILE);
			JsonReader jsonReader = Json.createReader(fis);
			JsonArray array = jsonReader.readArray();
			jsonReader.close();
			fis.close();			
			for (int i = 0; i < array.size(); i++) {				
				tips.add(array.getString(i));	
			}		
		} catch (FileNotFoundException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		}
	}
	
	/**
	 * Loads the list of tips from the file "tips.json". 
	 */
	private void loadGlobalAlbum() {		
		try {
			InputStream fis = new FileInputStream(JSON_FILE2);
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
				GameData.globalalbum.add(new AlbumElement(
						title, text, incompletedtext, author, type));
			}		
		} catch (FileNotFoundException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		}
	}
}
