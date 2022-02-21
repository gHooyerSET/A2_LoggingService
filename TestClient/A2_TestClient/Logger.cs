/*
* FILE : Logger.cs
* PROJECT : SENG2040_A3 SERVICES AND LOGGING
* PROGRAMMER : Nathan Domingo
* FIRST VERSION : 2022-02-17
* DESCRIPTION : Hilo Class contains the logic behind the game as well as data storage for
*               each user session.
*/
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;

namespace A2_TestClient
{
    enum ErrorCode
    {
        DEFAULT = -1,
        MAX = 50000,
    }

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

        // Invalid log field variables
        public static string invalidFieldTags { get; private set; } = "-idt -itm -idn -ian -ipi -iel -ims"; 
        public string invalidClientId { get; private set; } 
        public string invalidDate { get; private set; } 
        public string invalidTime { get; private set; } 
        public string invalidDevName { get; private set; }
        public string invalidAppName { get; private set; } 
        public int invalidPId { get; private set; } 
        public int invalidErrorLvl { get; private set; } 
        public string invalidMsg { get; private set; }

        /*
        * METHOD :Logger()
        *
        * DESCRIPTION : Default constructor with no Error Lvl or Message Arguments, they will be set to default
        */
        public Logger()
        {
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
            errorLvl = (int)ErrorCode.DEFAULT;

            // get message
            msg = "None";
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
            if(getErrorLvl > (int)ErrorCode.MAX || getErrorLvl < (int)ErrorCode.DEFAULT)
            {
                // Set to default if error level out of bounds
                getErrorLvl = (int)ErrorCode.DEFAULT;
            }
            errorLvl = getErrorLvl;

            // get message
            if(string.IsNullOrWhiteSpace(getMsg))
            {
                // Set to default if string is empty
                getMsg = "None";
            }
            msg = getMsg;

            /******************  invalid Data ******************/
            invalidClientId = clientId;
            invalidDate = "f220/13/x211";
            invalidTime = "5rfevc";
            invalidDevName = "-------";
            invalidPId = -123312;
            invalidErrorLvl = (int)ErrorCode.MAX + 1;
            invalidMsg = "/n/t/n/t";
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
        * METHOD : WriteCustomValidLog()
        *
        * DESCRIPTION : Writes a custom valid log to a string for testing purposes. Refer to Log-Format.txt for field tags
        * 
        * PARAMETERS : string fields : A string containing all the field tags that are to be included in the custom log
        *              int errorLvl
        *              string msg
        *
        * RETURNS : string : All log fields requested, with each field starting with their corresponding identifier 
        */
        public static string WriteCustomValidLog(string fields, int errorLvl, string msg)
        {
            Logger logger = new Logger(errorLvl, msg);

            // Build string
            StringBuilder logSb = new StringBuilder();
            logSb.Append(@"{");
            logSb.AppendFormat("\"clientID\":\"{0}\",", logger.clientId);
            if (fields.Contains("-dt"))
            {
                logSb.AppendFormat("\"date\":\"{0}\",", logger.date);
            }
            else
            {
                logSb.AppendFormat("\"date\":None,");
            }
            // Time
            if (fields.Contains("-tm"))
            {
                logSb.AppendFormat("\"time\":{0},", logger.time);
            }
            else
            {
                logSb.AppendFormat("\"time\":None,");
            }
            // Device Name
            if (fields.Contains("-dn"))
            {
                logSb.AppendFormat("\"devName\":{0},", logger.devName);
            }
            else
            {
                logSb.AppendFormat("\"devName\":None,");
            }
            // Application Name
            if (fields.Contains("-an"))
            {
                logSb.AppendFormat("\"appName\":{0},", logger.appName);
            }
            else
            {
                logSb.AppendFormat("\"appName\":None,");
            }
            // Process id
            if (fields.Contains("-pi"))
            {
                logSb.AppendFormat("\"pId\":{0},", logger.pId);
            }
            else
            {
                logSb.AppendFormat("\"pId\":None,");
            }
            // Error Level
            if (fields.Contains("-el"))
            {
                logSb.AppendFormat("\"errorLvl\":{0},", logger.errorLvl);
            }
            else
            {
                logSb.AppendFormat("\"errorLvl\":None,");
            }
            // Message
            if (fields.Contains("-ms"))
            {
                logSb.AppendFormat("\"msg\":{0},", logger.msg);
            }
            else
            {
                logSb.AppendFormat("\"msg\":None,");
            }
            // Remove final comma
            logSb.Length--;
            logSb.AppendLine(@"}");

            return logSb.ToString();
        }

        /*
        * METHOD : WriteCustomInvalidLog
        * 
        * PARAMETERS : string fields, int errorLvl, string msg
        * 
        * DESCRIPTION : Writes a custom invalid log. Log can contain valid and invalid fields
        */
        public static string WriteCustomInvalidLog(string fields, int errorLvl, string msg)
        {
            Logger logger = new Logger(errorLvl, msg);

            // Build string
            StringBuilder logSb = new StringBuilder();
            logSb.Append(@"{");
            // clientID : is constant
            logSb.AppendFormat("\"InvalidClientID\":{0},", logger.invalidClientId);
            // Date
            if (fields.Contains("-idt"))
            {
                logSb.AppendFormat("\"invalidDate\":{0},", logger.invalidDate);
            }
            else
            {
                logSb.AppendFormat("\"invalidDate\":None,");
            }
            if (fields.Contains("-dt"))
            {
                logSb.AppendFormat("\"date\":{0},", logger.date);
            }
            else
            {
                logSb.AppendFormat("\"date\":None,");
            }
            // Time
            if (fields.Contains("-itm"))
            {
                logSb.AppendFormat("\"invalidTime\":{0},", logger.invalidTime);
            }
            else
            {
                logSb.AppendFormat("\"invalidTime\":None,");
            }
            if (fields.Contains("-tm"))
            {
                logSb.AppendFormat("\"time\":\"{0}\",", logger.time);
            }
            else
            {
                logSb.AppendFormat("\"time\":None,");
            }
            // Device Name
            if (fields.Contains("-idn"))
            {
                logSb.AppendFormat("\"invalidDevName\":{0},", logger.invalidDevName);
            }
            else
            {
                logSb.AppendFormat("\"invalidDevName\":None,");
            }
            if (fields.Contains("-dn"))
            {
                logSb.AppendFormat("\"devName\":\"{0}\",", logger.devName);
            }
            else
            {
                logSb.AppendFormat("\"devName\":None,");
            }
            // Application Name
            if (fields.Contains("-ian"))
            {
                logSb.AppendFormat("\"invalidAppName\":{0},", logger.invalidAppName);
            }
            else
            {
                logSb.AppendFormat("\"invalidAppName\":None,");
            }
            if (fields.Contains("-an"))
            {
                logSb.AppendFormat("\"appName\":\"{0}\",", logger.appName);
            }
            else
            {
                logSb.AppendFormat("\"appName\":None,");
            }
            // Process id
            if (fields.Contains("-ipi"))
            {
                logSb.AppendFormat("\"invalidPId\":{0},", logger.invalidPId);
            }
            else
            {
                logSb.AppendFormat("\"invalidPId\":None,");
            }
            if (fields.Contains("-pi"))
            {
                logSb.AppendFormat("\"pId\":{0},", logger.pId);
            }
            else
            {
                logSb.AppendFormat("\"pId\":None,");
            }
            // Error Level
            if (fields.Contains("-iel"))
            {
                logSb.AppendFormat("\"invalidErrorLvl\":{0},", logger.invalidErrorLvl);
            }
            else
            {
                logSb.AppendFormat("\"invalidErrorLvl\":None,");
            }
            if (fields.Contains("-el"))
            {
                logSb.AppendFormat("\"errorLvl\":{0},", logger.errorLvl);
            }
            else
            {
                logSb.AppendFormat("\"errorLvl\":None,");
            }
            // Message
            if (fields.Contains("-ims"))
            {
                logSb.AppendFormat("\"invalidMsg\":{0},", logger.invalidMsg);
            }
            else
            {
                logSb.AppendFormat("\"invalidMsg\":None,");
            }
            if (fields.Contains("-ms"))
            {
                logSb.AppendFormat("\"msg\":\"{0}\",", logger.msg);
            }
            else
            {
                logSb.AppendFormat("\"msg\":None,");
            }
            // Remove final comma
            logSb.Length--;
            logSb.AppendLine(@"}");

            return logSb.ToString();
        }
    }
}
