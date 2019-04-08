using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;      //JSON framework

namespace MusicHub.Models
{
    public class Concerts
    {
        [JsonProperty("_embedded")]
        public Listings Listings { get; set; }
    }

    public class Listings
    {
        [JsonProperty("events")]
        public List<Events> Events { get; set; }
    }

    public class Events
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("url")]
        public string Link { get; set; }
    }
}
