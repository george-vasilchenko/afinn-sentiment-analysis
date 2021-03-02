using System.Globalization;
using SentimentAnalysis.Common.Models;

namespace SentimentAnalysis.AzureTextAnalytics.Models
{
    public class AzureTextAnalyticsResultEntry : IResultEntry
    {
        private readonly double average;
        private readonly string locale;
        private readonly double negative;
        private readonly double neutral;
        private readonly double positive;

        public AzureTextAnalyticsResultEntry(string text, double average, double negative, double neutral, double positive, string locale)
        {
            this.average = average;
            this.negative = negative;
            this.neutral = neutral;
            this.positive = positive;
            this.locale = locale;
            this.Text = text;
        }

        public string Text { get; }

        object IResultEntry.Score => (this.negative, this.neutral, this.positive);

        public string ToCombinedString() => $"{this.ToScoreString()}\t{this.Text}";

        public string ToScoreString()
        {
            var averageSegment = this.average.ToString(new CultureInfo(this.locale).NumberFormat);
            var negativeSegment = this.negative.ToString(new CultureInfo(this.locale).NumberFormat);
            var neutralSegment = this.neutral.ToString(new CultureInfo(this.locale).NumberFormat);
            var positiveSegment = this.positive.ToString(new CultureInfo(this.locale).NumberFormat);

            return $"{averageSegment}\t{negativeSegment}\t{neutralSegment}\t{positiveSegment}\t";
        }
    }
}