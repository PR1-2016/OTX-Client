using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OTX_Client
{
    public class IndicatorDetailsGeo
    {
        [Key]
        public int Key { get; set; }        
        [JsonPropertyName("country_name")]
        public string CountryName { get; set; }
        [JsonPropertyName("postal_code")]
        public string PostalCode { get; set; }
        [JsonPropertyName("city")]
        public string City { get; set; }
        [JsonPropertyName("subdivision")]
        public string Subdivision { get; set; }
        [JsonPropertyName("region")]
        public string Region { get; set; }
        [JsonPropertyName("longitude")]
        public float Longitude { get; set; }
        [JsonPropertyName("latitude")]
        public float Latitude { get; set; }

        public override string ToString()
        {
            return string.Format("City: {0}\nCountry: {1}\nSubdivision: {2}\nRegion: {3}\nLongitude: {4}\nLatitude: {5}\n",
                City, CountryName, Subdivision, Region, Longitude, Latitude);
        }
    }
}
