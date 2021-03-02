using System.Collections.Generic;

namespace SentimentAnalysis.Afinn.Models
{
    public interface ILexicon
    {
        Dictionary<string, int> Entries { get; }
    }
}