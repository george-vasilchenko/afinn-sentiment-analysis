using System.Collections.Generic;

namespace Afinn.Models
{
    public class Lexicon : ILexicon
    {
        public Lexicon(Dictionary<string, int> entries) => this.Entries = entries;

        public Dictionary<string, int> Entries { get; }
    }
}