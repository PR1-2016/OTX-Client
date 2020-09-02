using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OTX_Client
{
    public class Pulse
    {
        [Key]
        public int Key { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("TLP")]
        public string Tlp { get; set; }
        [JsonPropertyName("indicators")]
        public List<Indicator> Indicators { get; set; }
        [JsonPropertyName("modified")]
        public DateTime Modified { get; set; }
        [JsonPropertyName("created")]
        public DateTime Created { get; set; }      
        [JsonPropertyName("targeted_countries")]
        public List<string> Targeted_countries { get; set; }
        [JsonPropertyName("revision")]
        public int Revision { get; set; }
        [JsonPropertyName("isPublic")]
        public bool IsPublic { get; set; }

        public override string ToString()
        {
            string retVal = String.Format("Pulse {0}:\nName: '{1}'\nDescription: {2}\nCreated: {3}\nModified: {4}\n", 
                Id, Name, Description, Created, Modified);

            if (!string.IsNullOrEmpty(Tlp))
            {
                retVal += string.Format("TLP: {0}\n", Tlp);
            }

            if (Targeted_countries.Count != 0)
            {
                retVal += "Targeted countries: ";
                foreach (var i in Targeted_countries)
                {
                    retVal += i + " ";
                }
                retVal += "\n";
            }

            if (Indicators != null && Indicators.Count != 0)
            {
                retVal += "\nIndicators: \n\n";
                foreach (var i in Indicators)
                {
                    retVal += i + "\n";
                }
            }
            
            return retVal;
        }
    }
}
