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
        public static string fieldTags { get; private set; } = "-dt -tm -dn -an -pi -el -ms"; // Contains all field tags
        public string logId { get; private set; } // Message containing the log id, an MD5 hash of the device name and process ID
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
        * DESCRIPTION : Default constructor with no Error Lvl or Message Arguments, they will be set tio default
        */
        public Logger()
        {
            MD5 md5 = MD5.Create();
            // Create logId by combining appName and pId, converted into bytes, hash with MD5 and convert to hex string
            // Referenced https://stackoverflow.com/a/24031467
            string createLogId = appName + pId.ToString();
            var logIdBytes = new ASCIIEncoding().GetBytes(createLogId);
            var logIdHash = md5.ComputeHash(logIdBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < logIdHash.Length; i++)
            {
                sb.Append(logIdHash[i].ToString("X2"));
            }
            logId = sb.ToString();

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
        * METHOD :Logger()
        *
        * DESCRIPTION : Default constructor
        */
        public Logger(int getErrorLvl, string getMsg) 
        {
            MD5 md5 = MD5.Create();
            // Create logId by combining appName and pId, converted into bytes, hash with MD5 and convert to hex string
            // Referenced https://stackoverflow.com/a/24031467
            string createLogId = appName + pId.ToString();
            var logIdBytes = new ASCIIEncoding().GetBytes(createLogId);
            var logIdHash = md5.ComputeHash(logIdBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < logIdHash.Length; i++)
            {
                sb.Append(logIdHash[i].ToString("X2"));
            }
            logId = sb.ToString();

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
        * PARAMETERS : NA
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
        * DESCRIPTION : Writes a custom log to a string for testing purposes. Refer to Log-Format.txt for field tags
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
            StringBuilder logSb = new StringBuilder();
            logSb.Append(@"{");
            logSb.AppendFormat("\"clientID\":{0},", logger.logId);
            if (fields.Contains("-dt"))
            {
                logSb.AppendFormat("\"date\":{0},", logger.date);
            }
            else
            {
                logSb.AppendFormat("\"date\":None,");
            }
            if (fields.Contains("-tm"))
            {
                logSb.AppendFormat("\"time\":{0},", logger.time);
            }
            else
            {
                logSb.AppendFormat("\"time\":None,");
            }
            if (fields.Contains("-dn"))
            {
                logSb.AppendFormat("\"devName\":{0},", logger.devName);
            }
            else
            {
                logSb.AppendFormat("\"devName\":None,");
            }
            if (fields.Contains("-pi"))
            {
                logSb.AppendFormat("\"appName\":{0},", logger.appName);
            }
            else
            {
                logSb.AppendFormat("\"appName\":None,");
            }
            if (fields.Contains("-el"))
            {
                logSb.AppendFormat("\"pId\":{0},", logger.pId);
            }
            else
            {
                logSb.AppendFormat("\"pId\":None,");
            }
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
    }
}
