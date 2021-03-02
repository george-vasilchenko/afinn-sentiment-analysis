using System;
using SentimentAnalysis.Common.Models;

namespace SentimentAnalysis.Models
{
    public class NaturalExpression : INaturalExpression
    {
        public NaturalExpression(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(text));
            }

            this.Text = text;
        }

        public string Text { get; }
    }
}