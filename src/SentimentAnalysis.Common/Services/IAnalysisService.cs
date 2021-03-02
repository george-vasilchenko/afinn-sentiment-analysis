using System.Collections.Generic;
using System.Threading.Tasks;
using SentimentAnalysis.Common.Models;

namespace SentimentAnalysis.Common.Services
{
    public interface IAnalysisService
    {
        Task<IAnalysisResult> EvaluateAsync(IEnumerable<INaturalExpression> expressions);
    }
}