package lettergo.data;

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
}
