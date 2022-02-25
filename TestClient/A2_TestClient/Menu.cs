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
            Console.WriteLine("Options:");
            Console.WriteLine("\t1 Configure Network\n");
            Console.WriteLine("\t2 Run Tests\n");
            Console.WriteLine("\t3 Toggle Debug Mode\n");
            Console.WriteLine("\tq Quit\n");

            Console.WriteLine("Select option:");
            switch (Console.ReadLine())
            {
                /************************* Configure network **************************/
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

                /************************* Run Tests **************************/
                case RUN_TESTS:
                    {
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine("Tests:\n");
                            Console.WriteLine("\t1 Custom Log\n");
                            Console.WriteLine("\t2 Custom Invalid Log\n");
                            Console.WriteLine("\t3 Custom Mixed Log\n");
                            Console.WriteLine("\n");
                            Console.WriteLine("\t4 Test All Valid Field Permutations\n");
                            Console.WriteLine("\t5 Test All Invalid Log Field Permutations\n");
                            Console.WriteLine("\t6 Test All Mixed Log Field Permutations\n");
                            Console.WriteLine("\n");
                            Console.WriteLine("\t7 Regular Valid Log\n");
                            Console.WriteLine("\t8 Regular Invalid Log\n");
                            Console.WriteLine("\t9 Regular Mixed Log\n");
                            Console.WriteLine("\n");
                            Console.WriteLine("\t10 Invalid Error Levels\n");
                            Console.WriteLine("Select Test:");
                            switch (Console.ReadLine())
                            {
                                /************************* Test Custom Valid Log **************************/
                                case "1":
                                    Console.WriteLine("Valid field tags: \n\t(-dt)Date\n\t(-tm)Time\n\t(-dn)Device Name\n\t(-an)Application Name\n\t(-pi)Process ID\n\t(-el)Error Level\n\t(-ms)Message");
                                    Console.WriteLine("Enter field tags: ");
                                    string fieldTags = "";
                                    fieldTags = Console.ReadLine();
                                    Console.WriteLine("Enter error level: ");
                                    int errorLevel = -1;
                                    int.TryParse(Console.ReadLine(), out errorLevel);
                                    Console.WriteLine("Enter message: ");
                                    string message = "";
                                    message = Console.ReadLine();
                                    Tests.CustomValidFields(fieldTags, errorLevel, message);
                                    break;

                                /************************* Test Custom Invalid Log **************************/
                                case "2":
                                    Console.WriteLine("Invalid field tags: \n\t(-idt)Invalid Date\n\t(-itm)Invalid Time\n\t(-idn)Invalid Device Name\n\t(-ian)Invalid Application Name\n\t(-ipi)Invalid Process ID\n\t(-iel)Invalid Error Level\n\t(-ims)Invalid Message");
                                    Console.WriteLine("Enter field tags: ");
                                    fieldTags = "";
                                    fieldTags = Console.ReadLine();
                                    Console.WriteLine("Enter error level: ");
                                    errorLevel = -1;
                                    int.TryParse(Console.ReadLine(), out errorLevel);
                                    Console.WriteLine("Enter message: ");
                                    message = "";
                                    message = Console.ReadLine();
                                    Tests.CustomInvalidFields(fieldTags, errorLevel, message);
                                    break;
                                /************************* Test Custom Mixed Log **************************/
                                case "3":
                                    Console.WriteLine("Field tags: \n\t(-dt)Date\n\t(-tm)Time\n\t(-dn)Device Name\n\t(-an)Application Name\n\t(-pi)Process ID\n\t(-el)Error Level\n\t(-ms)Message");
                                    Console.WriteLine("Enter field tags: ");
                                    fieldTags = "";
                                    fieldTags = Console.ReadLine();
                                    Console.WriteLine("Enter error level: ");
                                    errorLevel = -1;
                                    int.TryParse(Console.ReadLine(), out errorLevel);
                                    Console.WriteLine("Enter message: ");
                                    message = "";
                                    message = Console.ReadLine();
                                    Tests.CustomMixedFields(fieldTags, errorLevel, message);
                                    break;
                                /************************* Test All Valid Log Field Permutations **************************/
                                case "4":
                                    Console.WriteLine("Enter delay time(ms): ");
                                    int delay = 0;
                                    int.TryParse(Console.ReadLine(), out delay);
                                    Console.WriteLine("Enter number of logs to send: ");
                                    int logsToSend = 0;
                                    int.TryParse(Console.ReadLine(), out logsToSend);
                                    Console.WriteLine("Sending {0} logs with a {1}(ms) delay between each log", logsToSend, delay);
                                    Tests.AllValidPermutatedFields(delay, logsToSend, Logger.fieldTags, "All Valid Log Field Permutations. # of Logs:" +  logsToSend + " delay(ms):" + delay);
                                    break;

                                /************************* Test Invalid Log Field Permutations **************************/
                                case "5":
                                    Console.WriteLine("Enter delay time(ms): ");
                                    delay = 0;
                                    int.TryParse(Console.ReadLine(), out delay);
                                    Console.WriteLine("Enter number of logs to send: ");
                                    logsToSend = 0;
                                    int.TryParse(Console.ReadLine(), out logsToSend);
                                    Console.WriteLine("Sending {0} logs with a {1}(ms) delay between each log", logsToSend, delay);
                                    Tests.AllInvalidPermutatedFields(delay, logsToSend, InvalidLogger.invalidFieldTags, "All Invalid Log Field Permutations. # of Logs:" + logsToSend + " delay(ms):" + delay);
                                    break;

                                /************************* Test Mixed Field Permutations **************************/
                                case "6":
                                    Console.WriteLine("Enter delay time(ms): ");
                                    delay = 0;
                                    int.TryParse(Console.ReadLine(), out delay);
                                    Console.WriteLine("Enter number of logs to send: ");
                                    logsToSend = 0;
                                    int.TryParse(Console.ReadLine(), out logsToSend);
                                    Console.WriteLine("Sending {0} logs with a {1}(ms) delay between each log", logsToSend, delay);
                                    Tests.AllMixedPermutatedFields(delay, logsToSend, MixedLogger.fieldTags, "Mixed Valid and Invalid Log Field Permutations. # of Logs:" + logsToSend + " delay(ms):" + delay);
                                    break;

                                /************************* Test Regular Valid Log **************************/
                                case "7":
                                    Console.WriteLine("Enter error level: ");
                                    int.TryParse(Console.ReadLine(), out errorLevel);
                                    Console.WriteLine("Enter message: ");
                                    message = Console.ReadLine();

                                    Logger logger = new Logger(errorLevel, message);
                                    if (Tests.debugMode)
                                    {
                                        Console.WriteLine(Logger.WriteLog(errorLevel, message));
                                    }
                                    else
                                    {
                                        Client.Send(Logger.WriteLog(errorLevel, message));
                                    }
                                    break;

                                /************************* Test Regular Invalid Log **************************/
                                case "8":
                                    Console.WriteLine("Enter error level: ");
                                    int.TryParse(Console.ReadLine(), out errorLevel);
                                    Console.WriteLine("Enter message: ");
                                    message = Console.ReadLine();

                                    InvalidLogger invalidLogger = new InvalidLogger(errorLevel, message);
                                    if (Tests.debugMode)
                                    {
                                        Console.WriteLine(InvalidLogger.WriteLog(errorLevel, message));
                                    }
                                    else
                                    {
                                        Client.Send(InvalidLogger.WriteLog(errorLevel, message));
                                    }
                                    break;

                                /************************* Test Regular Mixed Log **************************/
                                case "9":
                                    Console.WriteLine("Enter error level: ");
                                    int.TryParse(Console.ReadLine(), out errorLevel);
                                    Console.WriteLine("Enter message: ");
                                    message = Console.ReadLine();

                                    MixedLogger mixedLogger = new MixedLogger(errorLevel, message);
                                    if (Tests.debugMode)
                                    {
                                        Console.WriteLine(MixedLogger.WriteLog(errorLevel, message));
                                    }
                                    else
                                    {
                                        Client.Send(MixedLogger.WriteLog(errorLevel, message));
                                    }
                                    break;

                                /************************* Test Error Levels **************************/
                                case "10":
                                    Console.WriteLine("Enter delay time(ms): ");
                                    delay = 0;
                                    int.TryParse(Console.ReadLine(), out delay);
                                    Console.WriteLine("Enter number of logs to send: ");
                                    logsToSend = 0;
                                    int.TryParse(Console.ReadLine(), out logsToSend);

                                    // Change error level globals
                                    Console.WriteLine("Enter invalid minimum error level: ");
                                    int.TryParse(Console.ReadLine(), out Tests.errorMin);
                                    Console.WriteLine("Enter invalid maximum error level: ");
                                    int.TryParse(Console.ReadLine(), out Tests.errorMax);
                                    if (Tests.errorMax < Tests.errorMin)
                                    {
                                        Console.WriteLine("Maximum invalid error level must be greater than minimum invalid error level");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Sending {0} logs with a {1}(ms) delay between each log and error level range: {2} - {3}", logsToSend, delay, Tests.errorMin, Tests.errorMax);
                                        Tests.AllValidPermutatedFields(delay, logsToSend, Logger.fieldTags, "Error level testing. # of Logs:" + logsToSend + " delay(ms):" + delay + " error level rage:" + Tests.errorMin + "-" + Tests.errorMax);
                                    }

                                    // Restore error level globals
                                    Tests.errorMin = Tests.ERROR_MIN;
                                    Tests.errorMax = Tests.ERROR_MAX;
                                    break;


                                default:
                                    break;
                            }
                            Console.WriteLine("Continue testing?");
                            Console.WriteLine("Press any key to continue, or press 'q' to return to main menu:");
                            if (Console.ReadLine() == "q")
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
