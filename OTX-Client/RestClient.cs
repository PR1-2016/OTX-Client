using Spring.Http;
using Spring.Http.Converters.Json;
using Spring.Rest.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace OTX_Client
{
    public class RestClient
    {
        public RestTemplate restTemplate;
        public HttpEntity requestEntity;
        public Dictionary<string, object> parameters;
        public Database database = new Database();
        public JsonSerializerOptions serializerOptions = new JsonSerializerOptions() { WriteIndented = true };
        public void ConfigureRestTemplate()
        {
            restTemplate = new RestTemplate("https://otx.alienvault.com");

            requestEntity = new HttpEntity();
            requestEntity.Headers["X-OTX-API-KEY"] = "96e493a64ea73f7af266576765143c60ace434a380346934a66780b7f6601a3f";
            requestEntity.Headers.Add("User-Agent", 
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.125 Safari/537.36");
            requestEntity.Headers.ContentType = MediaType.APPLICATION_JSON;
        }

        public void GetPulseById(string id)
        {
            parameters = new Dictionary<string, object>();
            parameters.Add("id", id);

            HttpResponseMessage<string> responsePulse = restTemplate.Exchange<string>("/api/v1/pulses/{id}", HttpMethod.GET, requestEntity, parameters);
            if (responsePulse.StatusCode == HttpStatusCode.OK)
            {
                Pulse pulse = JsonSerializer.Deserialize<Pulse>(responsePulse.Body);
                database.AddPulse(pulse);
                File.WriteAllText(string.Format("./pulse-{0}.json", pulse.Id), JsonSerializer.Serialize(pulse, serializerOptions));
                Console.WriteLine(pulse);
                return;
            }

            Console.WriteLine("Response status: {0}", responsePulse.StatusCode.ToString());
        }
        public void GetIndicatorsByPulseId(string pulseId)
        {
            parameters = new Dictionary<string, object>();
            parameters.Add("pulseId", pulseId);

            int limit = 20;
            List<Indicator> indicators = new List<Indicator>();
            IndicatorPage indicatorPage;
            HttpResponseMessage<string> response = restTemplate.Exchange<string>("/api/v1/pulses/{pulseId}/indicators", HttpMethod.GET, requestEntity, parameters);
            do
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Console.WriteLine("Response status: {0}", response.StatusCode.ToString());
                    return;
                }

                indicatorPage = JsonSerializer.Deserialize<IndicatorPage>(response.Body);
                indicators = indicators.Concat(indicatorPage.Results).ToList();
                if (indicatorPage.Next == null)
                    break;
                response = restTemplate.Exchange<string>(indicatorPage.Next, HttpMethod.GET, requestEntity);
            } while (indicators.Count < limit);

            foreach (var i in indicators)
            {
                database.AddIndicator(i);
                File.WriteAllText(string.Format("./indicator-{0}.json", i.Id), JsonSerializer.Serialize(i, serializerOptions));
                Console.WriteLine(i);
            }
        }
        public void GetIndicatorDetails(string type, string indicator, string section)
        {
            parameters = new Dictionary<string, object>();
            parameters.Add("type", type);
            parameters.Add("indicator", indicator);
            parameters.Add("section", section);

            HttpResponseMessage<string> response = restTemplate.Exchange<string>("/api/v1/indicators/{type}/{indicator}/{section}", HttpMethod.GET, requestEntity, parameters);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                switch (section)
                {
                    case "geo":
                        {
                            IndicatorDetailsGeo indicatorDetails = JsonSerializer.Deserialize<IndicatorDetailsGeo>(response.Body);
                            Console.WriteLine(indicatorDetails);
                            //File.WriteAllText(string.Format("./indicator-{0}-geoDetails.json", i.Id), JsonSerializer.Serialize(i, serializerOptions));
                            break;
                        }
                    case "url_list":
                        {
                            IndicatorDetailsUrls indicatorDetails = JsonSerializer.Deserialize<IndicatorDetailsUrls>(response.Body);
                            
                            foreach (var f in indicatorDetails.Url_list)
                            {
                                Console.WriteLine(f);
                            }
                            break;
                        }
                }
                return;
            }

            Console.WriteLine("Response status: {0}", response.StatusCode.ToString());
        }
        public void GetPulsesFromUser(string username)
        {
            parameters = new Dictionary<string, object>();
            parameters.Add("username", username);

            int limit = 20;
            List<Pulse> pulses = new List<Pulse>();
            PulsePage pulsePage;
            HttpResponseMessage<string> response = restTemplate.Exchange<string>("/api/v1/pulses/user/{username}/?username={username}", HttpMethod.GET, requestEntity, parameters);
            do
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Console.WriteLine("Response status: {0}", response.StatusCode.ToString());
                    return;
                }

                pulsePage = JsonSerializer.Deserialize<PulsePage>(response.Body);
                pulses = pulses.Concat(pulsePage.Results).ToList();
                if (pulsePage.Next == null)
                    break;
                response = restTemplate.Exchange<string>(pulsePage.Next, HttpMethod.GET, requestEntity);
            } while (pulses.Count < limit);
            
            foreach (var p in pulses)
            {
                database.AddPulse(p);
                File.WriteAllText(string.Format("./pulse-{0}.json", p.Id), JsonSerializer.Serialize(p, serializerOptions));
                Console.WriteLine(p);
            }
        }
        public void GetPulsesFromUserWithLimit(string username, int limit)
        {
            int limitParam = limit > 10 ? 10 : limit;
            parameters = new Dictionary<string, object>();
            parameters.Add("username", username);
            parameters.Add("limit", limitParam);

            List<Pulse> pulses = new List<Pulse>();
            PulsePage pulsePage;
            HttpResponseMessage<string> response = restTemplate.Exchange<string>("/api/v1/pulses/user/{username}/?username={username}&limit={limit}&sort=-modified", HttpMethod.GET, requestEntity, parameters);
            do
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Console.WriteLine("Response status: {0}", response.StatusCode.ToString());
                    return;
                }

                pulsePage = JsonSerializer.Deserialize<PulsePage>(response.Body);
                pulses = pulses.Concat(pulsePage.Results).ToList();
                if (pulsePage.Next == null)
                    break;
                response = restTemplate.Exchange<string>(pulsePage.Next, HttpMethod.GET, requestEntity);
            } while (pulses.Count < limit);

            foreach (var p in pulses)
            {
                database.AddPulse(p);
                File.WriteAllText(string.Format("./pulse-{0}.json", p.Id), JsonSerializer.Serialize(p, serializerOptions));
                Console.WriteLine(p);
            }
        }
        public void GetPulsesFromUserSince(string username, DateTime lastUpdate)
        {
            parameters = new Dictionary<string, object>();
            parameters.Add("username", username);

            int limit = 20;
            List<Pulse> pulses = new List<Pulse>();
            PulsePage pulsePage;
            HttpResponseMessage<string> response = restTemplate.Exchange<string>("/api/v1/pulses/user/{username}/?username={username}", HttpMethod.GET, requestEntity, parameters);
            do
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Console.WriteLine("Response status: {0}", response.StatusCode.ToString());
                    return;
                }

                pulsePage = JsonSerializer.Deserialize<PulsePage>(response.Body);
                foreach (var p in pulsePage.Results)
                {
                    if (p.Modified > lastUpdate)
                    {
                        pulses.Add(p);
                    }
                    else
                    {
                        limit = pulses.Count();
                    }
                }
                if (pulsePage.Next == null)
                    break;
                response = restTemplate.Exchange<string>(pulsePage.Next, HttpMethod.GET, requestEntity);
            } while (pulses.Count < limit);
            
            foreach (var p in pulses)
            {
                database.AddPulse(p);
                File.WriteAllText(string.Format("./pulse-{0}.json", p.Id), JsonSerializer.Serialize(p, serializerOptions));
                Console.WriteLine(p);
            }
        }
        public void GetIndicatorsByPulseIdSince(string pulseId, DateTime lastUpdate)
        {
            parameters = new Dictionary<string, object>();
            parameters.Add("pulseId", pulseId);

            int limit = 20;
            List<Indicator> indicators = new List<Indicator>();
            IndicatorPage indicatorPage;
            HttpResponseMessage<string> response = restTemplate.Exchange<string>("/api/v1/pulses/{pulseId}/indicators/?sort=-created",
                HttpMethod.GET, requestEntity, parameters);
            do
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Console.WriteLine("Response status: {0}", response.StatusCode.ToString());
                    return;
                }

                indicatorPage = JsonSerializer.Deserialize<IndicatorPage>(response.Body);
                foreach (var i in indicatorPage.Results)
                {
                    if (i.Created > lastUpdate)
                    {
                        indicators.Add(i);
                    }
                    else
                    {
                        limit = indicators.Count();
                    }
                }
                if (indicatorPage.Next == null)
                    break;
                response = restTemplate.Exchange<string>(indicatorPage.Next, HttpMethod.GET, requestEntity);
            } while (indicators.Count < limit);

            foreach (var i in indicators)
            {
                database.AddIndicator(i);
                File.WriteAllText(string.Format("./indicator-{0}.json", i.Id), JsonSerializer.Serialize(i, serializerOptions));
                Console.WriteLine(i);
            }
        }
        public void GetLatestPulsesSubscribed()
        {
            int limit = 5;
            List<Pulse> pulses = new List<Pulse>();
            PulsePage pulsePage;
            HttpResponseMessage<string> response = restTemplate.Exchange<string>("/api/v1/pulses/subscribed", HttpMethod.GET, requestEntity);
            do
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Console.WriteLine("Response status: {0}", response.StatusCode.ToString());
                    return;
                }

                pulsePage = JsonSerializer.Deserialize<PulsePage>(response.Body);
                pulses = pulses.Concat(pulsePage.Results).ToList();
                if (pulsePage.Next == null)
                    break;
                response = restTemplate.Exchange<string>(pulsePage.Next, HttpMethod.GET, requestEntity);
            } while (pulses.Count < limit);
            
            foreach (var p in pulses)
            {
                database.AddPulse(p);
                File.WriteAllText(string.Format("./pulse-{0}.json", p.Id), JsonSerializer.Serialize(p, serializerOptions));
                Console.WriteLine(p);
            }
        }
        public void BrowseLatestPulses()
        {
            int limit = 20;
            List<Pulse> pulses = new List<Pulse>();
            PulsePage pulsePage;
            HttpResponseMessage<string> response = restTemplate.Exchange<string>("/api/v1/search/pulses/?sort=-modified", HttpMethod.GET, requestEntity);
            do
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Console.WriteLine("Response status: {0}", response.StatusCode.ToString());
                    return;
                }

                pulsePage = JsonSerializer.Deserialize<PulsePage>(response.Body);
                pulses = pulses.Concat(pulsePage.Results).ToList();
                if (pulsePage.Next == null)
                    break;
                response = restTemplate.Exchange<string>(pulsePage.Next, HttpMethod.GET, requestEntity);
            } while (pulses.Count < limit);
            
            foreach (var p in pulses)
            {
                database.AddPulse(p);
                File.WriteAllText(string.Format("./pulse-{0}.json", p.Id), JsonSerializer.Serialize(p, serializerOptions));
                Console.WriteLine(p);
            }
        }
    }
}
