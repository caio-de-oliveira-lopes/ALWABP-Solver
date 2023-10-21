using ALWABP.DataManagers;
using ALWABP.Domain.ALWABP;
using ALWABP.Domain.Base;

namespace ALWABP
{
    public static class Program
    {
        private static readonly string Warning = "\nPlease Follow The Structure => '<InputFileName> <InputFilePath> <OutputFilePath(OPTIONAL)>'";
        static void Main(string[] args)
        {
            string inputFileName;
            /*
            if (args.Length > 0)
                inputFileName = args[0];
            else
            {
                Console.WriteLine($"No Input File Name Was Informed.{Warning}");
                return;
            }*/

            string inputFileDirectory;
            if (args.Length > 0)
                inputFileDirectory = args[0];
            else
            {
                Console.WriteLine($"No Input File Path Was Informed.{Warning}");
                return;
            }

            string outputFilePath;
            if (args.Length > 1)
                outputFilePath = args[1];
            else
                outputFilePath = Directory.GetCurrentDirectory();

            if (!Directory.Exists(outputFilePath))
                Directory.CreateDirectory(outputFilePath);

            foreach (var file in Directory.GetFiles(inputFileDirectory).OrderBy(x => x).ToList())
            {
                inputFileName = file.Split(Path.DirectorySeparatorChar).Last();
                ALWABPInstance? instance = Reader.ReadInputFile<ALWABPInstance>(new(inputFileName, inputFileDirectory, outputFilePath));

                if (instance is null)
                {
                    Console.WriteLine($"Input Reading Error!");
                    return;
                }

                if (File.Exists(instance.Manager.GetFullOutputPath()))
                {
                    Console.WriteLine($"Skiped {inputFileName}!");
                    continue;
                }

                GRASP grasp = new();
                ALWABPSolution solution;

                foreach (Instance.GraphDirection graphDirection in Enum.GetValues<Instance.GraphDirection>())
                {
                    instance.ComputeData(graphDirection);
                    foreach (WorkerPriorityRule.RuleCriteria workerRuleCriteria in Enum.GetValues<WorkerPriorityRule.RuleCriteria>())
                    {
                        foreach (TaskPriorityRule.RuleCriteria ruleCriteria in Enum.GetValues<TaskPriorityRule.RuleCriteria>())
                        {
                            foreach (TaskPriorityRule.RuleSecondaryCriteria secondayCriteria in TaskPriorityRule.GetSecondaryCriterias(ruleCriteria))
                            {
                                solution = grasp.Construct(instance, graphDirection, workerRuleCriteria, ruleCriteria, secondayCriteria);
                                if (solution.IsFeasible())
                                    instance.AddSolution(solution);
                            }
                        }
                    }
                }

                Writer.WriteOutputFile(instance);
                // For now, only one execution
                break;
            }
        }
    }
}