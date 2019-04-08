using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MusicHub.Models;
using Newtonsoft.Json;

namespace MusicHub.Controllers
{
    public class TicketmasterAPI
    {
        public TicketmasterAPI()
        {

        }

        //Get list of concerts of the users favourite artists
        public (List<string> Url, List<string> Name) GetConcerts(List<string> Artists)
        {
            string url1 = string.Format("https://app.ticketmaster.com/discovery/v2/events.json?countryCode=IE&city=Dublin&classificationName=music&apikey=ZgYSxFmoUdl5nqXPzxFJ7OWpqG0ZLX2W&size=200");
            Concerts concerts = TicketmasterService<Concerts>(url1);

            
            List<string> Url = new List<string>();
            List<string> Name = new List<string>();

            foreach (var musician in Artists)
            {

                if (musician == null)
                    continue;

                foreach (var item in concerts.Listings.Events)
                {
                    //For every event returned by the ticketmaster API check if it matches the users artists
                    bool isStringContainedInList = item.Name.Contains(musician);

                    if (isStringContainedInList == false || musician == "And")
                        continue;
                    
                    //If the strings match add  to list of concerts
                    Name.Add(item.Name);
                    Url.Add(item.Link);
                      
                }
            }

            List<string> distinctname = Name.Distinct().ToList();
            List<string> distincturl = Url.Distinct().ToList();
            return (distincturl, distinctname);       //Return the list of strings
        }


        //Service for making call to the Ticketmaster API
        public T TicketmasterService<T>(string url)
        {
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Method = "GET";
                request.Headers.Set("Authorization", "Bearer");
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
