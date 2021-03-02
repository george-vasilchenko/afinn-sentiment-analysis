namespace Afinn.Models
{
    public interface IComment
    {
        string Text { get; }

        double Score { get; }

        void Evaluate(ILexicon lexicon);
    }
}