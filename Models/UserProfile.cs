using System;
using System.Collections.Generic;
using Newtonsoft.Json;      //JSON framework

namespace MusicHub.Controllers
{
    public class UserProfile
    {
        [JsonProperty("id")]
        public string UserId { get; set; }
        [JsonProperty("display_name")]
        public String DisplayName { get; set; }
    }

    public class Playlists
    {
        [JsonProperty("items")]
        public List<Playlist> Items { get; set; }
    }

    public class Playlist
    {
        [JsonProperty("id")]
        public String Id { get; set; }
        [JsonProperty("name")]
        public String Name { get; set; }
        [JsonProperty("owner")]
        public UserProfile Owner { get; set; }
    }

    public class Artist
    {
        [JsonProperty("name")]
        public String Name { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class Tracks
    {
        [JsonProperty("items")]
        public List<Track> Items { get; set; }
    }

    public class Track
    {
        [JsonProperty("track")]
        public FullTrack FullTrack { get; set; }
    }

    public class FullTrack
    {
        [JsonProperty("artists")]
        public List<Artist> Artists { get; set; }
        [JsonProperty("name")]
        public String Name { get; set; }
    }

    public class Releases
    {
        [JsonProperty("items")]
        public List<SimpleAlbum> Albums { get; set; }
    }

    public class SimpleAlbum
    {
        [JsonProperty("release_date")]
        public string Date { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }


    public class NewReleases
    {
        [JsonProperty("albums")]
        public Music1 Music1 { get; set; }
    }

    public class Music1
    {
        [JsonProperty("items")]
        public List<Music> Music { get; set; }
    }

    public class Music
    {
        [JsonProperty("artists")]
        public List<SimpleArtist> SimpleArtist { get; set; }
        [JsonProperty("images")]
        public List<Images> Images { get; set; }
        [JsonProperty("external_urls")]
        public OpenSpotify OpenSpotify { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }

    }

    public class SimpleArtist
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Images
    {
        [JsonProperty("url")]
        public string imgurl { get; set; }
    }

    public class OpenSpotify
    {
        [JsonProperty("spotify")]
        public string spotify { get; set; }
    }


}



