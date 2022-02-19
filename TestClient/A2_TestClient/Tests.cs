using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2_TestClient
{
    public static class Tests
    {
        /*
        * METHOD : TestFields()
        *
        * DESCRIPTION : Sends different combinations of our log fields
        * 
        * PARAMETERS : int delayTime : Time between each messsage in ms
        *              int numberOfMessages : Number of messages to send
        *
        * RETURNS :NA
        */
        public static void TestFields(int delayTime, int numberOfMessages)
        {
            System.Threading.Thread.Sleep(delayTime);
            string[] tags = Logger.fieldTags.Split(' ');
            int numberOfTags = tags.Count();
            var rand = new Random();

            for (int i = 0; i < numberOfMessages; i++)
            {
               // Console.WriteLine(Logger.WriteCustomLog(finalTags, 0, ""));
            }
        }

    }
}
