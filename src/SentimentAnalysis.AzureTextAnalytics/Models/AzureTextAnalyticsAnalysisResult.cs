using System.Collections.Generic;
using SentimentAnalysis.Common.Models;

namespace SentimentAnalysis.AzureTextAnalytics.Models
{
    public class AzureTextAnalyticsAnalysisResult : IAnalysisResult
    {
        private readonly IEnumerable<AzureTextAnalyticsResultEntry> entries;

        public AzureTextAnalyticsAnalysisResult(IEnumerable<AzureTextAnalyticsResultEntry> entries) => this.entries = entries;

        public IEnumerable<IResultEntry> Entries => this.entries;
    }
}