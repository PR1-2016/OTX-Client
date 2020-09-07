using System;

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
        }
    }
}
