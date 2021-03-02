using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Afinn.Models
{
    public class Comment : IComment
    {
        private const int ResultScoreDecimalPlaces = 3;
        private const char ResultSeparatorCharacter = '\t';
        private const double AfinnMin = -5;
        private const double AfinnMax = 5;

        public Comment(string text) => this.Text = text;

        public string Text { get; }

        public double Score { get; private set; }

        public void Evaluate(ILexicon lexicon)
        {
            var cleanText = SanitizeString(this.Text);
            var words = ExtractWords(cleanText);
            var scores = GetWordsScores(lexicon, words);
            this.Score = CalculateScore(scores);
        }

        public override string ToString() 
            => $"{this.Score.ToString(CultureInfo.CurrentUICulture.NumberFormat)}{ResultSeparatorCharacter}{this.Text}";

        private static IEnumerable<int> GetWordsScores(ILexicon lexicon, IEnumerable<string> words)
        {
            var scores = new List<int>();
            foreach (var word in words)
            {
                if (!lexicon.Entries.TryGetValue(word, out var wordScore))
                {
                    continue;
                }

                scores.Add(wordScore);
            }

            return scores;
        }

        private static double CalculateScore(IEnumerable<int> scores)
        {
            var scoreArray = scores.ToArray();
            var afinnScore = scoreArray.Length == 0 ? 0.0 : scoreArray.Average();
            var normalizedScore = Map(afinnScore, AfinnMin, AfinnMax, -1, 1);

            return Math.Round(normalizedScore, ResultScoreDecimalPlaces);
        }

        private static double Map(double x, double inMin, double inMax, double outMin, double outMax) =>
            (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;

        private static IEnumerable<string> ExtractWords(string line)
        {
            var sanitizedLine = SanitizeString(line);

            const char separator = ' ';
            var words = sanitizedLine
                .Split(separator, StringSplitOptions.RemoveEmptyEntries)
                .Where(w => w.Length > 1);

            return words.ToArray();
        }

        private static string SanitizeString(string value)
        {
            const string charactersToReplace = "?&^$#@!()+-,.:;<>’\'*";
            const string replacementCharacter = " ";

            return charactersToReplace
                .Aggregate(value, (current, character)
                    => current.Replace(character.ToString(), replacementCharacter, StringComparison.InvariantCultureIgnoreCase))
                .ToLower();
        }
    }
}