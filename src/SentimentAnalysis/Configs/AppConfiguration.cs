using System;
using Microsoft.Extensions.Configuration;
using SentimentAnalysis.Common.Configs;

namespace SentimentAnalysis.Configs
{
    public class AppConfiguration : IAppConfiguration
    {
        public AppConfiguration(IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var datasetPath = configuration[nameof(this.DatasetPath)];
            ValidateString(datasetPath, nameof(this.DatasetPath));
            this.DatasetPath = datasetPath;

            var datasetTargetSheetIndex = configuration[nameof(this.DatasetTargetSheetIndex)];
            ValidateNumber(datasetTargetSheetIndex, nameof(this.DatasetTargetSheetIndex), out var datasetTargetSheetIndexNumeric);
            this.DatasetTargetSheetIndex = datasetTargetSheetIndexNumeric;

            var datasetTargetColumnIndex = configuration[nameof(this.DatasetTargetColumnIndex)];
            ValidateNumber(datasetTargetColumnIndex, nameof(this.DatasetTargetColumnIndex), out var datasetTargetColumnIndexNumeric);
            this.DatasetTargetColumnIndex = datasetTargetColumnIndexNumeric;

            var datasetRowsToSkip = configuration[nameof(this.DatasetRowsToSkip)];
            ValidateNumber(datasetRowsToSkip, nameof(this.DatasetRowsToSkip), out var datasetRowsToSkipNumeric);
            this.DatasetRowsToSkip = datasetRowsToSkipNumeric;

            var resultsFilePath = configuration[nameof(this.ResultsFilePath)];
            ValidateString(resultsFilePath, nameof(this.ResultsFilePath));
            this.ResultsFilePath = resultsFilePath;

            var numberFormatLocale = configuration[nameof(this.NumberFormatLocale)];
            ValidateString(numberFormatLocale, nameof(this.NumberFormatLocale));
            this.NumberFormatLocale = numberFormatLocale;
            
            var resultsDecimalPlaces = configuration[nameof(this.ResultsValuesDecimalPlaces)];
            ValidateNumber(resultsDecimalPlaces, nameof(this.ResultsValuesDecimalPlaces), out var resultsDecimalPlacesNumeric);
            this.ResultsValuesDecimalPlaces = resultsDecimalPlacesNumeric;
        }

        public int DatasetRowsToSkip { get; }

        public string ResultsFilePath { get; }

        public string NumberFormatLocale { get; }

        public int ResultsValuesDecimalPlaces { get; }

        public string DatasetPath { get; }

        public int DatasetTargetSheetIndex { get; }

        public int DatasetTargetColumnIndex { get; }

        private static void ValidateNumber(string textValue, string argumentName, out int numericValue)
        {
            if (!int.TryParse(textValue, out numericValue))
            {
                throw new ArgumentException("Invalid value was provided", argumentName);
            }
        }

        private static void ValidateString(string value, string argumentName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", argumentName);
            }
        }
    }
}