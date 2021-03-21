namespace SentimentAnalysis.Common.Models
{
    public interface IResultEntry
    {
        string Text { get; }

        string ToScoreString();
    }
}