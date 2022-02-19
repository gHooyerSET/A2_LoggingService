using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace A2_TestClient
{
    internal class TestClient
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("{0}", Logger.WriteLog(90000,"Error! Something went wrong!"));
            //Tests.TestFields(500, 10);
            //Console.ReadLine();
            //Client.Send(Logger.WriteLog(1, "hello"));
            //Tests.TestValidFields(1000, 100);
            // Run Console Ment
            bool displayMenu = true;
            while(displayMenu)
            {
                displayMenu = Menu.MainMenu();
            }
        }
    }
}
