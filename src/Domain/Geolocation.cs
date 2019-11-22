using Newtonsoft.Json;

namespace Domain
{
    public class Geolocation
    {
        public string Key { get; set; }
        public string Ip { get; set; }
        public string Type { get; set; }
        [JsonProperty("continent_code")]
        public string ContinentCode { get; set; }
        [JsonProperty("continent_name")]
        public string ContinentName { get; set; }
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }
        [JsonProperty("country_name")]
        public string CountryName { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Location Location { get; set; }

        public Geolocation()
        {
        }
    }
}