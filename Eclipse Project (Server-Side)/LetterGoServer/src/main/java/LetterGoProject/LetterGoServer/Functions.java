package LetterGoProject.LetterGoServer;

import com.wordnik.swagger.annotations.Api;
import com.wordnik.swagger.annotations.ApiOperation;
import com.wordnik.swagger.annotations.ApiResponse;
import com.wordnik.swagger.annotations.ApiResponses;

import javax.ws.rs.GET;
import javax.ws.rs.Path;
import javax.ws.rs.Produces;
import javax.ws.rs.core.MediaType;

/**
 * Class that define the functions of the API.
 * 
 * @author David Garcia Centelles
 * 
 */
@Path("functions")
@Api(value = "/functions", description = "Functions")
public class Functions {
	
	
    /**
     * Response 200. 
     */
    static final int R200 = 200;
	
	/**
     * Hello World Function. 
     * @return Hello World String.
     */
    @GET
    @Path("/helloWorld")
    @Produces(MediaType.TEXT_PLAIN)
    @ApiOperation(value = "Hello World function")
    @ApiResponses(value = {
        @ApiResponse(code = R200, message = "OK") })
    public String helloWorld() { 
        return "Hello World";
    }   
}
