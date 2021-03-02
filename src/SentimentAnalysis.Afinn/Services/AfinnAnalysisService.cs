using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SentimentAnalysis.Afinn.Models;
using SentimentAnalysis.Common.Configs;
using SentimentAnalysis.Common.Models;
using SentimentAnalysis.Common.Services;

namespace SentimentAnalysis.Afinn.Services
{
    public class AfinnAnalysisService : IAnalysisService
    {
        private const double AfinnMin = -5;
        private const double AfinnMax = 5;

        private readonly ILexiconService lexiconService;
        private readonly IAppConfiguration appConfiguration;

        public AfinnAnalysisService(ILexiconService lexiconService, IAppConfiguration appConfiguration)
        {
            this.lexiconService = lexiconService;
            this.appConfiguration = appConfiguration;
        }

        public Task<IAnalysisResult> EvaluateAsync(IEnumerable<INaturalExpression> expressions)
        {
            return Task.Run(() => this.Evaluate(expressions));
        }

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

        private double CalculateScore(IEnumerable<int> scores)
        {
            var scoreArray = scores.ToArray();
            var afinnScore = scoreArray.Length == 0 ? 0.0 : scoreArray.Average();
            var normalizedScore = Map(afinnScore, AfinnMin, AfinnMax, -1, 1);

            return Math.Round(normalizedScore, this.appConfiguration.ResultsValuesDecimalPlaces);
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

        private double EvaluateExpression(INaturalExpression expression, ILexicon lexicon)
        {
            var cleanText = SanitizeString(expression.Text);
            var words = ExtractWords(cleanText);
            var scores = GetWordsScores(lexicon, words);

            return this.CalculateScore(scores);
        }

        private IAnalysisResult Evaluate(IEnumerable<INaturalExpression> expressions)
        {
            if (expressions is null)
            {
                throw new ArgumentNullException(nameof(expressions));
            }

            var lexicon = this.lexiconService.GetLexicon();
            var resultEntries = new List<AfinnResultEntry>();
            foreach (var expression in expressions)
            {
                var score = this.EvaluateExpression(expression, lexicon);
                resultEntries.Add(new AfinnResultEntry(expression.Text, score, this.appConfiguration.NumberFormatLocale));
            }

            return new AfinnAnalysisResult(resultEntries);
        }
    }
}