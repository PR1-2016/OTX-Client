using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OTX_Client
{
    public class Indicator
    {
        [Key]
        public int Key { get; set; }
        [JsonPropertyName("indicator")]
        public string IndicatorProperty { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("created")]
        public DateTime Created { get; set; }
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("pulse_key")]
        public string PulseId { get; set; }

        public override string ToString()
        {
            return String.Format("Indicator: {0}\nID: {1}\nType: {2}\nDescription: {3}\nCreated:  {4}\n",
                IndicatorProperty, Id, Type, Description, Created);
        }
    }
}
