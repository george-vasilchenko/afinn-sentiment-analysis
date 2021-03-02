namespace Afinn.Configs
{
    public interface IAppConfiguration
    {
        string LexiconPath { get; }

        string DatasetPath { get; }

        int DatasetTargetSheetIndex { get; }

        int DatasetTargetColumnIndex { get; }

        int DatasetRowsToSkip { get; }

        string ResultsFilePath { get; }
    }
}