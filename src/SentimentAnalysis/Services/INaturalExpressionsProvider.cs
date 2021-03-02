using System.Collections.Generic;
using SentimentAnalysis.Common.Models;

namespace SentimentAnalysis.Services
{
    public interface INaturalExpressionsProvider
    {
        IEnumerable<INaturalExpression> Expressions { get; }
    }
}