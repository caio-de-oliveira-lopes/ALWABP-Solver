using ALWABP.Domain.Base;

namespace ALWABP.Domain.ALWABP
{
    public class GRASP : Heuristic
    {
        private ALWABPInstance? CurrentInstance { get; set; }

        public GRASP()
            : base(nameof(GRASP))
        {

        }

        public ALWABPSolution Construct(ALWABPInstance instance, ALWABPInstance.GraphDirection graphDirection, WorkerPriorityRule.RuleCriteria workerRuleCriteria, TaskPriorityRule.RuleCriteria ruleCriteria, 
            TaskPriorityRule.RuleSecondaryCriteria ruleSecondaryCriteria = TaskPriorityRule.RuleSecondaryCriteria.None)
        {
            CurrentInstance = instance;
            ALWABPSolution? solution = null;
            int maxCycleTime = 1;

            System.Diagnostics.Stopwatch watch = new();
            watch.Start();
            while (solution is null)
            {
                solution = StationOrientedAssignmentProcedureALWABP1(maxCycleTime, graphDirection, workerRuleCriteria, ruleCriteria, ruleSecondaryCriteria);
                maxCycleTime++;
            }
            watch.Stop();
            solution.SetExecutionTimeMs(watch.ElapsedMilliseconds);

            Console.Write($"Found solution with Id: {solution.Id}\n\n");

            return solution;
        }

        private ALWABPSolution? StationOrientedAssignmentProcedureALWABP1(int maxCycleTime, ALWABPInstance.GraphDirection graphDirection, 
            WorkerPriorityRule.RuleCriteria workerRuleCriteria, TaskPriorityRule.RuleCriteria ruleCriteria, 
            TaskPriorityRule.RuleSecondaryCriteria ruleSecondaryCriteria = TaskPriorityRule.RuleSecondaryCriteria.None)
        {
            if (CurrentInstance == null)
                return null;

            Dictionary<int, int> workstationTimes = new();
            Dictionary<int, int> workstationWorker = new();
            Dictionary<int, List<int>> workerTasks = new();

            int currentWorkstation = 0;

            if (!workstationTimes.ContainsKey(currentWorkstation))
                workstationTimes.Add(currentWorkstation, 0);

            List<int> unassignedWorkers = CurrentInstance.GetWorkersList();
            List<int> unassignedTasks = CurrentInstance.GetTasksList();

            while (unassignedWorkers.Any() && unassignedTasks.Any())
            {
                int? worker;
                if (unassignedWorkers.Count > 1)
                    worker = WorkerPriorityRule.GetNextWorker(CurrentInstance, unassignedTasks, unassignedWorkers, workerRuleCriteria);
                else
                    worker = unassignedWorkers.FirstOrDefault();

                if (worker == null) break;

                List<int> availableTasks = CurrentInstance.GetAssignableTasks(worker.Value, unassignedTasks);
                int[]? orderedUnassignedTasks = TaskPriorityRule.Apply(CurrentInstance, availableTasks, worker.Value, ruleCriteria, ruleSecondaryCriteria);

                if (orderedUnassignedTasks == null) continue;

                workerTasks.Add(worker.Value, new List<int>()); // Initialize the list of tasks to a worker

                foreach (var task in orderedUnassignedTasks)
                {
                    var taskTime = CurrentInstance.GetTaskTime(task, worker.Value);

                    if (taskTime == null) continue;

                    if (workstationTimes[currentWorkstation] + taskTime <= maxCycleTime)
                    {
                        workerTasks[worker.Value].Add(task); // Assign the task to a worker
                        unassignedTasks.Remove(task); // Remove the task from the unassigned list

                        workstationTimes[currentWorkstation] += taskTime.Value; // Increase the workstation time
                    }
                }

                if (workerTasks[worker.Value].Any())
                {
                    workstationWorker.Add(currentWorkstation, worker.Value); // Assign the worker to the workstation
                    unassignedWorkers.Remove(worker.Value); // Remove the worker from the unassigned list

                    if (unassignedTasks.Any() && unassignedWorkers.Any())
                    {
                        currentWorkstation++;
                        workstationTimes.Add(currentWorkstation, 0);
                    }
                }
                else
                    break;
            }

            if (!unassignedTasks.Any())
            {
                var solution = new ALWABPSolution(workstationTimes, workstationWorker, workerTasks, 
                    graphDirection, workerRuleCriteria, ruleCriteria, ruleSecondaryCriteria);

                if (unassignedWorkers.Any())
                    solution.SetInfeasible();

                return solution;
            }
            else
                return null;
        }
    }
}
