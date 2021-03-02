using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Afinn.Configs;
using Afinn.Models;
using Serilog;

namespace Afinn.Services
{
    public class AppService : IAppService
    {
        private readonly ILexiconService lexiconService;
        private readonly ICommentsService commentsService;
        private readonly IAppConfiguration appConfiguration;

        public AppService(
            ILexiconService lexiconService,
            ICommentsService commentsService,
            IAppConfiguration appConfiguration)
        {
            this.lexiconService = lexiconService;
            this.commentsService = commentsService;
            this.appConfiguration = appConfiguration;
        }

        public void Run()
        {
            var lexicon = this.lexiconService.GetLexicon();
            var comments = this.commentsService.GetComments().ToArray();

            foreach (var comment in comments)
            {
                comment.Evaluate(lexicon);
            }

            this.LogResults(comments);
        }

        private void LogResults(IEnumerable<Comment> comments)
        {
            var outputText = comments
                .Select(c => c.ToString())
                .Aggregate((a, b) => $"{a}{Environment.NewLine}{b}");
            
            Log.Logger.Information(outputText);
            File.WriteAllText(this.appConfiguration.ResultsFilePath, outputText);
        }
    }
}