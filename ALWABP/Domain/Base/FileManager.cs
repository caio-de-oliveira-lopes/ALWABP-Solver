namespace ALWABP.Domain.Base
{
    public class FileManager
    {
        public string InputFileName { get; private set; }
        public string InputFilePath { get; private set; }
        public string OutputFilePath { get; private set; }
        public string OutputFileName { get; private set; }

        public FileManager(string inputFileName, string inputFilePath, string outputFilePath)
        {
            InputFileName = inputFileName;
            InputFilePath = inputFilePath;
            OutputFilePath = outputFilePath;
            OutputFileName = $"output{InputFileName}";
        }

        public string GetFullInputPath()
        {
            return Path.Combine(InputFilePath, InputFileName);
        }
    }
}