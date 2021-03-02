using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using ExcelDataReader;
using SentimentAnalysis.Common.Configs;
using SentimentAnalysis.Common.Models;
using SentimentAnalysis.Models;

namespace SentimentAnalysis.Services
{
    public class NaturalExpressionsProvider : INaturalExpressionsProvider
    {
        public NaturalExpressionsProvider(IAppConfiguration appConfiguration)
            => this.Expressions = InitializeExpressions(
                appConfiguration.DatasetPath,
                appConfiguration.DatasetTargetSheetIndex,
                appConfiguration.DatasetTargetColumnIndex,
                appConfiguration.DatasetRowsToSkip);

        public IEnumerable<INaturalExpression> Expressions { get; }

        private static IEnumerable<NaturalExpression> InitializeExpressions(string path, int sheetIndex, int columnIndex, int rowsToSkip)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using var stream = File.Open(path, FileMode.Open, FileAccess.Read);
            using var reader = ExcelReaderFactory.CreateReader(stream);

            return reader
                .AsDataSet()
                .Tables[sheetIndex]
                .AsEnumerable()
                .Skip(rowsToSkip)
                .Select(CommentSelector(columnIndex))
                .ToArray();
        }

        private static Func<DataRow, NaturalExpression> CommentSelector(int columnIndex)
        {
            return row => new NaturalExpression(row.ItemArray[columnIndex].ToString());
        }
    }
}