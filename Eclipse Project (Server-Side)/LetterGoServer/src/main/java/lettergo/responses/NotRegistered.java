package lettergo.responses;

import javax.ws.rs.core.Response.Status;

/**
 * Custom Response. 
 * The value introduced is not registered in the server. 
 * 
 * @author David Garcia Centelles
 * 
 */
public class NotRegistered extends AbstractStatusType {

  /**
   * Constructor. 
   * @param reasonPhrase Message to show in the response. 
   */
  public NotRegistered(final String reasonPhrase) {
      super(Status.NOT_FOUND, reasonPhrase);
  }
}