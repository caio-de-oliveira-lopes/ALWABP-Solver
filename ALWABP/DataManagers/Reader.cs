using ALWABP.Domain.ALWABP;
using ALWABP.Domain.Base;
using ALWABP.Utils;

namespace ALWABP.DataManagers
{
    public static class Reader
    {
        public static T? ReadInputFile<T>(FileManager fileManager) where T : Instance
        {
            if (typeof(T) == typeof(ALWABPInstance))
                return ReadALWABPInput(fileManager) as T;
            else
                return null;
        }

        private static ALWABPInstance? ReadALWABPInput(FileManager fileManager)
        {
            string? line;
            int numberOfWorkers = 0;
            int numberOfTasks = 0;
            int?[,]? matrix = null;
            List<(int, int)> precedenceGraph = new();
            try
            {
                StreamReader sr = new(fileManager.GetFullInputPath());

                line = sr.ReadLine();

                if (line is null) return null;

                // First Line
                numberOfTasks = int.Parse(line);

                // Second Line
                line = sr.ReadLine();
                if (line is null) return null;

                string[] splitedLines = line.Split(" ");
                numberOfWorkers = splitedLines.Length;

                matrix = new int?[numberOfTasks, numberOfWorkers];

                int task = 0;
                do
                {
                    int worker = 0;
                    int?[] values = Array.ConvertAll(splitedLines, i => Util.ToNullableInt(i));
                    foreach (int? v in values)
                    {
                        matrix[task, worker] = v;
                        worker++;
                    }

                    line = sr.ReadLine();
                    if (line is null) return null;

                    splitedLines = line.Split(" ");
                    task++;
                } while (task < numberOfTasks);

                int uTask = -1, vTask = -1;
                do
                {
                    uTask = int.Parse(splitedLines.First());
                    vTask = int.Parse(splitedLines.Last());

                    if (uTask != -1 && vTask != -1)
                    {
                        precedenceGraph.Add((uTask - 1, vTask - 1));

                        line = sr.ReadLine();
                        if (line is null) return null;

                        splitedLines = line.Split(" ");
                    }
                } while (uTask != -1 && vTask != -1);
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Input Reading Finished!");
            }

            if (numberOfWorkers != 0 && numberOfTasks != 0 && matrix is not null)
                return new ALWABPInstance(fileManager, numberOfWorkers, numberOfTasks, matrix, precedenceGraph.ToArray());
            else
            {
                Console.WriteLine("Not a Valid Input!");
                return null;
            }
        }
    }
}
