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
            foreach (int task in GetTasksList())
            {
                List<int?> originalTaskOrder = GetTaskTimes(task);
                List<(int?, int)> labeledOriginalTaskOrder = new();
                for (int i = 0; i < originalTaskOrder.Count; i++)
                {
                    labeledOriginalTaskOrder.Add((originalTaskOrder[i], i));
                }

                List<(int?, int)> ordered = labeledOriginalTaskOrder.OrderBy(x => x.Item1).ToList();

                for (int i = 0; i < ordered.Count; i++)
                {
                    int worker = ordered[i].Item2;
                    WorkersRanks.Add((task, worker), i);
                }
            }
        }

        private void ComputeFasterWorkerForTasks()
        {
            FasterWorkerForTasks.Clear();
            foreach (int task in GetTasksList())
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

        public int? GetMinTaskTime(int task, int[]? considerWorkers = null)
        {
            var taskTimes = GetTaskTimes(task);
            if (considerWorkers != null)
            {
                if (!considerWorkers.Any()) return null;

                taskTimes = taskTimes.Where(x => considerWorkers.Contains(taskTimes.IndexOf(x))).ToList();
            }
            return taskTimes.Min();
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

        public int? GetTaskTime(int task, int worker)
        {
            return Matrix[task, worker];
        }

        public int GetMaxTasks(int worker, List<int>? tasks = null)
        {
            int count = 0;
            tasks ??= GetTasksList();
            for (int i = 0; i < tasks.Count; i++)
            {
                if (Matrix[tasks[i], worker].HasValue)
                    count++;
            }
            return count;
        }

        public int GetMinBWA(int worker, List<int> unassignedWorkers, List<int>? unassignedTasks = null)
        {
            return 0;
            Dictionary<int, int> workerTimeMap = new();
            foreach (var uw in unassignedWorkers)
            {
                workerTimeMap.Add(uw, 0);
            }
            foreach (var task in unassignedTasks)
            {

            }
        }

        public int GetMinRLB(int worker, List<int> unassignedWorkers, List<int>? unassignedTasks = null)
        {
            unassignedTasks ??= GetTasksList();
            int[] tasks = GetAssignableTasks(worker, unassignedTasks).ToArray();
            unassignedWorkers.Remove(worker);
            int ammountOfTime = 0;

            for (int i = 0; i < tasks.Length; i++)
            {
                int? taskTime = GetMinTaskTime(tasks[i], unassignedWorkers.ToArray());
                if (taskTime.HasValue)
                    ammountOfTime += taskTime.Value;
            }

            return ammountOfTime / unassignedWorkers.Count;
        }
    }
}
