using System.Globalization;
using SentimentAnalysis.Common.Models;

namespace SentimentAnalysis.Afinn.Models
{
    public class AfinnResultEntry : IResultEntry
    {
        private readonly string locale;
        private readonly double score;

        public AfinnResultEntry(string text, double score, string locale)
        {
            this.Text = text;
            this.score = score;
            this.locale = locale;
        }

        public string Text { get; }

        public string ToScoreString() => this.score.ToString(new CultureInfo(this.locale).NumberFormat);
    }
}