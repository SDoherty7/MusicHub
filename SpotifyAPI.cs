using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
namespace MusicHub.Controllers
{
    public class SpotifyAPI 
    {
        public string Token { get; set; }

        public SpotifyAPI()
        {

        }

        public SpotifyAPI(string token)
        {
            Token = token;
        }

        //Method for getting the user login information
        public UserProfile GetUserProfile(string token)
        {
            string url = "https://api.spotify.com/v1/me";
            UserProfile spotifyUser = SpotifyService<UserProfile>(url, token);
            return spotifyUser;
        }

        //method for getting User playlist information from this the users favourite artists can be found
        public Playlists GetPlaylists(string UserId, string token)
        {
            string url = string.Format("https://api.spotify.com/v1/users/{0}/playlists", UserId);   //URL to access the spotify api
            Playlists playlists = SpotifyService<Playlists>(url, token);

            return playlists;
        }

        //Method for returning artists from the users playlists
        public List<string> GetArtists(Playlists playlists, string token)
        {
            List<string> Artists = new List<string>();

            foreach (var playlist in playlists.Items)
            {
                string url = string.Format("https://api.spotify.com/v1/users/" + playlist.Owner.UserId + "/playlists/" + playlist.Id + "/tracks");
                Tracks tracks = SpotifyService<Tracks>(url, token);

                if (tracks == null)
                    continue;

                foreach (var track in tracks.Items)
                {

                    foreach (var artist in track.FullTrack.Artists)
                    {
                        Artists.Add(artist.Name);
                    }
                }
            }
            //Return the list of artists
            return Artists;
        }

        //A method which checks users favourite artists against  new releases and returns the new release if there is a match
        public (List<string> Url, List<string> Album, List<string> Artwork) GetNewReleases(List<string> Artists, string token)
        {

            string url1 = string.Format("https://api.spotify.com/v1/browse/new-releases?country=IE");
            NewReleases newreleases = SpotifyService<NewReleases>(url1, token);


            //Three lists to be returned for the display
            List<string> Url = new List<string>();
            List<string> Album = new List<string>();
            List<string> Artwork = new List<string>();


            foreach (var item in newreleases.Music1.Music)
            {

                if (newreleases == null)
                    continue;

                foreach (var musician in item.SimpleArtist)
                {

                    string releaseArtist = musician.Name;

                    //Check if the new release object has an artist which matches the users artist list
                    bool isStringContainedInList = Artists.Contains(releaseArtist);
                    //If there is a match add the album information to the appropriate lists, ready for display
                    if (isStringContainedInList == true)
                    {
                        Url.Add(item.OpenSpotify.spotify);                                     
                        Album.Add(item.Name);
                        Artwork.Add(item.Images[1].imgurl);
                    }

                }


            }
            return (Url, Album, Artwork);       //Return the list of strings
        }


        //This is an alternative method which gets the latest artist release if it is within the pas month
        //This method returns much more results however it requires a lot of calls to the spotify API
        //due to spotifys rate limiting on api calls this is a very time consuming but it returns more detailed results

        //The method can be changed to get more parameters from the JSON data as per GetNewReleases() above
        public List<string> GetReleasesFromAlubms(Playlists playlists, string token)
        {
            //Make a list of new releases
            List<string> Releases = new List<string>();

            foreach (var playlist in playlists.Items)
            {
                //Get all tracks from the playlists
                string url = string.Format("https://api.spotify.com/v1/users/" + playlist.Owner.UserId + "/playlists/" + playlist.Id + "/tracks");
                Tracks tracks = SpotifyService<Tracks>(url, token);

                if (tracks == null)
                    continue;

                foreach (var track in tracks.Items)
                {
                    string music = track.FullTrack.Name;
                    string artists = "";

                    //From the tracks get the artist, the artist will then be checked for album releases
                    foreach (var artist in track.FullTrack.Artists)
                    {
                        
                        //Current date to use as a comparator 
                        DateTime dt1 = DateTime.Now;
                        string url1 = string.Format("https://api.spotify.com/v1/artists/" + artist.Id + "/albums?market=ES&include_groups=album,single,appears_on,compilation&limit=2");
                        Releases releases = SpotifyService<Releases>(url1, token);

                        if (releases == null)
                            continue;

                        foreach (var album in releases.Albums)
                        {
                            string date = album.Date;
                            
                            //To deal with apotify returning only the year release
                            if (date.Length < 9)
                                continue;

                            DateTime dt2 = DateTime.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

                            //Get number of days between 2 dates if it is less than 30 days it can be considered a new release
                            var result = (dt1 - dt2).Days;
                            if (result < 30)
                            {
                                Releases.Add(album.Name);
                            }
                        }
                    }
                }
            }
            return Releases;
        }


        //Service for making calls to the spotify API it is generic so will work with all api calls
        //The authorisation token provided by spotify must always be provided
        public T SpotifyService<T>(string url, string token)
        {
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Method = "GET";
                request.Headers.Set("Authorization", "Bearer" + " " + token);
                request.ContentType = "application/json; charset=utf-8";

                T type = default(T);

                using (WebResponse response = request.GetResponse())
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(dataStream))
                        {
                            string responseFromServer = reader.ReadToEnd();
                            type = JsonConvert.DeserializeObject<T>(responseFromServer);
                        }
                    }
                }
                return type;
            }
            catch (WebException ex)
            {
                return default(T);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
