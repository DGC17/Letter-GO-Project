package lettergo.responses; 

import javax.ws.rs.core.Response.Status;

/**
 * Custom Response. 
 * The user has introduced an invalid value. 
 * 
 * @author David Garcia Centelles
 * 
 */
public class Invalid extends AbstractStatusType {

  /**
   * Constructor. 
   * @param reasonPhrase Message to show in the response. 
   */
  public Invalid(final String reasonPhrase) {
      super(Status.NOT_ACCEPTABLE, reasonPhrase);
  }
}
