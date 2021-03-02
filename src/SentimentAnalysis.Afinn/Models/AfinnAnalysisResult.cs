using System.Collections.Generic;
using SentimentAnalysis.Common.Models;

namespace SentimentAnalysis.Afinn.Models
{
    public class AfinnAnalysisResult : IAnalysisResult
    {
        private readonly IEnumerable<AfinnResultEntry> entries;

        public AfinnAnalysisResult(IEnumerable<AfinnResultEntry> entries) => this.entries = entries;

        public IEnumerable<IResultEntry> Entries => this.entries;
    }
}