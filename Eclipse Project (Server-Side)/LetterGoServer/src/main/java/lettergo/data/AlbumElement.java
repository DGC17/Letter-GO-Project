package lettergo.data;

import java.io.Serializable;
import java.math.BigDecimal;
import java.util.ArrayList;

/**
 * Class that represents an element inside the album of an user.
 * 
 * @author David Garcia Centelles
 */
public final class AlbumElement implements Serializable {
	
	private static final long serialVersionUID = 1L;

	/**
	 * 100%.
	 */
	private static final float P100 = 100.0f;
	
	/**
	 * Score for completing an Album element. 
	 */
	private static final double S1000 = 1000d;
	
	/**
	 * The title of the Album Element.
	 */
	private String title;
	
	/**
	 * The text of the Album Element. 
	 */
	private String text;
	
	/**
	 * The incomplete text of the Album Element. 
	 */
	private String incompleteText;
	
	/**
	 * The author of the text stored in the Album Element. 
	 */
	private String author;
	
	/**
	 * The type of text stored in the Album Element (Example: Poem).
	 */
	private String type;
	
	/**
	 * The completion rate of the Album Element. 
	 */
	private float completionRate;
	
	/**
	 * The list of all the missing letters in the text. 
	 */
	private ArrayList<MissingLetter> missingLetters;
	
	/** 
	 * The initial number of missing letters.
	 */
	private int missingCount;
	
	/**
	 * Constructor. 
	 * @param title0 Title.
	 * @param text0 Text.
	 * @param incompleteText0 Incomplete Text.
	 * @param author0 Author.
	 * @param type0 Type.
	 */
	public AlbumElement(final String title0,
			final String text0, final String incompleteText0,
			final String author0, final String type0) {
		super();
		this.title = title0;
		this.text = text0;
		this.incompleteText = incompleteText0;
		this.author = author0;
		this.type = type0;
		this.completionRate = 0.0f;
		this.missingLetters = addMissingLetters(this.text, this.incompleteText);
		this.missingCount = this.missingLetters.size();
	}

	/**
	 * Gets the Title.
	 * @return the title.
	 */
	public String getTitle() {
		return title;
	}

	/**
	 * Sets the Title.
	 * @param newtitle the title to set.
	 */
	public void setTitle(final String newtitle) {
		this.title = newtitle;
	}

	/**
	 * Gets the text. 
	 * @return the text
	 */
	public String getText() {
		return text;
	}

	/**
	 * Sets the text. 
	 * @param newtext the text to set.
	 */
	public void setText(final String newtext) {
		this.text = newtext;
	}

	/**
	 * Gets the incomplete text. 
	 * @return the incompleteText.
	 */
	public String getIncompleteText() {
		return incompleteText;
	}

	/**
	 * Sets the incomplete text. 
	 * @param newincompleteText the incompleteText to set.
	 */
	public void setIncompleteText(final String newincompleteText) {
		this.incompleteText = newincompleteText;
	}

	/**
	 * Gets the author of the text. 
	 * @return the author
	 */
	public String getAuthor() {
		return author;
	}

	/**
	 * Sets the author of the text. 
	 * @param newauthor the author to set.
	 */
	public void setAuthor(final String newauthor) {
		this.author = newauthor;
	}

	/**
	 * Gets the type of the text. 
	 * @return the type.
	 */
	public String getType() {
		return type;
	}

	/**
	 * Sets the type of the text. 
	 * @param newtype the type to set.
	 */
	public void setType(final String newtype) {
		this.type = newtype;
	}

	/**
	 * Gets the completion rate of the Album Element. 
	 * @return the completionRate.
	 */
	public float getCompletionRate() {
		return completionRate;
	}

	/**
	 * Sets the completion rate of the Album Element. 
	 * @param newcompletionRate the completionRate to set.
	 */
	public void setCompletionRate(final float newcompletionRate) {
		this.completionRate = newcompletionRate;
	}

	/**
	 * Gets the list of missing letters in the text. 
	 * @return the missingLetters.
	 */
	public ArrayList<MissingLetter> getMissingLetters() {
		return missingLetters;
	}

	/**
	 * Sets the list of missing letters in the text. 
	 * @param newmissingLetters the missingLetters to set.
	 */
	public void setMissingLetters(
			final ArrayList<MissingLetter> newmissingLetters) {
		this.missingLetters = newmissingLetters;
	}
	
	/**
	 * Gets the missing letters count. 
	 * @return the missingCount.
	 */
	public int getMissingCount() {
		return missingCount;
	}

	/**
	 * Sets the missing letters count. 
	 * @param newmissingCount the missingCount to set.
	 */
	public void setMissingCount(final int newmissingCount) {
		this.missingCount = newmissingCount;
	}

	/**
	 * Explores the Incomplete Text to find the missing letter symbol "_". 
	 * Compares this position with the position of the Complete Text to know 
	 * the letter missing and after that adds an element to 
	 * the list of missing letters. 
	 * @param t Complete Text.
	 * @param it Incomplete Text.
	 * @return List of Missing Letters.
	 */
	private ArrayList<MissingLetter> addMissingLetters(
			final String t, final String it) {
		ArrayList<MissingLetter> mls = 
				new ArrayList<MissingLetter>();
		
		for (int i = 0; i < it.length(); i++) {
			if (it.charAt(i) == '_') {
				char l = t.charAt(i);
				boolean uppercase = Character.toString(l)
						.equals(Character.toString(l).toUpperCase());
				MissingLetter ml = new MissingLetter(
						Character.toUpperCase(l), i, uppercase);
				mls.add(ml);
			}
		}
		
		return mls;
	}
	
	/**
	 * Calculates the completion rate of the Album Element.
	 * @return The completion rate calculated. 
	 */
	private float calculateCompletionRate() {
		float total = (float) this.missingCount;
		float partial = (float) this.missingLetters.size();
		float rate = ((total - partial) / total) * P100;
		BigDecimal bd = new BigDecimal(Float.toString(rate));
		bd = bd.setScale(2, BigDecimal.ROUND_HALF_UP);
		rate = bd.floatValue();
		
		return rate;		
	}
	
	/**
	 * Checks if a letter is missing in the Album Element. 
	 * @param letter Letter.
	 * @return True/False.
	 */
	public boolean isLetterMissing(final char letter) {
		boolean found = false;
		int i = 0;
		while (!found && i < this.missingLetters.size()) {
			if (this.missingLetters.get(i).getLetter() == letter) {
				found = true;
			} else {
				i++;
			}
		}
		return found;
	}
	
	/**
	 * Introduces a letter in the incomplete text.
	 * @param letter The letter to introduce. 
	 * @return Score achieved. 
	 */
	public double fillLetterInText(final char letter) {
		boolean found = false;
		int i = 0;
		while (!found && i < this.missingLetters.size()) {
			if (this.missingLetters.get(i).getLetter() == letter) {
				found = true;
			} else {
				i++;
			}
		}
		
		int letterPositionInText = this.missingLetters.get(i).getPosition();
		boolean isLowerCase = !this.missingLetters.get(i).isUppercase();
		this.missingLetters.remove(i);
		
		char letterToAdd = letter;
		if (isLowerCase) {
			letterToAdd = Character.toLowerCase(letterToAdd);
		}

		StringBuilder newIncompleteText = 
				new StringBuilder(this.incompleteText);
		newIncompleteText.setCharAt(letterPositionInText, letterToAdd);
		this.incompleteText = newIncompleteText.toString();

		this.completionRate = calculateCompletionRate();
		
		double score = 0d;
		if (this.completionRate == P100) {
			score = S1000;
		}
		return score;
	}
	
	/**
	 * Inner Class that represents a missing letter inside an album element. 
	 * @author David Garcia Centelle
	 */
	public final class MissingLetter implements Serializable {
		
		private static final long serialVersionUID = 1L;

		/**
		 * Letter missing. 
		 */
		private char letter;
		
		/**
		 * The position of the letter missing. 
		 */
		private int position;
		
		/**
		 * Checks if the letter is uppercase or lowercase. 
		 */
		private boolean isUppercase;
		
		/**
		 * Constructor. 
		 * @param letter0 Missing Letter.
		 * @param position0 Position of the Missing Letter. 
		 * @param isUppercase0 Is Upper Case?. 
		 */
		MissingLetter(final char letter0, final int position0,
				final boolean isUppercase0) {
			super();
			this.letter = letter0;
			this.position = position0;
			this.isUppercase = isUppercase0;
		}

		/**
		 * Gets the missing letter. 
		 * @return the letter.
		 */
		public char getLetter() {
			return letter;
		}
		
		/**
		 * Sets the missing. 
		 * @param newletter the letter to set.
		 */
		public void setLetter(final char newletter) {
			this.letter = newletter;
		}
		
		/**
		 * Gets the position of the missing letter. 
		 * @return the position.
		 */
		public int getPosition() {
			return position;
		}
		
		/**
		 * Sets the position of the missing letter. 
		 * @param newposition the position to set.
		 */
		public void setPosition(final int newposition) {
			this.position = newposition;
		}

		/**
		 * Gets true if it's uppercase, and false if isn't.
		 * @return the isUppercase.
		 */
		public boolean isUppercase() {
			return isUppercase;
		}

		/**
		 * Sets true if it's uppercase, and false if isn't. 
		 * @param newisUppercase the isUppercase to set.
		 */
		public void setUppercase(final boolean newisUppercase) {
			this.isUppercase = newisUppercase;
		}
	}	
}
