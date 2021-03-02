using System.Collections.Generic;
using System.IO;
using Afinn.Configs;
using Afinn.Models;

namespace Afinn.Services
{
    public class LexiconService : ILexiconService
    {
        private readonly ILexicon lexicon;

        public LexiconService(IAppConfiguration appConfiguration) => this.lexicon = new Lexicon(InitializeLexicon(appConfiguration.LexiconPath));

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