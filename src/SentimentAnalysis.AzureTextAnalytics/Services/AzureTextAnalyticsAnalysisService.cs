using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.AI.TextAnalytics;
using SentimentAnalysis.AzureTextAnalytics.Configs;
using SentimentAnalysis.AzureTextAnalytics.Models;
using SentimentAnalysis.Common.Configs;
using SentimentAnalysis.Common.Models;
using SentimentAnalysis.Common.Services;
using Serilog;

namespace SentimentAnalysis.AzureTextAnalytics.Services
{
    public class AzureTextAnalyticsAnalysisService : IAnalysisService
    {
        private readonly IAppConfiguration appConfiguration;
        private readonly IAzureTextAnalyticsConfiguration azureConfiguration;

        public AzureTextAnalyticsAnalysisService(IAzureTextAnalyticsConfiguration azureConfiguration, IAppConfiguration appConfiguration)
        {
            this.azureConfiguration = azureConfiguration;
            this.appConfiguration = appConfiguration;
        }

        public async Task<IAnalysisResult> EvaluateAsync(IEnumerable<INaturalExpression> expressions)
        {
            var endpointUri = new Uri(this.azureConfiguration.Endpoint);
            var credential = new AzureKeyCredential(this.azureConfiguration.ApiKey);
            var client = new TextAnalyticsClient(endpointUri, credential);
            var entries = await this.SendRequestsInBatches(expressions.ToArray(), client);

            return new AzureTextAnalyticsAnalysisResult(entries);
        }

        private static double CalculateAverageValue(double negative, double neutral, double positive)
        {
            var halfNeutral = neutral / 2;
            var _negative = negative + halfNeutral;
            var _positive = positive + halfNeutral;

            return _positive - _negative;
        }

        private async Task<IEnumerable<AzureTextAnalyticsResultEntry>> SendRequestsInBatches(
            IReadOnlyCollection<INaturalExpression> expressionsArray,
            TextAnalyticsClient client)
        {
            var entries = new List<AzureTextAnalyticsResultEntry>();
            var total = expressionsArray.Count;
            var taken = 0;
            const int chunkSize = 10;

            while (taken < total)
            {
                var expressionsSegmentArray = expressionsArray.Skip(taken).Take(chunkSize).ToArray();
                var response = await client.AnalyzeSentimentBatchAsync(expressionsSegmentArray.Select(e => e.Text));
                var results = expressionsSegmentArray.Zip(response.Value, this.ResultEntrySelector).ToArray();

                taken += expressionsSegmentArray.Length;
                entries.AddRange(results);
                Log.Logger.Information($"Processed documents: {taken}");
            }

            return entries;
        }

        private AzureTextAnalyticsResultEntry ResultEntrySelector(INaturalExpression expression, AnalyzeSentimentResult sentimentResult)
        {
            var (average, negative, neutral, positive) = this.ComposeScore(sentimentResult);
            return new AzureTextAnalyticsResultEntry(
                expression.Text,
                average,
                negative,
                neutral,
                positive,
                this.appConfiguration.NumberFormatLocale);
        }

        private (double average, double negative, double neutral, double positive) ComposeScore(AnalyzeSentimentResult sentimentResult)
        {
            var averageScore = Math.Round(CalculateAverageValue(
                    sentimentResult.DocumentSentiment.ConfidenceScores.Negative,
                    sentimentResult.DocumentSentiment.ConfidenceScores.Neutral,
                    sentimentResult.DocumentSentiment.ConfidenceScores.Positive),
                this.appConfiguration.ResultsValuesDecimalPlaces);
            var negativeScore = Math.Round(sentimentResult.DocumentSentiment.ConfidenceScores.Negative,
                this.appConfiguration.ResultsValuesDecimalPlaces);
            var neutralScore = Math.Round(sentimentResult.DocumentSentiment.ConfidenceScores.Neutral,
                this.appConfiguration.ResultsValuesDecimalPlaces);
            var positiveScore = Math.Round(sentimentResult.DocumentSentiment.ConfidenceScores.Positive,
                this.appConfiguration.ResultsValuesDecimalPlaces);

            return (averageScore, negativeScore, neutralScore, positiveScore);
        }
    }
}