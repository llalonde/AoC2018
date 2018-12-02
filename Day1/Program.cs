using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoCDay1
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            // Load frequency values
            var reader = new StreamReader("AoC-Day1-Input.txt");
            string frequencyInput = reader.ReadToEnd();
            List<int> frequencyValues = frequencyInput
                .Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                .Select(Int32.Parse)
                .ToList();

            // Part 1: Find frequency result
            int frequencyResult = frequencyValues.Sum();
            Console.WriteLine($"Frequency Result: {frequencyResult}");

            //Part 2: Find 1st occurrence of a duplicate frequency result
            int duplicateValue = FindDuplicateFrequencyResult(frequencyValues);
            Console.WriteLine($"Duplicate Frequency Result: {duplicateValue}");

        }

        static int FindDuplicateFrequencyResult(List<int> frequencyValues)
        {
            List<int> frequencyResult = new List<int>();
            HashSet<int> hs = new HashSet<int>();

            int currentFrequencyResult = 0;
            bool duplicateFrequencyFound = false;

            while (!duplicateFrequencyFound)
            {
                foreach (int frequencyValue in frequencyValues)
                {
                    currentFrequencyResult += frequencyValue;
                    frequencyResult.Add(currentFrequencyResult);

                    if (!hs.Add(currentFrequencyResult))
                    {
                        duplicateFrequencyFound = true;
                        break;
                    }
                }
            }

            return (duplicateFrequencyFound ? currentFrequencyResult : 0);
        }

    }
}
