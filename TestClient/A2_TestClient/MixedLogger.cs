/*
* FILE : MixedLogger.cs
* PROJECT : SENG2040_A3 SERVICES AND LOGGING
* PROGRAMMER : Nathan Domingo
* FIRST VERSION : 2022-02-17
* DESCRIPTION : Contains functions for a Logger with both valid and invalid fields
*/
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;

namespace A2_TestClient
{

    public class MixedLogger
    {
        // Mixed Valid and Invalid log field variables
        public static string fieldTags { get; private set; } = "-dt -tm -dn -an -pi -el -ms"; 
        public string clientId { get; private set; } 
        public string invalidDate { get; private set; }
        public string time { get; private set; }
        public string invalidDevName { get; private set; }
        public string appName { get; private set; }
        public int invalidPId { get; private set; }
        public int errorLvl { get; private set; } 
        public string invalidMsg { get; private set; }

        /*
        * METHOD :MixedLogger()
        * 
        * PARAMETERS : int getErrorLvl, string getMsg
        * 
        * DESCRIPTION : Default constructor
        */
        public MixedLogger(int getErrorLvl, string getMsg)
        {
            MD5 md5 = MD5.Create();
            // Create clientId by combining appName and pId, converted into bytes, hash with MD5 and convert to hex string
            // Referenced https://stackoverflow.com/a/24031467
            string createclientId = appName + invalidPId.ToString();
            var clientIdBytes = new ASCIIEncoding().GetBytes(createclientId);
            var clientIdHash = md5.ComputeHash(clientIdBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < clientIdHash.Length; i++)
            {
                sb.Append(clientIdHash[i].ToString("X2"));
            }
            clientId = sb.ToString();

            // Valid fields
            time = DateTime.Now.ToString("hh:mm:ss");
            appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            // get error lvl
            if (getErrorLvl > Tests.errorMax || getErrorLvl < Tests.errorMin)
            {
                // Set to default if error level out of bounds
                getErrorLvl = Tests.errorMin;
            }
            errorLvl = getErrorLvl;

            // Invalid fields
            invalidDate = "f220/13/x211";
            invalidDevName = "-------";
            invalidPId = -123312;
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
            MixedLogger MixedLogger = new MixedLogger(0, "");
            string jsonString = JsonConvert.SerializeObject(MixedLogger);
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
            MixedLogger MixedLogger = new MixedLogger(errorLvl, msg);
            string jsonString = JsonConvert.SerializeObject(MixedLogger);
            return jsonString;
        }

        /*
        * METHOD : WriteCustomLog()
        *
        * DESCRIPTION : Writes a custom mixed log to a string for testing purposes. Refer to Log-Format.txt for field tags
        * 
        * PARAMETERS : string fields : A string containing all the field tags that are to be included in the custom log
        *              int errorLvl
        *              string msg
        *
        * RETURNS : string : All log fields requested, with each field starting with their corresponding identifier 
        */
        public static string WriteCustomLog(string fields, int errorLvl, string msg)
        {
            MixedLogger MixedLogger = new MixedLogger(errorLvl, msg);

            // Build string
            if (!fields.Contains("-dt"))
            {
                MixedLogger.invalidDate = null;
            }
            // Time
            if (!fields.Contains("-tm"))
            {
                MixedLogger.time = null;
            }
            // Device Name
            if (!fields.Contains("-dn"))
            {
                MixedLogger.invalidDevName = null;
            }
            // Application Name
            if (!fields.Contains("-an"))
            {
                MixedLogger.appName = null;
            }
            // Process id
            if (!fields.Contains("-pi"))
            {
                MixedLogger.invalidPId = -1;
            }

            // Error Level
            if (!fields.Contains("-el"))
            {
                MixedLogger.errorLvl = -1;
            }
            // Message
            if (!fields.Contains("-ms"))
            {
                MixedLogger.invalidMsg = null;
            }

            string jsonString = JsonConvert.SerializeObject(MixedLogger);
            return jsonString;
        }
    }
}
