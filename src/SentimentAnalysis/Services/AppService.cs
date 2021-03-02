using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SentimentAnalysis.Common.Configs;
using SentimentAnalysis.Common.Models;
using SentimentAnalysis.Common.Services;
using Serilog;

namespace SentimentAnalysis.Services
{
    public class AppService : IAppService
    {
        private readonly IEnumerable<IAnalysisService> analysisServices;
        private readonly IAppConfiguration appConfiguration;
        private readonly INaturalExpressionsProvider naturalExpressionsProvider;

        public AppService(
            IEnumerable<IAnalysisService> analysisServices,
            INaturalExpressionsProvider naturalExpressionsProvider,
            IAppConfiguration appConfiguration)
        {
            this.analysisServices = analysisServices;
            this.naturalExpressionsProvider = naturalExpressionsProvider;
            this.appConfiguration = appConfiguration;
        }

        public async Task RunAsync()
        {
            var expressions = this.naturalExpressionsProvider.Expressions.Take(100).ToArray();
            var combinedResults = await this.RunAnalysisAsync(expressions);
            var analysisResultsText = ConvertAnalysisResultsToText(combinedResults);

            Log.Logger.Information(analysisResultsText);
            await File.WriteAllTextAsync(this.appConfiguration.ResultsFilePath, analysisResultsText);
        }

        private static void ProcessEntry(IResultEntry entry, IDictionary<string, string> combinedResults)
        {
            var key = entry.Text;
            var value = entry.ToScoreString();

            if (combinedResults.ContainsKey(key))
            {
                combinedResults[key] += value;
            }
            else
            {
                combinedResults[key] = $"{value}\t";
            }
        }

        private static string ConvertAnalysisResultsToText(Dictionary<string, string> analysisResults)
        {
            return analysisResults
                .Select(e => $"{e.Value}{e.Key}")
                .Aggregate((a, b) => $"{a}{Environment.NewLine}{b}");
        }

        private async Task<Dictionary<string, string>> RunAnalysisAsync(INaturalExpression[] expressions)
        {
            var combinedResults = new Dictionary<string, string>();
            foreach (var analysisService in this.analysisServices)
            {
                var analysisResult = await analysisService.EvaluateAsync(expressions);
                foreach (var entry in analysisResult.Entries)
                {
                    ProcessEntry(entry, combinedResults);
                }
            }

            return combinedResults;
        }
    }
}