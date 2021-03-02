using System;
using Microsoft.Extensions.Configuration;

namespace Afinn.Configs
{
    public class AppConfiguration : IAppConfiguration
    {
        public AppConfiguration(IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var lexiconPath = configuration[nameof(this.LexiconPath)];
            ValidatePath(lexiconPath, nameof(this.LexiconPath));
            this.LexiconPath = lexiconPath;

            var datasetPath = configuration[nameof(this.DatasetPath)];
            ValidatePath(datasetPath, nameof(this.DatasetPath));
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
            ValidatePath(resultsFilePath, nameof(this.ResultsFilePath));
            this.ResultsFilePath = resultsFilePath;
        }

        public int DatasetRowsToSkip { get; }

        public string ResultsFilePath { get; }

        public string LexiconPath { get; }

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

        private static void ValidatePath(string path, string argumentName)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", argumentName);
            }
        }
    }
}