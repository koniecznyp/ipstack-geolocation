using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain
{
    public class Location
    {
        public int GeonameId { get; set; }
        public string Capital { get; set; }
        [JsonProperty("country_flag")]
        public string CountryFlag { get; set; }
        [JsonProperty("country_flag_emoji")]
        public string CountryFlagEmoji { get; set; }
        [JsonProperty("country_flag_emoji_unicode")]
        public string CountryFlagEmojiUnicode { get; set; }
        [JsonProperty("calling_code")]
        public string CallingCode { get; set; }
        public bool IsEu { get; set; }
        public IEnumerable<Language> Languages { get; set; }
        
        public Location()
        {
        }
    }
}