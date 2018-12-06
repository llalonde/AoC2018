using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;

namespace AoCDay5
{
    class MainClass
    {

        public static void Main(string[] args)
        {
            var reader = new StreamReader("AoC-Day5-Input.txt");
            string input = reader.ReadToEnd().Trim();

            //Part 1: Find total number of units after reacting polymer  
            Stack<string> polymerUnits = FindPolymerUnits(input);
            Console.WriteLine($"# of Polymers: {polymerUnits.Count}");

            //Part 2: Find polymer with least number of units after cleaning and reacting polymer
            var optimalPolymerUnit = FindOptimalPolymerUnit(input);
            Console.WriteLine($"Optimal Polymer Unit: {optimalPolymerUnit.Key}, Number: {optimalPolymerUnit.Value}");
        }

        static Stack<string> FindPolymerUnits(string input)
        {
            string lastItem;
            char[] inputArray = input.ToCharArray();
            Stack<string> output = new Stack<string>();

            foreach (char c in inputArray)
            {
                string letter = c.ToString();
                if (output.Any())
                {
                    lastItem = output.Peek();

                    // check to see if the two letters are the same but different case
                    if (string.Compare(lastItem, letter, true) == 0 &&
                        string.Compare(lastItem, letter, false) != 0)
                    {
                        output.Pop();
                    }
                    else
                    {
                        output.Push(letter);
                    }
                }
                else
                {
                    output.Push(letter);
                }
            }

            return output;
        }

        static KeyValuePair<string,int> FindOptimalPolymerUnit(string input)
        {
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            string replacement = "";
            string cleanedInput;

            Dictionary<string,int> polymerDict = new Dictionary<string, int>();
            char[] polymerArray = alphabet.ToCharArray();

            for (int i = 0; i < polymerArray.Length; i++)
            {
                string letter = polymerArray[i].ToString();
                cleanedInput = Regex.Replace(input, letter, replacement, RegexOptions.IgnoreCase);

                Stack<string> cleanStack = FindPolymerUnits(cleanedInput);
                polymerDict[letter] = cleanStack.Count;
            }

            return polymerDict.OrderBy(x => x.Value).First();
        }
    }
}
