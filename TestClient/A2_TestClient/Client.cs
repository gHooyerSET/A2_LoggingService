using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace A2_TestClient
{
    public static class Client
    {
        public static string ip = "127.0.0.1"; // Ip address of server
        public static int port = 8500; // Port of server

        public static int bufferSize = 1024; // Buffer size for sending log

        static byte[] byteBuffer = new byte[bufferSize]; // Buffer for sending log

        /*
        * METHOD : SendLog()
        *
        * DESCRIPTION : Attempts to connect to server by opening a socket and streaming a log message to server
        *               Referenced my own client funciton A5Client from Windows Programming 
        * 
        * PARAMETERS : string log
        *
        * RETURNS :NA
        */
        public static void Send(string log)
        {
            try
            {
                System.Net.Sockets.TcpClient tcpClient = new System.Net.Sockets.TcpClient(ip, port);
                NetworkStream networkStream = tcpClient.GetStream();

                // Send log message
                byteBuffer = System.Text.Encoding.ASCII.GetBytes(log);
                networkStream.Write(byteBuffer, 0, byteBuffer.Length);

                networkStream.Dispose();
                tcpClient.Close();

            }
            catch (Exception)
            {
                Console.WriteLine("\nError connectiong with Server\n");
            }
        }

    }
}
