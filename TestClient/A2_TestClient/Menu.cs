using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2_TestClient
{
    public class Menu
    {
        const string NET_CONFIG = "1";
        const string RUN_TESTS = "2";
        const string DEBUG_MODE = "3";
        const string QUIT = "q";

        // https://wellsb.com/csharp/beginners/create-menu-csharp-console-application
        public static bool MainMenu()
        {
            Console.Clear();
            Console.WriteLine("*****Logging Test Client*****\n");
            Console.WriteLine("IP Address:{0}", Client.ip);
            Console.WriteLine("Port:{0}", Client.port);
            Console.WriteLine("Debug Mode:{0}\n", Tests.debugMode);
            Console.WriteLine("Options:\n");
            Console.WriteLine("\t1 Configure Network\n");
            Console.WriteLine("\t2 Run Tests\n");
            Console.WriteLine("\t3 Toggle Debug Mode\n");
            Console.WriteLine("\tQ Quit\n");

            Console.WriteLine("Select option:");
            switch (Console.ReadLine())
            {
                case NET_CONFIG:
                    Console.Clear();
                    // Get Ip
                    Console.WriteLine("Set IP:");
                    string getIp = Console.ReadLine();
                    if(!string.IsNullOrWhiteSpace(getIp))
                    {
                        Client.ip = getIp;
                    }
                    else
                    {
                        Console.WriteLine("Invalid IP\n");
                    }
                    // Get port
                    Console.WriteLine("Set Port:");
                    int getPort = 0;
                    int.TryParse(Console.ReadLine(), out getPort);
                    if (getPort > 0)
                    {
                        Client.port = getPort;
                    }
                    else
                    {
                        Console.WriteLine("Invalid Port\n");
                    }
                    return true;

                case RUN_TESTS:
                    {
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine("Tests:\n");
                            Console.WriteLine("\t1 Send Valid Field Combinations\n");
                            Console.WriteLine("Select Test:");
                            switch (Console.ReadLine())
                            {
                                case "1":
                                    Console.WriteLine("Enter delay time(ms): ");
                                    int delay = 0;
                                    int.TryParse(Console.ReadLine(), out delay);
                                    Console.WriteLine("Enter number of logs to send: ");
                                    int logsToSend = 0;
                                    int.TryParse(Console.ReadLine(), out logsToSend);
                                    Console.WriteLine("Sending {0} logs with a {1}(ms) delay between each log", logsToSend, delay);
                                    Tests.TestValidFields(delay, logsToSend);
                                    break;
                            }
                            Console.WriteLine("Continue testing?");
                            Console.WriteLine("Enter 'y' to continue, or any key to return to main menu:");
                            if (!(Console.ReadLine() == "y"))
                            {
                                break;
                            }
                        }
                        return true;
                    }
                case DEBUG_MODE:
                    Tests.debugMode = !Tests.debugMode;
                    return true;
                case QUIT:
                    return false;
                default:
                    return true;
            }
        }
    }
}
