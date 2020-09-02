using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OTX_Client
{
    public class Page<T>
    {
        [JsonPropertyName("results")]
        public List<T> Results { get; set; }
        [JsonPropertyName("next")]
        public string Next { get; set; }
    }
}
