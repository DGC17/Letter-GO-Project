package lettergo.data;

import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.util.ArrayList;
import java.util.HashMap;

import lettergo.server.Main;

/**
 * Class that represents the DB for the application.
 * 
 * @author David Garcia Centelles
 *
 */
public final class GameData {

	/**
	 * Allowed letters inside the game.
	 */
	private static final String[] LETTERS = {	
			"A", "B", "C", "D", "E", "F", "G", 
			"H", "I", "J", "K", "L", "M", "N", 
			"O", "P", "Q", "R", "S", "T", "U", 
			"V", "W", "X", "Y", "Z"};
	
	/**
	 * List of Users of the application. 
	 */
	private ArrayList<User> users;
	
	/**
	 * List of letters with their respective counts. 
	 */
	private HashMap<String, Double> letterCount;
	
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
		letterCount = new HashMap<String, Double>();
		for (String letter : LETTERS) {
			letterCount.put(letter, 0d);
		}
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
	 * Adds an User to the application.
	 * @param username User to add.
	 * @param password Password of the user.
	 */
	public void addUser(final String username, final String password) {
		User user = new User(username, password);
		users.add(user);
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
		
		return exists;
	}

	/**
	 * Gets the array of letters defined in this class.
	 * @return Array of letters. 
	 */
	public static String[] getLetters() {
		return LETTERS;
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
}