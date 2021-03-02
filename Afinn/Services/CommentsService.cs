using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Afinn.Configs;
using Afinn.Models;
using ExcelDataReader;

namespace Afinn.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly IEnumerable<Comment> comments;

        public CommentsService(IAppConfiguration appConfiguration)
            => this.comments = InitializeComments(
                appConfiguration.DatasetPath,
                appConfiguration.DatasetTargetSheetIndex,
                appConfiguration.DatasetTargetColumnIndex,
                appConfiguration.DatasetRowsToSkip);

        public IEnumerable<Comment> GetComments() => this.comments;

        private static IEnumerable<Comment> InitializeComments(string path, int sheetIndex, int columnIndex, int rowsToSkip)
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

        private static Func<DataRow, Comment> CommentSelector(int columnIndex)
        {
            return dr => new Comment(dr.ItemArray[columnIndex].ToString());
        }
    }
}