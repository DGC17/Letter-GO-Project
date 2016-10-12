package LetterGoProject.LetterGoServer;

import java.io.File;
import java.io.IOException;
import java.net.URI;
import java.util.HashMap;
import java.util.Map;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.ws.rs.ext.ContextResolver;

import org.eclipse.persistence.tools.file.FileUtil;
import org.glassfish.grizzly.http.server.HttpServer;
import org.glassfish.jersey.grizzly2.httpserver.GrizzlyHttpServerFactory;
import org.glassfish.jersey.moxy.json.MoxyJsonConfig;
import org.glassfish.jersey.server.ResourceConfig;

import com.wordnik.swagger.jaxrs.config.BeanConfig;

import LetterGoProject.LetterGoData.GameData;

/**
 * Class that defines the configuration and deploying
 * of the API service.
 * 
 * @author David Garcia Centelles
 */
public final class Main {
	
    /**
     * URI of the Server. 
     */
    private static URI BASE_URI;
    
    /**
     * Path to the DB Directory.
     */
   	public static String DBPATH;
    
    /**
     * Acts as a DB. 
     */
    public static GameData GD;
    
    /**
     * Void Constructor. 
     */
    private Main() { }
    
    /**
     * Configuration and upload of the server. 
     * @param args Arguments for main. 
     */
    public static void main(final String[] args) {
    	
    	boolean start = true;
    	String dir = "localhost:8080";
    	String path = "DB";
    	GD = new GameData();
    	
    	if (args.length > 0) {
			if (args[0].equals("help")) {
				System.out.println(
                        String.format(
                                "Accepted Parameters:"
                                + "%n1.IP:Port (Default: 'localhost:8080')"
                                + "%n2.Path to DB (Default: 'DB')"
                                + "%n3.Reset (Reset the application data)"));
				start = false;
			} else {
				dir = args[0];
		    	
		    	if (args.length > 1) {
		    		path = args[1];
		    		
		    		if (args.length > 2) {
		    			if (args[2].equals("reset")) {
		    				System.out.println(String.format(
		                			"Reset activated..."));
		    				File f = new File(path);
		    				FileUtil.delete(f);
		    			}
		    		}
		    	}
			}
    	}
    	
    	if (start) {
    		
    		BASE_URI = URI.create("http://" + dir + "/lettergo/");
    		DBPATH = path;
    		
    		//We check if the necessary directories exists. 
    		//If not, we create them.
    		File dirs = new File(DBPATH);
    		if (!dirs.exists()) {
    			System.out.println(String.format(
            			"Creating new directories..."));
    			dirs.mkdir();
    			for (String l : GameData.getLetters()) {
    				File temp = new File(DBPATH + "/Letters/" + l);
    				temp.mkdirs();
    			}
    		}
    		
	    	try {
	            BeanConfig beanConfig = new BeanConfig();
	            beanConfig.setVersion("1.0");
	            beanConfig.setScan(true);
	            beanConfig.setResourcePackage(
	                    Functions.class.getPackage().getName());
	            beanConfig.setBasePath(BASE_URI.toString());
	            beanConfig.setDescription("Letter GO Client API");
	            beanConfig.setTitle("Letter GO Client API");
	            
	            System.out.println(String.format(
	            		"Loading game data..."));
	            boolean exists = GD.loadDataFromFile();
	            if (!exists) {
	            	System.out.println(String.format(
	            			"Using default data..."));
	            }
	            
	            final HttpServer server
	                    = GrizzlyHttpServerFactory.createHttpServer(
	                            BASE_URI, createApp());           
	            
	            System.out.println(
	                    String.format(
	                            "Application started."
	                            + "%nHit enter to stop it..."));
	            System.in.read();
	            
	            System.out.println(
	                    String.format(
	                            "Saving game data..."));
	            GD.saveDataOnFile();
	            
	            server.shutdownNow();
	            System.exit(0);
	        } catch (IOException ex) {
	            Logger.getLogger(Main.class.getName()).
	            	log(Level.SEVERE, null, ex);
	        }
    	}	 	
    }

    /**
     * Create App. 
     * @return Resource Configuration. 
     */
    public static ResourceConfig createApp() {
        return new ResourceConfig().
                packages(Functions.class.getPackage().getName(),
                        "com.wordnik.swagger.jaxrs.listing").
                register(createMoxyJsonResolver());
    }

    /**
     * Create Moxy Json Resolver.
     * @return Context Resolver.  
     */
    public static ContextResolver<MoxyJsonConfig> createMoxyJsonResolver() {
        final MoxyJsonConfig moxyJsonConfig = new MoxyJsonConfig();
        Map<String, String> namespacePrefixMapper
                = new HashMap<String, String>(1);
        namespacePrefixMapper.put(
                "http://www.w3.org/2001/XMLSchema-instance", "xsi");
        moxyJsonConfig.setNamespacePrefixMapper(namespacePrefixMapper)
                .setNamespaceSeparator(':');
        return moxyJsonConfig.resolver();
    }
}