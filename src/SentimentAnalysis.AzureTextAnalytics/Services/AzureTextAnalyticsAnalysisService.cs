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

            var expressionsArray = expressions.ToArray();
            var documents = expressionsArray.Select(e => e.Text);
            var response = await client.AnalyzeSentimentBatchAsync(documents);
            var results = expressionsArray.Zip(response.Value, this.ResultEntrySelector);

            return new AzureTextAnalyticsAnalysisResult(results);
            /*var results = response.Value
                .Select(this.ResultEntrySelector);*/
        }

        /*private static string ComposeText(AnalyzeSentimentResult sentimentResult)
        {
            return string.Join(" ", sentimentResult.DocumentSentiment.Sentences.Select(s => s.Text));
        }*/

        private AzureTextAnalyticsResultEntry ResultEntrySelector(INaturalExpression expression, AnalyzeSentimentResult sentimentResult)
        {
            var (negative, neutral, positive) = this.ComposeScore(sentimentResult);
            return new AzureTextAnalyticsResultEntry(
                expression.Text,
                negative,
                neutral,
                positive,
                this.appConfiguration.NumberFormatLocale);
        }
        
        /*private AzureTextAnalyticsResultEntry ResultEntrySelector(AnalyzeSentimentResult sentimentResult)
        {
            var (negative, neutral, positive) = this.ComposeScore(sentimentResult);
            return new AzureTextAnalyticsResultEntry(
                ComposeText(sentimentResult),
                negative,
                neutral,
                positive,
                this.appConfiguration.NumberFormatLocale);
        }*/

        private (double negative, double neutral, double positive) ComposeScore(AnalyzeSentimentResult sentimentResult)
        {
            var negativeScore = Math.Round(sentimentResult.DocumentSentiment.ConfidenceScores.Negative,
                this.appConfiguration.ResultsValuesDecimalPlaces);
            var neutralScore = Math.Round(sentimentResult.DocumentSentiment.ConfidenceScores.Neutral,
                this.appConfiguration.ResultsValuesDecimalPlaces);
            var positiveScore = Math.Round(sentimentResult.DocumentSentiment.ConfidenceScores.Positive,
                this.appConfiguration.ResultsValuesDecimalPlaces);

            return (negativeScore, neutralScore, positiveScore);
        }
    }
}