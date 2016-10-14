**Letter GO Project**
-------------------------

**AUTHOR** 

David García Centelles

--------------------

SERVER DEPLOYMENT
------------------

--------------------

**REQUIRED SOFTWARE**

* (Only Option 1) Eclipse IDE for Java Developers (32/64 bits) - [Eclipse Download](https://www.eclipse.org/downloads/) (It's important that the version includes Maven integration.) 
* Last version of Java installed in the machine. 

**STEPS** 
  
  * **OPTION 1: Using Source Project**

    1. Download the repository. 
    2. Install and **open Eclipse IDE.** 
    3. Import the project inside the folder: **Eclipse Project (Server-Side)**
    4. Run the project an the API will run in: **localhost:8080** and all the data will be stored in the **DB** directory. 

  * **OPTION 2: Using JAR File**

    1. Download the repository. 
    2. Copy the files located at: **Builds/Server** in a desired place. 
    3. Open the command shell and run: **java -jar lettergoserver.jar**
      * Without adding parameters the application will run in: **localhost:8080** and all the data will be stored in the **DB** directory. 
      * Additional parameters: 
        1. IP/Port (IP/Port of the API service... The port needs to be open previously).
          * Example: **java -jar lettergoserver.jar localhost:8080**
        2. DB Directory (The directory where the data will be stored). 
          * Example: **java -jar lettergoserver.jar localhost:8080 DB**
        3. Reset (To clean all the data from the server on initialization - CAUTION! It can't be recovered!). 
          * Example: **java -jar lettergoserver.jar localhost:8080 DB reset**

**API TEST**

1. (Recommendation) Download [Advanced REST Client](https://chrome.google.com/webstore/detail/advanced-rest-client/hgmloofddffdnphfgcellkdfbfbjeloo) to test the API functions. 
2. API calls (Change "localhost:8080 with your selected IP/Port if needed):
  * http://localhost:8080/lettergo/functions/generateLetter
  * http://localhost:8080/lettergo/functions/addUser
      * Data Required (JSON): **{"username":*[string]*, "password":*[string]*}**
  * http://localhost:8080/lettergo/functions/loginUser
      * Data Required (JSON): **{"username":*[string]*, "password":*[string]*}**
  * http://localhost:8080/lettergo/functions/sendResults 
    * Data Required (JSON): **{"username":*[string]*, "letter":*[string]*, "image":*[base64_string]*}**

--------------------

CLIENT DEPLOYMENT
------------------

--------------------

**REQUIRED SOFTWARE**

* (Only Option 1) Unity 3D Personal Edition - [Unity Download](https://store.unity.com/es/?_ga=1.108037634.1009149316.1476277598)
* An Android Device. 

**STEPS** 
  
  * **OPTION 1: Using Source Project**

    0. IMPORTANT: Most of the functionalities will not be accessible due to the fact that this application uses the mobile camera.
    1. Download the repository. 
    2. Install and **open Eclipse IDE.** 
    3. Import the project inside the folder: **Unity Project (Client-Side)**
    4. Run the project and you will see the basic interfaces and some functionalities. 
      * If the server is running in localhost:8080 you can check the login and the letter generation.

  * **OPTION 2: Using APK File**

    1. Download the repository. 
    2. Copy the files located at: **Builds/Client** in the device.  
    3. Install the application. 
    4. Run the application and specify the IP/Port in the first screen. 