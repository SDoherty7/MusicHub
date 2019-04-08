# MusicHub
A music hub that provides personalised new music releases and concert dates based on the users favourite artists. This personal project was a way for me to learn the C# language and ASP.Net framework which I have never used or been introduced to before. The project also features elements of JavaScript and HTML.
This is still an active project which I will be regularly commiting to. 

### App Features
While some of the front-end code for the apps interface needs completing I have included all of the server side code which performs the neccessary authentication and api calls.

The app works by accessing the users spotify information including their playlists, tracks and artists. Based on the users artists the app returns new music releases for the user which can be directly accessed from the app. A ticketmaster API is queried to see if any artists have upcoming concerts in the local area. The app also includes a ticketmast widget with genric event information. 
This app implements the MVC pattern. The HomeController appropriately handles all requests and makes calls to the SpotifyAPI service class and TicketmasterAPI service class and returns Views to the user. 
The code also has an api call method from the client side using JavaScript this method is not used in my app as it it done in the backend using C#but I have left the code in just as an alternative method.

### Prerequisites
.NET Core SDK, ticketmaster developer account and a spotify account which will give access to their developer features.

### Configuration
- Replace the client_id and client_secret in the homeController.
- Add the redirect url to the whitelist on your spotify developer dashboard.
- Change the ticketmaster api key 

#### Spotify Authentication
The first step in the app requires authentication against the Spotify web API. App authorization is required to access the Spotify platform and User authorization to access the users data. This project show how the spotify authentication flow works by implementing the "Authorization Code" flow which is similar to the example from the [Spotify Guide](https://github.com/spotify/web-api-auth-examples) which is written in Node.js. 
Upon receiving authorization from the user Spotify returns an access token which must be used in all subsequent call to the API


#### Questions?
Please let me know if you have any questions or want any information on the workings of the code
