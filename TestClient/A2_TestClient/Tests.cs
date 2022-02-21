using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2_TestClient
{
    public static class Tests
    {
        // debugMode = true: prints log instead of sending it to server
        public static bool debugMode = false; 

        /*
        * METHOD : CustomFields()
        *
        * DESCRIPTION : Sends custom combinations of valid inputted log fields
        * 
        * PARAMETERS : int delayTime : Time between each messsage in ms
        *              int numberOfMessages : Number of messages to send
        *
        * RETURNS :NA
        */
        public static void CustomValidFields(string fieldTags, int errorLevel, string message)
        {
            // Send log
            if (Tests.debugMode)
            {
                Console.WriteLine(Logger.WriteCustomLog(fieldTags, errorLevel, message));
            }
            else
            {
                Client.Send(Logger.WriteCustomLog(fieldTags, errorLevel, message));
            }
        }

        /*
        * METHOD : CustomInvalidFields()
        *
        * DESCRIPTION : Sends custom combinations of valid OR invalid inputted log fields
        * 
        * PARAMETERS : int delayTime : Time between each messsage in ms
        *              int numberOfMessages : Number of messages to send
        *
        * RETURNS :NA
        */
        public static void CustomInvalidFields(string fieldTags, int errorLevel, string message)
        {
            // Send log
            if (Tests.debugMode)
            {
                Console.WriteLine(invalidLogger.WriteCustomLog(fieldTags, errorLevel, message));
            }
            else
            {
                Client.Send(invalidLogger.WriteCustomLog(fieldTags, errorLevel, message));
            }
        }
       
        /*
        * METHOD : AllPermutatedFields()
        *
        * DESCRIPTION : Sends different permutations of inputted log fields
        * 
        * PARAMETERS : int delayTime : Time between each messsage in ms
        *              int numberOfMessages : Total number of messages to send
        *
        * RETURNS :NA
        */
        public static void AllValidPermutatedFields(int delayTime, int numberOfMessages, string inputTags, int errorLevel, string message)
        {
            string[] tags = inputTags.Split(' ');
            int numberOfTags = tags.Count();
            var rand = new Random();
            int numberOfFieldsToExlclude = 0;
            // Create a list with every permutaion of our field tags
            IList<IList<string>> tagPermutations = Permute(tags);
            int i = 0;
            foreach (var list in tagPermutations)
            {
                System.Threading.Thread.Sleep(delayTime);

                // Remove fields, increasing each permutation
                for (int count = 0; count < numberOfFieldsToExlclude; count++)
                {
                    list[count] = "";
                }

                //Send custom log
                CustomValidFields(string.Join("", list), errorLevel, message);

                numberOfFieldsToExlclude++;

                if(numberOfFieldsToExlclude == numberOfTags - 1)
                {
                    numberOfFieldsToExlclude = 0;
                }

                i++;

                if(i == numberOfMessages)
                {
                    break;
                }
            }
        }

        /*
        * METHOD : AllPermutatedFields()
        *
        * DESCRIPTION : Sends different permutations of inputted log fields
        * 
        * PARAMETERS : int delayTime : Time between each messsage in ms
        *              int numberOfMessages : Total number of messages to send
        *
        * RETURNS :NA
        */
        public static void AllInvalidPermutatedFields(int delayTime, int numberOfMessages, string inputTags, int errorLevel, string message)
        {
            string[] tags = inputTags.Split(' ');
            int numberOfTags = tags.Count();
            var rand = new Random();
            int numberOfFieldsToExlclude = 0;
            // Create a list with every permutaion of our field tags
            IList<IList<string>> tagPermutations = Permute(tags);
            int i = 0;
            foreach (var list in tagPermutations)
            {
                System.Threading.Thread.Sleep(delayTime);

                // Remove fields, increasing each permutation
                for (int count = 0; count < numberOfFieldsToExlclude; count++)
                {
                    list[count] = "";
                }

                //Send custom log
                CustomInvalidFields(string.Join("", list), errorLevel, message);

                numberOfFieldsToExlclude++;

                if (numberOfFieldsToExlclude == numberOfTags - 1)
                {
                    numberOfFieldsToExlclude = 0;
                }

                i++;

                if (i == numberOfMessages)
                {
                    break;
                }
            }
        }

        //https://www.chadgolden.com/blog/finding-all-the-permutations-of-an-array-in-c-sharp
        /*
        * METHOD : Permute
        *
        * DESCRIPTION : Creates a new list and fills it with permutated array
        * 
        * PARAMETERS : int[] nums
        *
        * RETURNS : IList<IList<int>> 
        */
        static IList<IList<string>> Permute(string[] fields)
        {
            var list = new List<IList<string>>();
            return DoPermute(fields, 0, fields.Length - 1, list);
        }

        //https://www.chadgolden.com/blog/finding-all-the-permutations-of-an-array-in-c-sharp
        /*
        * METHOD : DoPermute
        *
        * DESCRIPTION : Permutates a 1d array
        * 
        * PARAMETERS : int[] nums, int start, int end, IList<IList<int>> list
        *
        * RETURNS : IList<IList<int>> 
        */
        static IList<IList<string>> DoPermute(string[] fields, int start, int end, IList<IList<string>> list)
        {
            if (start == end)
            {
                // We have one of our possible n! solutions,
                // add it to the list.
                list.Add(new List<string>(fields));
            }
            else
            {
                for (var i = start; i <= end; i++)
                {
                    Swap(ref fields[start], ref fields[i]);
                    DoPermute(fields, start + 1, end, list);
                    Swap(ref fields[start], ref fields[i]);
                }
            }

            return list;
        }

        //https://www.chadgolden.com/blog/finding-all-the-permutations-of-an-array-in-c-sharp
        /*
        * METHOD : Swap
        *
        * DESCRIPTION :Swaps strings
        * 
        * PARAMETERS : ref int a, ref int b
        *
        * RETURNS : NA
        */
        static void Swap(ref string a, ref string b)
        {
            var temp = a;
            a = b;
            b = temp;
        }
    }
}
