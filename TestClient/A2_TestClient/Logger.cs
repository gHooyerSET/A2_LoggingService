/*
* FILE : Logger.cs
* PROJECT : SENG2040_A3 SERVICES AND LOGGING
* PROGRAMMER : Nathan Domingo
* FIRST VERSION : 2022-02-17
* DESCRIPTION : Hilo Class contains the logic behind the game as well as data storage for
*               each user session.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2_TestClient
{
    // Enumerations for Log Field positions
    enum LogField
    {
        LOG,
        DATE,
        TIME,
        DEVICE_NAME,
        APPLICATION_NAME,
        PROCESS_ID,
        ERROR_LEVEL,
        MESSAGE
    }

    public class Logger
    {
        public bool isLog { get; private set; } // Message header that is true if message is a log
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
        * DESCRIPTION : Default constructor
        */
        public Logger(int getErrorLvl, string getMsg) 
        {
            isLog = true;

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
            errorLvl = getErrorLvl;

            // get message
            msg = getMsg;
        }

        /*
        * METHOD : WriteLog()
        *
        * DESCRIPTION : Writes the current instance of log to a string
        * 
        * PARAMETERS : NA
        *
        * RETURNS : string : All log fields appended in order, with each field starting with their corresponding identifier 
        */
        public string WriteLog()
        {
            StringBuilder logSb = new StringBuilder();
            logSb.AppendFormat("-lg{0} -dt{1} -tm{2} -dn{3} -an{4} -pi{5} -el{6} -ms{7}", isLog, date, time, devName, appName, pId, errorLvl, msg);
            return logSb.ToString();
        }
    }
}
