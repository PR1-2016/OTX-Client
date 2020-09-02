using System;
using System.ComponentModel.Design;

namespace OTX_Client
{
    public class Program
    {
        private static void Menu(RestClient restClient) 
        {
            int input = 0;
            do
            {
                Console.WriteLine("Please choose an option: \n");
                Console.WriteLine("1. Get pulse by id");
                Console.WriteLine("2. Get indicators by pulse id");
                Console.WriteLine("3. Get pulses from user");
                Console.WriteLine("4. Get pulses from user with limit");
                Console.WriteLine("5. Get pulses from user since");
                Console.WriteLine("6. Get indicators by pulse id since");
                Console.WriteLine("7. Get indicator details");
                Console.WriteLine("8. Get latest pulses from subscribed");
                Console.WriteLine("9. Get latest pulses");
                Console.WriteLine("10. Close the program");
                if (int.TryParse(Console.ReadLine(), out input))
                {
                    switch (input)
                    {
                        case 1:
                            {
                                Console.WriteLine("Please enter pulse id: ");
                                restClient.GetPulseById(Console.ReadLine());
                                break;
                            }
                        case 2:
                            {
                                Console.WriteLine("Please enter pulse id: ");
                                restClient.GetIndicatorsByPulseId(Console.ReadLine());
                                break;
                            }
                        case 3:
                            {
                                Console.WriteLine("Please enter the username: ");
                                restClient.GetPulsesFromUser(Console.ReadLine());
                                break;
                            }
                        case 4:
                            {
                                Console.WriteLine("Please enter the username: ");
                                string username = Console.ReadLine();
                                Console.WriteLine("Please enter the limit: ");
                                if (int.TryParse(Console.ReadLine(), out int limit))
                                {
                                    restClient.GetPulsesFromUserWithLimit(username, limit);
                                }
                                else
                                {
                                    Console.WriteLine("That is not a valid number!");
                                }
                                break;
                            }
                        case 5:
                            {
                                Console.WriteLine("Please enter the username: ");
                                string username = Console.ReadLine();
                                Console.WriteLine("Please enter the date (dd-mm-yyyy [hh:mm:ss]): ");
                                DateTime date = DateTime.Parse(Console.ReadLine());
                                restClient.GetPulsesFromUserSince(username, date);
                                break;
                            }
                        case 6:
                            {
                                Console.WriteLine("Please enter pulse id: ");
                                string pulseId = Console.ReadLine();
                                Console.WriteLine("Please enter the date (dd-mm-yyyy [hh:mm:ss]): ");
                                DateTime date = DateTime.Parse(Console.ReadLine());
                                restClient.GetIndicatorsByPulseIdSince(pulseId, date);
                                break;
                            }
                        case 7:
                            {
                                Console.WriteLine("Please enter indicator type: ");
                                string type = Console.ReadLine();
                                Console.WriteLine("Please enter indicator: ");
                                string indicator = Console.ReadLine();
                                Console.WriteLine("Please enter desired section name ('geo' or 'url_list'): ");
                                string section = Console.ReadLine();
                                restClient.GetIndicatorDetails(type, indicator, section);
                                break;
                            }
                        case 8:
                            {
                                restClient.GetLatestPulsesSubscribed();
                                break;
                            }
                        case 9:
                            {
                                restClient.BrowseLatestPulses();
                                break;
                            }
                        case 10:
                            {
                                Console.WriteLine("You have successfully closed the program!");
                                break;
                            }
                        default:
                            {
                                Console.WriteLine("Please enter a valid option number (1-10).\n");
                                break;
                            }
                    }
                }

            } while (input != 10);
        }
        private static void Main(string[] args)
        {
            RestClient restClient = new RestClient();
            restClient.ConfigureRestTemplate();
            Menu(restClient);

            //Console.WriteLine("------------GET PULSE BY ID------------");
            //restClient.GetPulseById("5f3fe1749751382284740104");
            //Console.WriteLine("------------GET INDICATORS BY PULSE ID------------");
            //restClient.GetIndicatorsByPulseId("5eca9dbd2a4812482fc16e3d");
            //Console.WriteLine("------------GET PULSES BY USERNAME------------");
            //restClient.GetPulsesFromUser("david3");
            //Console.WriteLine("------------GET PULSES BY USERNAME WITH LIMIT------------");
            //restClient.GetPulsesFromUserWithLimit("AlienVault", 30);
            //Console.WriteLine("------------GET INDICATOR DETAILS------------");
            //restClient.GetIndicatorDetails("IPv4", "192.241.237.51", "url_list");
            //Console.WriteLine("------------GET LATEST PULSES SUBSCRIBED------------");
            //restClient.GetLatestPulsesSubscribed();
            //Console.WriteLine("------------BROWSE LATEST PULSES------------");
            //restClient.BrowseLatestPulses();
            //Console.WriteLine("------------GET LATEST PULSES FROM USER SINCE------------");
            //restClient.GetPulsesFromUserSince("AlienVault", new DateTime(2020, 8, 15, 8, 0, 0));
            //Console.WriteLine("------------GET LATEST INDICATORS BY PULSE ID SINCE------------");
            //restClient.GetIndicatorsByPulseIdSince("5c76b2acd1420a1aac451307", new DateTime(2020, 8, 24, 15, 0, 0));
        }
    }
}
