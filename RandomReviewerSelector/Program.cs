using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace RandomReviewerSelector
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = string.Empty;
            while (input != "exit")
            {
                var availableReviewers = ConfigurationManager.AppSettings["AvailableReviewers"].Split(';').ToList();
                int reviewersCount;
                if (!int.TryParse(input, out reviewersCount) || reviewersCount > availableReviewers.Count)
                {
                    Console.WriteLine(
                        $"Please specify reviewers count, should be less than or equal to {availableReviewers.Count}.");
                    Console.WriteLine("If you want to exit, type \"exit\".");
                    input = Console.ReadLine();
                    continue;
                }

                var reviewers = new List<string>(availableReviewers.Count);
                var random = new Random();
                while (reviewersCount != 0)
                {
                    var reviewWinnerIndex = random.Next(availableReviewers.Count);
                    reviewers.Add(availableReviewers[reviewWinnerIndex]);
                    availableReviewers.RemoveAt(reviewWinnerIndex);
                    reviewersCount--;
                }

                var result = string.Join(", ", reviewers);
                Console.WriteLine($"Send your code review to {result}");
                input = string.Empty;
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.WriteLine();
            }
        }
    }
}
