
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MusicHub.Models;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.Extensions;


namespace MusicHub.Controllers
{
    public class HomeController : Controller
    { 

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            ViewData["Message"] = "Please Login";
            ViewBag.AuthUri = GetAuth();

            return View();
        }

        public IActionResult Spotify()
        {
            return Redirect(GetAuth());
        }

        public string client_id = "client_id"; // Your client id
        public string client_secret = "client_secret"; // Your secret
        public string redirect_uri = "https://localhost:44301/Home/Auth"; // Your redirect uri

        //Method to get authorisation token from spotify
        private string GetAuth()
        {
            var qb = new QueryBuilder();
            qb.Add("response_type", "token");
            qb.Add("client_id", client_id);
            qb.Add("scope", "user-read-private user-read-email playlist-read-private");     //Scope of privleeges
            qb.Add("redirect_uri", redirect_uri);                                           //Redirect url must match url in sptify api dashboard

            return "https://accounts.spotify.com/authorize/" + qb.ToQueryString().ToString();

        }
        
        //Redirect from authorisation call
        public ActionResult Auth(string access_token, string token_type, string expires_in, string state)
        {
            
            if (string.IsNullOrEmpty(access_token))
                return View();

            //Create a new service for making spotify api calls
            SpotifyAPI spotifyService = new SpotifyAPI();

            //Get user authentication and return name to client display
            UserProfile spotifyUser = spotifyService.GetUserProfile(access_token);  //pass in the authentication token provided by spotify
            ViewBag.Users = spotifyUser.DisplayName;

            //Get user playlists ids
            Playlists playlists = spotifyService.GetPlaylists(spotifyUser.UserId, access_token);
            ViewBag.Playlists = playlists;

            //Get all tracks from user
            List<string> Artists = spotifyService.GetArtists(playlists, access_token);
            ViewBag.Artists = Artists;
            
            //Make a call to a method which returns new releases in the form of 3 seperate variables
            var(l1, l2, l3) = spotifyService.GetNewReleases(Artists, access_token);
            ViewBag.Url = l1;
            ViewBag.Album = l2;
            ViewBag.Artwork = l3;

            //Ticketmaster service for checking if any of the users artists have any upcoming concert dates
            TicketmasterAPI ticketmasterService = new TicketmasterAPI();

            //Return url link to event and event name
            var (url, name) = ticketmasterService.GetConcerts(Artists);
            ViewBag.Url1 = url;
            ViewBag.Name = name;

            //Return the app html file, Auth.cshtml
            return View();
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
