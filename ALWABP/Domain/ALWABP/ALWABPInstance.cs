using ALWABP.Domain.Base;
using ALWABP.Utils;

namespace ALWABP.Domain.ALWABP
{
    public class ALWABPInstance : Instance
    {
        public int Workers { get; private set; }
        public int?[,] Matrix { get; private set; }
        public Dictionary<int, int> FasterWorkerForTasks { get; private set; }
        private Dictionary<(int, int), int> WorkersRanks { get; set; }
        public ALWABPSolution? BestSolution { get; private set; }
        public Dictionary<(GraphDirection, WorkerPriorityRule.RuleCriteria, TaskPriorityRule.RuleCriteria, TaskPriorityRule.RuleSecondaryCriteria), ALWABPSolution> Solutions { get; protected set; }

        public ALWABPInstance(FileManager fileManager, int workers, int tasks, int?[,] matrix, (int, int)[] precedenceGraph)
            : base(InstanceType.ALWABP, fileManager, tasks, precedenceGraph)
        {
            Workers = workers;
            Matrix = matrix;
            FasterWorkerForTasks = new();
            WorkersRanks = new();
            Solutions = new();

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
            int fasterWorker = FasterWorkerForTasks[task];
            return workerTime.HasValue ? workerTime.Value - Matrix[task, fasterWorker] : null;
        }

        public int? GetRatioToBestWorker(int task, int worker)
        {
            int? workerTime = Matrix[task, worker];
            int fasterWorker = FasterWorkerForTasks[task];
            return workerTime.HasValue ? workerTime.Value / Matrix[task, fasterWorker] : null;
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

        public IOrderedEnumerable<int> GetMinBWA(List<int> unassignedWorkers, List<int>? unassignedTasks = null)
        {
            unassignedTasks ??= GetTasksList();
            Dictionary<int, int> workerTimeMap = new();

            foreach (var uw in unassignedWorkers)
                workerTimeMap.Add(uw, 0);

            foreach (var task in unassignedTasks)
            {
                var fastestWorker = unassignedWorkers.MinBy(x => GetTaskTime(task, x));
                int taskTime = GetTaskTime(task, fastestWorker) ?? int.MaxValue;
                workerTimeMap[fastestWorker] += taskTime;
            }

            return unassignedWorkers.OrderBy(x => workerTimeMap[x]);
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

        public void AddSolution(ALWABPSolution solution)
        {
            Solutions.TryAdd((solution.GraphDirection, solution.WorkerRuleCriteria, solution.TaskRuleCriteria, solution.TaskRuleSecondaryCriteria), solution);

            if (solution.IsFeasible())
            {
                if (BestSolution == null || BestSolution.MaxCycleTime > solution.MaxCycleTime ||
                    (BestSolution.MaxCycleTime == solution.MaxCycleTime && solution.GetIdleTime() < BestSolution.GetIdleTime()))
                    BestSolution = solution;
            }
        }

        public Dictionary<string, List<Dictionary<string, object?>>> OutputAsDictionary()
        {
            Dictionary<string, List<Dictionary<string, object?>>> result = new();

            List<Dictionary<string, object?>> list = new();
            if (BestSolution != null)
            {
                Dictionary<string, object?> data = new()
                {
                    { nameof(BestSolution.Id), BestSolution.Id },
                    { nameof(BestSolution.MaxCycleTime), BestSolution.MaxCycleTime },
                    { nameof(BestSolution.ExecutionTimeMs), BestSolution.ExecutionTimeMs },
                    { nameof(BestSolution.Feasible), BestSolution.Feasible },
                    { nameof(BestSolution.WorkstationsCicleTimes), BestSolution.WorkstationsCicleTimes },
                    { nameof(BestSolution.WorkerTasks), Util.ToIntStringDict(BestSolution.WorkerTasks) },
                    { nameof(BestSolution.AssignedWorkers), BestSolution.AssignedWorkers },
                    { nameof(BestSolution.GraphDirection), BestSolution.GraphDirection.ToString() },
                    { nameof(BestSolution.WorkerRuleCriteria), BestSolution.WorkerRuleCriteria.ToString() },
                    { nameof(BestSolution.TaskRuleCriteria), BestSolution.TaskRuleCriteria.ToString() },
                    { nameof(BestSolution.TaskRuleSecondaryCriteria), BestSolution.TaskRuleSecondaryCriteria.ToString() }
                };
                list.Add(data);
            }
                  
            foreach (var solution in Solutions.Values)
            {
                if (solution == BestSolution) continue;

                Dictionary<string, object?> data = new()
                {
                    { nameof(solution.Id), solution.Id },
                    { nameof(solution.MaxCycleTime), solution.MaxCycleTime },
                    { nameof(solution.ExecutionTimeMs), solution.ExecutionTimeMs },
                    { nameof(solution.Feasible), solution.Feasible },
                    { nameof(solution.WorkstationsCicleTimes), solution.WorkstationsCicleTimes },
                    { nameof(solution.WorkerTasks), Util.ToIntStringDict(solution.WorkerTasks) },
                    { nameof(solution.AssignedWorkers), solution.AssignedWorkers },
                    { nameof(solution.GraphDirection), solution.GraphDirection.ToString() },
                    { nameof(solution.WorkerRuleCriteria), solution.WorkerRuleCriteria.ToString() },
                    { nameof(solution.TaskRuleCriteria), solution.TaskRuleCriteria.ToString() },
                    { nameof(solution.TaskRuleSecondaryCriteria), solution.TaskRuleSecondaryCriteria.ToString() }
                };
                list.Add(data);
            }
            result.Add(Name, list);
            
            return result;
        }
    }
}
