package lettergo.server; 

import com.wordnik.swagger.annotations.Api;
import com.wordnik.swagger.annotations.ApiOperation;
import com.wordnik.swagger.annotations.ApiResponse;
import com.wordnik.swagger.annotations.ApiResponses;

import lettergo.data.AlbumElement;
import lettergo.data.GameData;
import lettergo.data.User;
import lettergo.responses.Invalid;
import lettergo.responses.NotRegistered;

import javax.ws.rs.GET;
import javax.ws.rs.POST;
import javax.ws.rs.Path;
import javax.ws.rs.Produces;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;

import java.awt.image.BufferedImage;
import java.io.ByteArrayInputStream;
import java.io.File;
import java.io.IOException;
import java.io.UnsupportedEncodingException;

import javax.json.Json;
import javax.json.JsonArray;
import javax.json.JsonArrayBuilder;
import javax.json.JsonObject;
import javax.imageio.ImageIO;

import java.util.ArrayList;
import java.util.Base64;
import java.util.Random;

import java.net.URLDecoder;

/**
 * Class that define the functions of the API.
 * 
 * @author David Garcia Centelles
 * 
 */
@Path("functions")
@Api(value = "/functions", description = "Functions")
public final class Functions {
	
    /**
     * TOP 10. 
     */
    static final int T10 = 10;
	
	/**
     * Response 200. 
     */
    static final int R200 = 200;
    
    /**
     * Response 400.
     */
    static final int R400 = 400;
    
    /**
     * Response 406.
     */
    static final int R406 = 406;
    
    /**
     * Generate Letter (GET). 
     * Generates a new letter to "capture" for an user, 
     * and sends it to the user. 
     * 
     * @return Response.
     */
    @GET
    @Path("/generateLetter")
    @Produces(MediaType.APPLICATION_JSON)
    @ApiOperation(value = "generates a new letter for the user")
    @ApiResponses(value = {
        @ApiResponse(code = R200, message = "OK")})
    public Response generateLetter() { 
 	
    	//Generates the letter.
    	String[] letters = GameData.getLetters();
    	Random index = new Random();
    	int n = index.nextInt(letters.length);
    	
    	//Builds the JSON with the generated letter.
    	JsonObject response = Json.createObjectBuilder()
	  	           .add("letter", letters[n])
	  	           .build();
	  	
	   return Response.status(R200).entity(response).build();
    }
    
    /**
     * Add User (POST). 
     * Adds a user to the application.
     * 
     * @return Response.
     * @param request Request.
     */
    @POST
    @Path("/addUser")
    @Produces(MediaType.APPLICATION_JSON)
    @ApiOperation(value = "Add User to the DB")
    @ApiResponses(value = {
        @ApiResponse(code = R200, message = "OK"),
        @ApiResponse(code = R400, 
        	message = "This username is already registered")})
    public Response addUser(final String request) { 
 	
    	//Extract the values from the data of the request. 
    	//The format of the data is: "label1=value1&label2=value2&...".
    	String username = request.split("&")[0].split("=")[1];
    	String password = request.split("&")[1].split("=")[1];
    	
    	//If the username used arealdy exists, we send a custom exception.
    	if (Main.getGameData().isUsernameRegistered(username)) {
    		throw new Invalid("This username is already registered").except();
    	}
 	
    	//Adds the new users to the DB.
    	Main.getGameData().addUser(username, password);
    	
    	return Response.status(R200).build();
    }
    
    /**
     * Login User (POST). 
     * Logins the user in the application and returns his score.
     * 
     * @return Response.
     * @param request Request.
     */
    @POST
    @Path("/loginUser")
    @Produces(MediaType.APPLICATION_JSON)
    @ApiOperation(value = "Add User to the DB")
    @ApiResponses(value = {
        @ApiResponse(code = R200, message = "OK"),
        @ApiResponse(code = R400, message = "User or Password are incorrect")})
    public Response loginUser(final String request) { 
    	
    	//Extract the values from the data of the request. 
    	//The format of the data is: "label1=value1&label2=value2&...".
    	String username = request.split("&")[0].split("=")[1];
    	String password = request.split("&")[1].split("=")[1];
    	
    	//If the username is registered in the DB...
    	if (Main.getGameData().isUsernameRegistered(username)) {
    		//We get the password of that user...
    		int i = Main.getGameData().getUserIndexWithName(username);
    		String pass = Main.getGameData().getUsers().get(i).getPassword();
    		//If the password is correct... 
    		if (password.equals(pass)) {
    	    	
    			//Builds the JSON with the score of the user.
    			JsonObject response = Json.createObjectBuilder()
    	  	           .add("score", Main.getGameData().
    	  	        		   getUsers().get(i).getPoints())
    	  	           .build();
    	  	
    	     	return Response.status(R200).entity(response).build();
    	     	
    		} else {
    			//If the username doesn't exist, we send a custom exception. 
    			throw new Invalid("User or Password are incorrect").except();
    		}
    	} else {
    		//If the username doesn't exist, we send a custom exception. 
    		throw new Invalid("User or Password are incorrect").except();
    	}
    }    
    
	/**
     * Send Results (POST). 
     * Send the results of the recognition of an image to the server,
     * updates the score, updates the letter count and saves the image inside
     * the specific directory of the server. 
     * 
     * @return Response.
     * @param request Request.
     */
    @POST
    @Path("/sendResults")
    @Produces(MediaType.APPLICATION_JSON)
    @ApiOperation(value = "Send results from client")
    @ApiResponses(value = {
        @ApiResponse(code = R200, message = "OK"),
        @ApiResponse(code = R400, message = "This isn't a valid letter"),
        @ApiResponse(code = R406, message = "This username doesn't exists")})
    public Response sendResults(final String request) { 

    	//Extract the values from the data of the request. 
    	//The format of the data is: "label1=value1&label2=value2&...".
    	String username = request.split("&")[0].split("=")[1];
    	String letter = request.split("&")[1].split("=")[1];
    	String picture = request.split("&")[2].split("=")[1];
    	String decodedPicture = "";
    	
    	try {
			decodedPicture = URLDecoder.decode(picture, "UTF-8");
		} catch (UnsupportedEncodingException e1) {
			System.out.println("There was an error decoding the image!");
			e1.printStackTrace();
		}
		
    	//If the username doesn't exist we send a custom exception. 
    	if (!Main.getGameData().isUsernameRegistered(username)) {
    		System.out.println("Username not registered!");
    		throw new NotRegistered("This username doesn't exists").except();
    	}
    	
    	//If the letter isn't allowed in the application we 
    	//send a custom exception.
    	if (!Main.getGameData().isValidLetter(letter)) {
    		System.out.println("Invalid Letter!");
    		throw new Invalid("This isn't a valid letter").except();
    	}
    	
    	//Update Letters of User. 
    	Main.getGameData().addLetterToUser(letter, username);
    	
    	//Update User Score. 
    	double score = Main.getGameData().updateScore(username);
    	
    	//Update Letter Count.
    	double letterCount = Main.getGameData().updateLetterCount(letter); 
    	
    	//Save Picture
    	
    	try {
    		byte[] imageByte = Base64.getDecoder().decode(decodedPicture);
			BufferedImage bi = createImageFromBytes(imageByte);
	    	File outputfile = new File(Main.getDbPath() + "/Letters/"
	    			+ letter + "/" + (int) letterCount + ".png");
	    	ImageIO.write(bi, "png", outputfile);
		} catch (IOException e) {
			System.out.println("There was an error saving the image!");
			e.printStackTrace();
		} 	
    	//Builds the JSON with the new score. 
    	JsonObject response = Json.createObjectBuilder()
    	           .add("score", score)
    	           .build();

    	
    	return Response.status(R200).entity(response).build();
    }  
    
    /**
     * Get Top 10 (GET). 
     * Returns a list with 10 users with the best score. 
     * 
     * @return Response.
     */
    @GET
    @Path("/getTop10")
    @Produces(MediaType.APPLICATION_JSON)
    @ApiOperation(value = "returns a list with 10 users with the best score")
    @ApiResponses(value = {
        @ApiResponse(code = R200, message = "OK")})
    public Response getTop10() { 
    	
        JsonArrayBuilder builder = Json.createArrayBuilder();	
        ArrayList<User> top = Main.getGameData().getTop(T10);
        
        for (User u: top) {
            builder.add(Json.createObjectBuilder()
                    .add("username", u.getUsername())
                    .add("score", u.getPoints()));
        }

        JsonArray topFinal = builder.build();
	  	
	   return Response.status(R200).entity(topFinal).build();
    }
    
    /**
     * Get Album (POST). 
     * Gets the Album of an User.
     * 
     * @return Response.
     * @param request Request.
     */
    @POST
    @Path("/getAlbum")
    @Produces(MediaType.APPLICATION_JSON)
    @ApiOperation(value = "get the Album of an User")
    @ApiResponses(value = {
        @ApiResponse(code = R200, message = "OK")})
    public Response getAlbum(final String request) { 
    	
    	//Extract the values from the data of the request. 
    	//The format of the data is: "label1=value1&label2=value2&...".
    	String username = request.split("=")[1];
    	
		JsonArrayBuilder builder = Json.createArrayBuilder();	
        ArrayList<AlbumElement> album = 
        		Main.getGameData().getAlbumOfUser(username);
        
        for (AlbumElement ae: album) {
            builder.add(Json.createObjectBuilder()
                    .add("title", ae.getTitle())
                    .add("author", ae.getAuthor())
                    .add("rate", ae.getCompletionRate()));
        }

        JsonArray albumFinal = builder.build();
	  	
	   return Response.status(R200).entity(albumFinal).build();
    }
    
    /**
     * Get Album Element (POST). 
     * Gets an Album Element from an Album of an User.
     * 
     * @return Response.
     * @param request Request.
     */
    @POST
    @Path("/getAlbumElement")
    @Produces(MediaType.APPLICATION_JSON)
    @ApiOperation(value = "get an Album Element of an Album of an User")
    @ApiResponses(value = {
        @ApiResponse(code = R200, message = "OK")})
    public Response getAlbumElement(final String request) { 
    	
    	//Extract the values from the data of the request. 
    	//The format of the data is: "label1=value1&label2=value2&...".
    	String username = request.split("&")[0].split("=")[1];
    	String title = request.split("&")[1].split("=")[1];

    	AlbumElement ae = Main.getGameData().
    			getAlbumElementFromAlbumOfUser(title, username);
    	
    	JsonObject response = Json.createObjectBuilder()
	  	           .add("title", ae.getTitle())
	  	           .add("author", ae.getAuthor())
	  	           .add("text", ae.getIncompleteText())
	  	           .add("type", ae.getType())
	  	           .add("rate", ae.getCompletionRate())
	  	           .build();
	  	
	   return Response.status(R200).entity(response).build();
    }
    
    /**
     * Get Letters (POST). 
     * Gets the letters of an user.
     * 
     * @return Response.
     * @param request Request.
     */
    @POST
    @Path("/getLetters")
    @Produces(MediaType.APPLICATION_JSON)
    @ApiOperation(value = "get the Letters of an User")
    @ApiResponses(value = {
        @ApiResponse(code = R200, message = "OK")})
    public Response getLetters(final String request) { 
    	
    	//Extract the values from the data of the request. 
    	//The format of the data is: "label1=value1&label2=value2&...".
    	String username = request.split("=")[1];
    	
		JsonArrayBuilder builder = Json.createArrayBuilder();	
        ArrayList<String> letters = 
        		Main.getGameData().getLettersOfUser(username);
        
        for (String l: letters) {
            builder.add(l);
        }
        
        JsonObject response = 
        		Json.createObjectBuilder().add("letters", builder.build())
        			.build();
	  	
	   return Response.status(R200).entity(response).build();
    }
    
    /**
     * Fill Letter Album Element (POST). 
     * Tries to fill a letter in an album element.
     * 
     * @return Response.
     * @param request Request.
     */
    @POST
    @Path("/fillLetterAlbumElement")
    @Produces(MediaType.APPLICATION_JSON)
    @ApiOperation(value = "tries to fill a letter in an album element")
    @ApiResponses(value = {
        @ApiResponse(code = R200, message = "OK"),
        @ApiResponse(code = R400, 
        	message = "This Letter isn't missing in the Album Element")})
    public Response fillLetterAlbumElement(final String request) { 
    	
    	//Extract the values from the data of the request. 
    	//The format of the data is: "label1=value1&label2=value2&...".
    	String letter = request.split("&")[0].split("=")[1];
    	String title = request.split("&")[1].split("=")[1];
    	String username = request.split("&")[1].split("=")[1];
    	
    	boolean success = Main.getGameData().
    			fillLetterInAlbumElementOfUser(letter, title, username);
    	
    	if (!success) {
    		throw new Invalid("This Letter isn't missing in the Album Element")
    			.except();
    	} else {
        	AlbumElement ae = Main.getGameData().
        			getAlbumElementFromAlbumOfUser(title, username);
        	JsonObject response = Json.createObjectBuilder()
 	  	           .add("title", ae.getTitle())
 	  	           .add("author", ae.getAuthor())
 	  	           .add("text", ae.getIncompleteText())
 	  	           .add("type", ae.getType())
 	  	           .add("rate", ae.getCompletionRate())
 	  	           .build();
     	   return Response.status(R200).entity(response).build();
    	}
    }
    
    /**
     * Transform the picture from an array of bytes to a Buffered Image.
     * @param imageData Picture in an array of bytes.
     * @return Picture in Buffered Image.
     */
    private BufferedImage createImageFromBytes(final byte[] imageData) {
        ByteArrayInputStream bais = new ByteArrayInputStream(imageData);
        try {
            return ImageIO.read(bais);
        } catch (IOException e) {
            throw new RuntimeException(e);
        }
    }
}
