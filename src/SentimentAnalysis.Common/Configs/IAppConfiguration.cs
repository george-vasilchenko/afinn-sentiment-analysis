namespace SentimentAnalysis.Common.Configs
{
    public interface IAppConfiguration
    {
        string DatasetPath { get; }

        int DatasetTargetSheetIndex { get; }

        int DatasetTargetColumnIndex { get; }

        int DatasetRowsToSkip { get; }

        string ResultsFilePath { get; }

        string NumberFormatLocale { get; }

        int ResultsValuesDecimalPlaces { get; }
    }
}