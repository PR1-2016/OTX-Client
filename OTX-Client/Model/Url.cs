using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OTX_Client
{
    public class Url
    {
        [Key]
        public int Key { get; set; }
        [JsonPropertyName("domain")]
        public string Domain { get; set; }
        [JsonPropertyName("url")]
        public string ActualUrl { get; set; }
        [JsonPropertyName("hostname")]
        public string Hostname { get; set; }
        [JsonPropertyName("date")]
        public string Date { get; set; }
        [JsonPropertyName("encoded")]
        public string EncodedUrl { get; set; }

        public override string ToString()
        {
            return string.Format("Domain: {0}\nUrl: {1}\nEncoded: {2}\nHostname: {3}\nDate: {4}\n", 
                Domain, ActualUrl, EncodedUrl, Hostname, Date);
        }
    }
}
