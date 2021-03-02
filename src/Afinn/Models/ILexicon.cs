using System.Collections.Generic;

namespace Afinn.Models
{
    public interface ILexicon
    {
        Dictionary<string, int> Entries { get; }
    }
}