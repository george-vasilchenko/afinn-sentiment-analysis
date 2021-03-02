using System;
using Microsoft.Extensions.Configuration;

namespace SentimentAnalysis.AzureTextAnalytics.Configs
{
    public class AzureTextAnalyticsConfiguration : IAzureTextAnalyticsConfiguration
    {
        public AzureTextAnalyticsConfiguration(IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var endpoint = configuration[nameof(this.Endpoint)];
            Validate(endpoint, nameof(this.Endpoint));
            this.Endpoint = endpoint;

            var apiKey = configuration[nameof(this.ApiKey)];
            Validate(apiKey, nameof(this.ApiKey));
            this.ApiKey = apiKey;
        }

        public string Endpoint { get; }

        public string ApiKey { get; }

        private static void Validate(string value, string argumentName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", argumentName);
            }
        }
    }
}