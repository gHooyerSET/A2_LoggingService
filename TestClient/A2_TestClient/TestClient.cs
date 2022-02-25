/*
* FILE : TestClient.cs
* PROJECT : SENG2040_A3 SERVICES AND LOGGING
* PROGRAMMER : Nathan Domingo
* FIRST VERSION : 2022-02-17
* DESCRIPTION : Driver for our test client
*/
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
            bool displayMenu = true;
            while(displayMenu)
            {
                displayMenu = Menu.MainMenu();
            }
        }
    }
}
