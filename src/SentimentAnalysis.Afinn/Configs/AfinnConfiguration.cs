using System;
using Microsoft.Extensions.Configuration;

namespace SentimentAnalysis.Afinn.Configs
{
    public class AfinnConfiguration : IAfinnConfiguration
    {
        public AfinnConfiguration(IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var lexiconPath = configuration[nameof(this.LexiconPath)];
            ValidatePath(lexiconPath, nameof(this.LexiconPath));
            this.LexiconPath = lexiconPath;
        }

        public string LexiconPath { get; }

        private static void ValidatePath(string path, string argumentName)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", argumentName);
            }
        }
    }
}