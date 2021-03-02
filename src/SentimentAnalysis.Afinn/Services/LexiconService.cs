using System.Collections.Generic;
using System.IO;
using SentimentAnalysis.Afinn.Configs;
using SentimentAnalysis.Afinn.Models;

namespace SentimentAnalysis.Afinn.Services
{
    public class LexiconService : ILexiconService
    {
        private readonly ILexicon lexicon;

        public LexiconService(IAfinnConfiguration afinnConfiguration)
            => this.lexicon = new Lexicon(InitializeLexicon(afinnConfiguration.LexiconPath));

        public ILexicon GetLexicon() => this.lexicon;

        private static Dictionary<string, int> InitializeLexicon(string path)
        {
            using var stream = File.OpenRead(path);
            using var streamReader = new StreamReader(stream);

            return ConvertTextToDictionary(streamReader);
        }

        private static Dictionary<string, int> ConvertTextToDictionary(StreamReader streamReader)
        {
            var collection = new Dictionary<string, int>();

            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                var (word, score) = ProcessLine(line);
                collection.Add(word, score);
            }

            return collection;
        }

        private static KeyValuePair<string, int> ProcessLine(string line)
        {
            const char separator = '\t';
            var elements = line.Split(new[] { separator });

            return KeyValuePair.Create(elements[0], int.Parse(elements[1]));
        }
    }
}