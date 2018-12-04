using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoCDay4
{
    class MainClass
    {
        public enum GuardStatus
        {
            Awake,
            OnDuty,
            Asleep
        }

        public class Guard
        {
            public int GuardId { get; set; }
            public int TotalSleepMinutes { get; set; }
            public int SleepiestMinute { get; set; }
            public int SleepiestMinuteRecurrence { get; set; }
            public Dictionary<int, int> SleepMinutes { get; set; }

        }

        public class GuardDetails
        {
            public int GuardId;
            public DateTime CurrentTime { get; set; }
            public GuardStatus CurrentStatus { get; set; }
        }

        static List<GuardDetails> GuardDetailsList { get; set; }
        static List<Guard> Guards { get; set; }

        public static void Main(string[] args)
        {
            // Load shift details from input file
            LoadShiftDetails();

            // Part 1: Find guard that has the most minutes of sleep, 
            // then find the minute that the guard sleeps during most often 
            // and calculate Guard Id * sleep minute (max recurrence)

            // Step 1: load sleep minute occurrences into SleepMinutes dictionary for each guard 
            LoadGuardSleepMinuteOccurrences();

            // Step 2: Get the guard that sleeps the most
            Guard sleepyGuard = Guards.OrderByDescending(g => g.TotalSleepMinutes).First();

            // Step 3: Get the minute which the guard sleeps during most often
            LoadGuardSleepiestMinute(sleepyGuard);

            // Step 4: Calculate Guard Id * sleep minute with most recurrence
            int sleepyGuardCalculationResult = sleepyGuard.GuardId * sleepyGuard.SleepiestMinute;
            Console.WriteLine($"Guard Id ({sleepyGuard.GuardId}) * Sleepiest Minute ({sleepyGuard.SleepiestMinute}) = {sleepyGuardCalculationResult}");

            // Part 2: Get the minute on which a guard has slept during the most often 
            // and calculate Guard Id * sleep minute (max recurrence)
            // Step 1: Load sleepiest minute and recurrence for each guard
            Guards.ForEach(LoadGuardSleepiestMinute);

            // Step 2: Get most frequent sleeping minute
            Guard guardSleepiestMinute = Guards.OrderByDescending(g => g.SleepiestMinuteRecurrence).First();

            // Step 3: Calculate Guard Id * sleep minute with most recurrence
            int guardSleepiestMinuteCalculationResult = guardSleepiestMinute.GuardId * guardSleepiestMinute.SleepiestMinute;
            Console.WriteLine($"Guard Id ({guardSleepiestMinute.GuardId}) * Sleepiest Minute ({guardSleepiestMinute.SleepiestMinute}) = {guardSleepiestMinuteCalculationResult}");

        }

        static void LoadShiftDetails()
        {
            GuardDetailsList = new List<GuardDetails>();
            Guards = new List<Guard>();

            var reader = new StreamReader("AoC-Day4-Input.txt");
            string guardInput = reader.ReadToEnd();
            string[] guardDetails = guardInput
                .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string detail in guardDetails)
            {
                string[] details = detail.Split(new string[] { "] ", "Guard #", " begins" }, StringSplitOptions.RemoveEmptyEntries);

                int guardId = (details.Length > 2) ? int.Parse(details[1]) : 0;

                GuardStatus status = (guardId > 0 ? 
                                     GuardStatus.OnDuty :
                                     details[1] == "wakes up" ?
                                         GuardStatus.Awake : 
                                         GuardStatus.Asleep);

                GuardDetails currentGuardDetails = new GuardDetails
                {
                    CurrentTime = DateTime.Parse(details[0].Replace("[15", "20")),
                    GuardId = guardId,
                    CurrentStatus = status

                };

                if (Guards.FirstOrDefault(g => g.GuardId == guardId) == null)
                {
                    Guards.Add(new Guard { GuardId = guardId, SleepMinutes = new Dictionary<int, int>() });
                }

                GuardDetailsList.Add(currentGuardDetails);

                // sort by date/time field
                GuardDetailsList = GuardDetailsList.OrderBy(x => x.CurrentTime).ToList();
            }
        }

        static void LoadGuardSleepMinuteOccurrences()
        {
            int id = 0;
            DateTime wakeTime = DateTime.MinValue;
            DateTime sleepTime = DateTime.MinValue;

            foreach (GuardDetails guard in GuardDetailsList)
            {

                if (guard.GuardId == 0)
                {
                    guard.GuardId = id;

                    if (guard.CurrentStatus == GuardStatus.Asleep)
                    {
                        IncrementSleepMinuteOccurrence(guard.GuardId, guard.CurrentTime.Minute);

                        sleepTime = guard.CurrentTime;
                        wakeTime = DateTime.MinValue;
                    }
                    else
                    {
                        if (wakeTime == DateTime.MinValue)
                        {
                            wakeTime = guard.CurrentTime;
                            if (sleepTime != DateTime.MinValue)
                            {
                                int sleepMinutes = wakeTime.Subtract(sleepTime).Minutes;
                                for (int i = 1; i < sleepMinutes; i++)
                                {
                                    IncrementSleepMinuteOccurrence(guard.GuardId, sleepTime.AddMinutes(i).Minute);
                                }

                                sleepTime = DateTime.MinValue;
                            }
                        }
                    }
                }
                else
                {
                    id = guard.GuardId;
                }
            }
        }

        static void IncrementSleepMinuteOccurrence(int guardId, int minute)
        {
            Guard guard = Guards.Single(g => g.GuardId == guardId);
            if (guard.SleepMinutes.ContainsKey(minute))
            {
                guard.SleepMinutes[minute]++;
            }
            else
            {
                guard.SleepMinutes.Add(minute, 1);
            }

            guard.TotalSleepMinutes++;
        }

        static void LoadGuardSleepiestMinute(Guard guard)
        {
            guard.SleepiestMinute = guard.SleepMinutes
                                        .OrderByDescending(c => c.Value)
                                        .FirstOrDefault()
                                        .Key;

            if (guard.SleepMinutes.ContainsKey(guard.SleepiestMinute))
            {
                guard.SleepiestMinuteRecurrence = guard.SleepMinutes[guard.SleepiestMinute];
            }
        }
    }
}
