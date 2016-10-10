package LetterGoProject.LetterGoData;

import java.io.Serializable;

/**
 * Class that represents an user of the application.
 * 
 * @author David Garcia Centelles
 */
public final class User  implements Serializable {

	private static final long serialVersionUID = 1L;
	
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
	 * Constructor.
	 * @param username0 Username.
	 * @param password0 Password.
	 */
	public User(final String username0, final String password0) {
		super();
		this.username = username0;
		this.password = password0;
		this.points = 0;
	}
	
	/**
	 * Getter.
	 * @return Username.
	 */
	public String getUsername() {
		return username;
	}
	
	/**
	 * Setter.
	 * @param usernameNew New User Name.
	 */
	public void setUsername(final String usernameNew) {
		this.username = usernameNew;
	}
	
	/**
	 * Getter.
	 * @return Password.
	 */
	public String getPassword() {
		return password;
	}
	
	/**
	 * Setter.
	 * @param passwordNew New Password.
	 */
	public void setPassword(final String passwordNew) {
		this.password = passwordNew;
	}
	
	/**
	 * Getter.
	 * @return Points (Score).
	 */
	public double getPoints() {
		return points;
	}
	
	/**
	 * Setter.
	 * @param pointsNew New Points (New Score).
	 */
	public void setPoints(final double pointsNew) {
		this.points = pointsNew;
	}
}
