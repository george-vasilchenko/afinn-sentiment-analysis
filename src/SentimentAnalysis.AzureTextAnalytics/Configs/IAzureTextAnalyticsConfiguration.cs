namespace SentimentAnalysis.AzureTextAnalytics.Configs
{
    public interface IAzureTextAnalyticsConfiguration
    {
        string Endpoint { get; }

        string ApiKey { get; }
    }
}