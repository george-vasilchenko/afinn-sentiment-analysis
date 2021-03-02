using SentimentAnalysis.Afinn.Models;

namespace SentimentAnalysis.Afinn.Services
{
    public interface ILexiconService
    {
        ILexicon GetLexicon();
    }
}