using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;

namespace AoCDay3
{
    class MainClass
    {
        public class Claim
        {
            public int Id { get; set; }
            public Rectangle Rect { get; set; }
            public bool HasOverlappingCoordinates { get; set; }
        }
         
        static List<Claim> ClaimsList { get; set; }
        static Dictionary<string, int> CoordinatesList { get; set; }

        public static void Main(string[] args)
        {

            LoadClaims();

            // Part 1: Get number of points that overlap
            var overlappingPoints = GetNumberOfOverlappingPoints();
            Console.WriteLine($"Overlapping Points Count: {overlappingPoints}");

            // Part 2: Get non-overlapping claim Id
            var nonOverlappingClaimId = GetNonOverlappingClaimId();
            Console.WriteLine($"Non-overlapping point: {nonOverlappingClaimId}");

        }

        static void LoadClaims()
        {
            ClaimsList = new List<Claim>();

            var reader = new StreamReader("AoC-Day3-Input.txt");
            string claimIdInput = reader.ReadToEnd();
            string[] claims = claimIdInput
                .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string claim in claims)
            {
                string[] claimDetails = claim.Split(new Char[] { '@', ',', ':', 'x' }, StringSplitOptions.None);
                int id = int.Parse(claimDetails[0].Replace('#', ' '));

                Rectangle rect = new Rectangle(
                    int.Parse(claimDetails[1]),
                    int.Parse(claimDetails[2]),
                    int.Parse(claimDetails[3]),
                    int.Parse(claimDetails[4])
                    );

                ClaimsList.Add(new Claim { Id = id, Rect = rect });
            }
        }

        static int GetNumberOfOverlappingPoints()
        {
            int overlappingPoints = 0;
            CoordinatesList = new Dictionary<string, int>();

            foreach (Claim claim in ClaimsList)
            {
                for (int x = claim.Rect.Top; x < claim.Rect.Top + claim.Rect.Height; x++)
                {
                    for (int y = claim.Rect.Left; y < claim.Rect.Left + claim.Rect.Width; y++)
                    {
                        var coordinates = $"{x},{y}";
                        if (CoordinatesList.ContainsKey(coordinates))
                        {
                            CoordinatesList[coordinates]++;
                            if (CoordinatesList[coordinates] == 2)
                            {
                                overlappingPoints++;
                            }
                        }
                        else
                        {
                            CoordinatesList.Add(coordinates, 1);
                        }
                    }
                }
            }

            return overlappingPoints;
        }

        static int GetNonOverlappingClaimId()
        {
            foreach (Claim claim in ClaimsList)
            {
                for (int x = claim.Rect.Top; x < claim.Rect.Top + claim.Rect.Height; x++)
                {
                    for (int y = claim.Rect.Left; y < claim.Rect.Left + claim.Rect.Width; y++)
                    {
                        var coordinates = $"{x},{y}";

                        if (!claim.HasOverlappingCoordinates)
                            claim.HasOverlappingCoordinates = (CoordinatesList.ContainsKey(coordinates) 
                                                                && CoordinatesList[coordinates] != 1);

                    }
                }
            }

            int nonOverlappingClaimId = 0;

            Claim nonOverlappingClaim = ClaimsList.Where(c => !c.HasOverlappingCoordinates).FirstOrDefault();
            if (nonOverlappingClaim != null)
                nonOverlappingClaimId = nonOverlappingClaim.Id;

            return nonOverlappingClaimId;
        }
    }
}
