using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Speech.Synthesis;

namespace RandomReviewerSelector
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = null;
            while ((input = args.Any() && input == null ? args[0] : Console.ReadLine()) != "exit")
            {
                var availableReviewers = ConfigurationManager.AppSettings["AvailableReviewers"].Split(';').ToList();
                int reviewersCount;
                if (!int.TryParse(input, out reviewersCount) || reviewersCount > availableReviewers.Count)
                {
                    Console.WriteLine(
                        $"Please specify reviewers count, should be less than or equal to {availableReviewers.Count}.");
                    Console.WriteLine("If you want to exit, type \"exit\".");
                    continue;
                }

                var reviewers = GetReviewers(availableReviewers, reviewersCount);

                OutputReviewers(reviewers);
                Console.WriteLine();
            }
        }

        private static void OutputReviewers(IEnumerable<string> reviewers)
        {
            var separator = reviewers.Count() == 2 ? " and " : ", ";
            var result = string.Join(separator, reviewers);
            var messageFormat = ConfigurationManager.AppSettings["SendCodeReviewMessage"] ?? "Send your code review to {0}";
            var text = string.Format(messageFormat, result);

            //Outputing to Console
            Console.WriteLine(text);

            //Outputing to SpeechSynthesizer
            bool speechEnabled;
            if (bool.TryParse(ConfigurationManager.AppSettings["EnableSpeech"], out speechEnabled))
            {
                using (var synth = new SpeechSynthesizer())
                {
                    synth.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult);
                    synth.SetOutputToDefaultAudioDevice();
                    synth.Speak(text);
                }
            }
        }

        private static IEnumerable<string> GetReviewers(IEnumerable<string> availableReviewers, int reviewersCount)
        {
            var availableReviewersList = availableReviewers.ToList();
            var reviewers = new List<string>(availableReviewersList.Count);
            var random = new Random();
            while (reviewersCount != 0)
            {
                var reviewWinnerIndex = random.Next(availableReviewersList.Count);
                reviewers.Add(availableReviewersList[reviewWinnerIndex]);
                availableReviewersList.RemoveAt(reviewWinnerIndex);
                reviewersCount--;
            }
            return reviewers;
        }
    }
}
