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
            Console.WriteLine("{0}", Logger.WriteCustomLog("-lg -dt -tm", 1, "hello"));
            Tests.TestFields(500, 10);
            Console.ReadLine();
            //Client.Send(Logger.WriteLog(1, "hello"));
        }
    }
}
