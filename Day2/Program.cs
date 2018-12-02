using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoCDay2
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            // Load frequency values
            var reader = new StreamReader("AoC-Day2-Input.txt");
            string candidateIdInput = reader.ReadToEnd();
            string[] idValues = candidateIdInput
                .Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            // Part 1: Find checksum of repeating characters in id
            int checksum = GetCandidateIdCheckSum(idValues);
            Console.WriteLine($"Checksum: {checksum}");

            // Part 2: Find candidate Ids that match with the exception of single letter 
            // and return the set of common characters in those Ids
            string commonCharacters = FindCommonCharacters(idValues);
            Console.Write($"Common characters: {commonCharacters}");

        }

        static int GetCandidateIdCheckSum(string[] idValues)
        {
            int twoRepeatedLetterCount = 0;
            int threeRepeatedLetterCount = 0;

            foreach (string id in idValues)
            {
                // get letters that repeat exactly twice within a single Id
                var twoRepeatedLetterIds = id.ToCharArray()
                    .GroupBy(x => x)
                    .Where(y => y.Count() == 2)
                    .Select(z => z.Key);

                // get letters that repeat exactly three times within a single Id
                var threeRepeatedLetterIds = id.ToCharArray()
                    .GroupBy(x => x)
                    .Where(y => y.Count() == 3)
                    .Select(z => z.Key);

                // if one or more sets of two repeated letters are found, add one for checksum calculation
                if (twoRepeatedLetterIds.Any())
                    twoRepeatedLetterCount += 1;

                // if one or more sets of three repeated letters are found, add one for checksum calculation
                if (threeRepeatedLetterIds.Any())
                    threeRepeatedLetterCount += 1;
            }

            return (twoRepeatedLetterCount * threeRepeatedLetterCount);
        }

        static string FindCommonCharacters(string[] idValues)
        {
            string commonCharactersList = string.Empty;

            foreach(string x1 in idValues)
            {
                foreach (string x2 in idValues)
                {
                    // enumerate list of characters to compare from both strings
                    var charsToCompare = x1.Zip(x2, (x1Char, x2Char) => new { x1Char, x2Char });

                    // select all characters that match at the same index in both strings
                    var matchingChars = charsToCompare.Where(pair => pair.x1Char == pair.x2Char).Select(c => c.x1Char);


                    // if the strings differ by 1 character, return the set of common characters between the two strings
                    if (matchingChars.Count() == x1.Length - 1)
                    {
                        commonCharactersList = string.Join("", matchingChars);
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(commonCharactersList))
                    break;
            }            

            return commonCharactersList;
        }

    }
}
