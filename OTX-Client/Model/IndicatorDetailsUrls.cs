using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OTX_Client
{
    public class IndicatorDetailsUrls
    {
        private List<Url> url_list;

        [JsonPropertyName("url_list")]
        public List<Url> Url_list { get => url_list; set => url_list = value; }
    }
}
