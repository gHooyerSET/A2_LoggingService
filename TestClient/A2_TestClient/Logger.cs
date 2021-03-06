/*
* FILE : Logger.cs
* PROJECT : SENG2040_A3 SERVICES AND LOGGING
* PROGRAMMER : Nathan Domingo
* FIRST VERSION : 2022-02-17
* DESCRIPTION : Contains functions for a Logger with valid fields
*/
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;



namespace A2_TestClient
{

    public class Logger
    {
        // Valid log field variables
        public static string fieldTags { get; private set; } = "-dt -tm -dn -an -pi -el -ms"; // Contains all customizeable field tags
        public string clientId { get; private set; } // Message containing the client id, an MD5 hash of the device name and process ID
        public string date { get; private set; }  // A string containing the date (dd/mm/yy)
        public string time { get; private set; }  // A string containing the time (hh:mm:ss)
        public string devName { get; private set; } // A string containg the device/computer name
        public string appName { get; private set; } // A string containing the application source
        public int pId { get; private set; } // An interger containg the Process ID
        public int errorLvl { get; private set; } // An integer denoting the level and severity of error : (0-10,000 Debug)(0-20,000 Info)(0-30,000 Warning)(0-40,000 Error)(0-50,000 Fatal)
        public string msg { get; private set; } // A string containing the log message

        /*
        * METHOD :Logger()
        *
        * DESCRIPTION : Default constructor with no Error Lvl or Message Arguments, they will be set to default
        */
        public Logger()
        {
            MD5 md5 = MD5.Create();            

            // Get todays date and time
            date = DateTime.Now.ToString("dd/mm/yyyy");
            time = DateTime.Now.ToString("hh:mm:ss");

            // Get device name
            devName = System.Environment.MachineName;

            // Get app name
            appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

            // get process ID
            pId = System.Diagnostics.Process.GetCurrentProcess().Id;

            // set error lvl to default
            errorLvl = Tests.errorMin;

            // get message
            msg = "None";

            // Create clientId by combining appName and pId, converted into bytes, hash with MD5 and convert to hex string
            // Referenced https://stackoverflow.com/a/24031467
            string createclientId = appName + pId.ToString();
            var clientIdBytes = new ASCIIEncoding().GetBytes(createclientId);
            var clientIdHash = md5.ComputeHash(clientIdBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < clientIdHash.Length; i++)
            {
                sb.Append(clientIdHash[i].ToString("X2"));
            }
            clientId = sb.ToString();
        }

        /*
        * METHOD :Logger() OVERLOADED
        * 
        * PARAMETERS : int getErrorLvl, string getMsg
        * 
        * DESCRIPTION : Default constructor
        */
        public Logger(int getErrorLvl, string getMsg) 
        {
            // Get todays date and time
            date = DateTime.Now.ToString("dd/mm/yyyy");
            time = DateTime.Now.ToString("hh:mm:ss");

            // Get device name
            devName = System.Environment.MachineName;

            // Get app name
            appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

            // get process ID
            pId = System.Diagnostics.Process.GetCurrentProcess().Id;

            // get error lvl
            if(getErrorLvl > Tests.errorMax || getErrorLvl < Tests.errorMin)
            {
                // Set to default if error level out of bounds
                getErrorLvl = Tests.errorMin;
            }
            errorLvl = getErrorLvl;

            // get message
            if(string.IsNullOrWhiteSpace(getMsg))
            {
                // Set to default if string is empty
                getMsg = "None";
            }
            msg = getMsg;

            /******************  Valid Data ******************/
            MD5 md5 = MD5.Create();
            // Create clientId by combining appName and pId, converted into bytes, hash with MD5 and convert to hex string
            // Referenced https://stackoverflow.com/a/24031467
            string createclientId = appName + pId.ToString();
            var clientIdBytes = new ASCIIEncoding().GetBytes(createclientId);
            var clientIdHash = md5.ComputeHash(clientIdBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < clientIdHash.Length; i++)
            {
                sb.Append(clientIdHash[i].ToString("X2"));
            }
            clientId = sb.ToString();
        }

        /*
       * METHOD : WriteLog()
       *
       * DESCRIPTION : Writes the current instance of log to a string without an error level or message
       * 
       * PARAMETERS : NA
       *
       * RETURNS : string : All log fields appended in order, with each field starting with their corresponding identifier 
       */
        public static string WriteLog()
        {
            Logger logger = new Logger(0, "");
            string jsonString = JsonConvert.SerializeObject(logger);
            return jsonString;
        }

        /*
        * METHOD : WriteLog() OVERLOADED
        *
        * DESCRIPTION : Writes the current instance of log to a string with error level and message
        * 
        * PARAMETERS : int errorLvl, string msg
        *
        * RETURNS : string : All log fields appended in order, with each field starting with their corresponding identifier 
        */
        public static string WriteLog(int errorLvl, string msg)
        {
            Logger logger = new Logger(errorLvl, msg);
            string jsonString = JsonConvert.SerializeObject(logger);
            return jsonString;
        }

        /*
        * METHOD : WriteCustomLog()
        *
        * DESCRIPTION : Writes a custom valid log to a string for testing purposes. Refer to Log-Format.txt for field tags
        * 
        * PARAMETERS : string fields : A string containing all the field tags that are to be included in the custom log
        *              int errorLvl
        *              string msg
        *
        * RETURNS : string : All log fields requested, with each field starting with their corresponding identifier 
        */
        public static string WriteCustomLog(string fields, int errorLvl, string msg)
        {
            Logger logger = new Logger(errorLvl, msg);

            // Build string
            if (!fields.Contains("-dt"))
            {
                logger.date = null;
            }
            // Time
            if (!fields.Contains("-tm"))
            {
                logger.time = null;
            }
            // Device Name
            if (!fields.Contains("-dn"))
            {
                logger.devName = null;
            }
            // Application Name
            if (!fields.Contains("-an"))
            {
                logger.appName = null;
            }
            // Process id
            if (!fields.Contains("-pi"))
            {
                logger.pId = -1;
            }

            // Error Level
            if (!fields.Contains("-el"))
            {
                logger.errorLvl = -1;
            }
            // Message
            if (!fields.Contains("-ms"))
            {
                logger.msg = null;
            }

            string jsonString = JsonConvert.SerializeObject(logger);
            return jsonString;
        }
    }
}
