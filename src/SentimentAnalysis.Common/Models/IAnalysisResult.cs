using System.Collections.Generic;

namespace SentimentAnalysis.Common.Models
{
    public interface IAnalysisResult
    {
        IEnumerable<IResultEntry> Entries { get; }
    }
}