using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALWABP.Domain.Base;

namespace ALWABP.Domain.ALWABP
{
    public class ALWABPInstance : Instance
    {
        public int Workers { get; private set; }
        public int?[,] Matrix { get; private set; }
        public Dictionary<int, int> FasterWorkerForTasks { get; private set; }
        private Dictionary<(int, int), int> WorkersRanks { get; set; } 

        public ALWABPInstance(FileManager fileManager, int workers, int tasks, int?[,] matrix, (int, int)[] precedenceGraph)
            : base(InstanceType.ALWABP, fileManager, tasks, precedenceGraph)
        {
            Workers = workers;
            Matrix = matrix;
            FasterWorkerForTasks = new Dictionary<int, int>();
            WorkersRanks = new Dictionary<(int, int), int>();

            ComputeFasterWorkerForTasks();
            ComputeWorkersRanks();
        }

        private void ComputeWorkersRanks()
        {
            WorkersRanks.Clear();
            foreach (var task in GetTasksList())
            {
                var originalTaskOrder = GetTaskTimes(task);
                var ordered = originalTaskOrder.OrderBy(x => x).ToList();

                for (int i = 0; i < ordered.Count; i++)
                {
                    var worker = originalTaskOrder.IndexOf(ordered[i]);
                    WorkersRanks.Add((task, worker), i);
                }
            }
        }

        private void ComputeFasterWorkerForTasks()
        {
            FasterWorkerForTasks.Clear();
            foreach(var task in GetTasksList())
                FasterWorkerForTasks.Add(task, GetTaskTimes(task).IndexOf(GetMinTaskTime(task)));
        }

        public List<int> GetWorkersList()
        {
            return Enumerable.Range(0, Workers).ToList();
        }

        private List<int?> GetTaskTimes(int task) 
        {
            List<int?> result = new();
            for (int w = 0; w < Workers; w++)
                result.Add(Matrix[task, w]);
            return result;
        }

        public List<int> GetAssignableTasks(int worker, List<int>? taskList = null)
        {
            taskList ??= GetTasksList();
            return taskList.Where(t => Matrix[t, worker].HasValue).ToList();
        }

        public int? GetMinTaskTime(int task)
        {
            return GetTaskTimes(task).Min();
        }

        public int? GetMaxTaskTime(int task)
        {
            return GetTaskTimes(task).Max();
        }

        public double? GetAverageTaskTime(int task)
        {
            return GetTaskTimes(task).Average();
        }

        public int? GetMinPositionalWeight(int task)
        {
            return GetMinTaskTime(task) + Followers[task].Sum(x => GetMinTaskTime(x));
        }

        public int? GetMaxPositionalWeight(int task)
        {
            return GetMaxTaskTime(task) + Followers[task].Sum(x => GetMaxTaskTime(x));
        }

        public double? GetAveragePositionalWeight(int task)
        {
            return GetAverageTaskTime(task) + Followers[task].Sum(x => GetAverageTaskTime(x));
        }

        public int? GetDifferenceToBestWorker(int task, int worker)
        {
            int? workerTime = Matrix[task, worker];
            return workerTime.HasValue ? workerTime.Value - FasterWorkerForTasks[task] : null;
        }

        public int? GetRatioToBestWorker(int task, int worker)
        {
            int? workerTime = Matrix[task, worker];
            return workerTime.HasValue ? workerTime.Value / FasterWorkerForTasks[task] : null;
        }

        public int? GetNumberOfFollowersPerTime(int task, int worker)
        {
            int? workerTime = Matrix[task, worker];
            return workerTime.HasValue ? Followers[task].Count / workerTime.Value : null;
        }

        public int? GetNumberOfImmediateFollowersPerTime(int task, int worker)
        {
            int? workerTime = Matrix[task, worker];
            return workerTime.HasValue ? ImmediateFollowers[task].Count / workerTime.Value : null;
        }

        public int? GetRank(int task, int worker)
        {
            return WorkersRanks.ContainsKey((task, worker)) ? WorkersRanks[(task, worker)] : null;
        }
    }
}
