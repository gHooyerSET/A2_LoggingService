/*
* FILE : InvalidLogger.cs
* PROJECT : SENG2040_A3 SERVICES AND LOGGING
* PROGRAMMER : Nathan Domingo
* FIRST VERSION : 2022-02-17
* DESCRIPTION : Contains functions for a Logger with invalid fields
*/
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;

namespace A2_TestClient
{

    public class InvalidLogger
    {

        /* Invalid log field variables */
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
        * METHOD :InvalidLogger()
        * 
        * PARAMETERS : int getErrorLvl, string getMsg
        * 
        * DESCRIPTION : Default constructor
        */
        public InvalidLogger(int getErrorLvl, string getMsg)
        {
            MD5 md5 = MD5.Create();
            // Create clientId by combining appName and pId, converted into bytes, hash with MD5 and convert to hex string
            // Referenced https://stackoverflow.com/a/24031467
            string createclientId = invalidAppName + invalidPId.ToString();
            var clientIdBytes = new ASCIIEncoding().GetBytes(createclientId);
            var clientIdHash = md5.ComputeHash(clientIdBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < clientIdHash.Length; i++)
            {
                sb.Append(clientIdHash[i].ToString("X2"));
            }
            invalidClientId = sb.ToString();

            invalidDate = "f220/13/x211";
            invalidTime = "5rfevc";
            invalidDevName = "-------";
            invalidAppName = "Invalid_App";
            invalidPId = -123312;
            invalidErrorLvl = Tests.errorMax + 1;
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
            InvalidLogger InvalidLogger = new InvalidLogger(0, "");
            string jsonString = JsonConvert.SerializeObject(InvalidLogger);
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
            InvalidLogger InvalidLogger = new InvalidLogger(errorLvl, msg);
            string jsonString = JsonConvert.SerializeObject(InvalidLogger);
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
        public static string WriteCustomLog(string fields, int errorLvl, string msg)
        {
            InvalidLogger InvalidLogger = new InvalidLogger(errorLvl, msg);

            // Build string
            if (!fields.Contains("-idt"))
            {
                InvalidLogger.invalidDate = null;
            }
            // Time
            if (!fields.Contains("-itm"))
            {
                InvalidLogger.invalidTime = null;
            }
            // Device Name
            if (!fields.Contains("-idn"))
            {
                InvalidLogger.invalidDevName = null;
            }
            // Application Name
            if (!fields.Contains("-ian"))
            {
                InvalidLogger.invalidAppName = null;
            }
            // Process id
            if (!fields.Contains("-ipi"))
            {
                InvalidLogger.invalidPId = -1;
            }

            // Error Level
            if (!fields.Contains("-iel"))
            {
                InvalidLogger.invalidErrorLvl = -1;
            }
            // Message
            if (!fields.Contains("-ims"))
            {
                InvalidLogger.invalidMsg = null;
            }

            string jsonString = JsonConvert.SerializeObject(InvalidLogger);
            return jsonString;
        }
    }
}
