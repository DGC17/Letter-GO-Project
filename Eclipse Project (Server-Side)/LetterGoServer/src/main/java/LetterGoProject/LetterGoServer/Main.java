package LetterGoProject.LetterGoServer;

import java.io.IOException;
import java.net.URI;
import java.util.HashMap;
import java.util.Map;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.ws.rs.ext.ContextResolver;
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
    private static final URI BASE_URI
            = URI.create("http://localhost:8080/lettergo/");
    
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
        try {
            BeanConfig beanConfig = new BeanConfig();
            beanConfig.setVersion("1.0");
            beanConfig.setScan(true);
            beanConfig.setResourcePackage(
                    Functions.class.getPackage().getName());
            beanConfig.setBasePath(BASE_URI.toString());
            beanConfig.setDescription("Letter GO Client API");
            beanConfig.setTitle("Letter GO Client API");
            
            //TODO: Use args. 
            DBPATH = "DB";
            
            GD = new GameData();
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
                            "Application started.%nHit enter to stop it..."));
            System.in.read();
            
            System.out.println(
                    String.format(
                            "Saving game data..."));
            GD.saveDataOnFile();
            
            server.shutdownNow();
            System.exit(0);
        } catch (IOException ex) {
            Logger.getLogger(Main.class.getName()).log(Level.SEVERE, null, ex);
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
