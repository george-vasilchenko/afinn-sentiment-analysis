namespace SentimentAnalysis.Common.Models
{
    public interface IResultEntry
    {
        string Text { get; }

        object Score { get; }

        string ToCombinedString();

        string ToScoreString();
    }
}