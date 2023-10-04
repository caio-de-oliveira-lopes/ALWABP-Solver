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

        public ALWABPSolution Construct(ALWABPInstance instance, WorkerPriorityRule.RuleCriteria workerRuleCriteria, TaskPriorityRule.RuleCriteria ruleCriteria, 
            TaskPriorityRule.RuleSecondaryCriteria ruleSecondaryCriteria = TaskPriorityRule.RuleSecondaryCriteria.None)
        {
            CurrentInstance = instance;
            ALWABPSolution? solution = null;
            int maxCycleTime = 250; // TODO: Define good start value for maxCycleTime

            while (solution is null)
            {
                solution = StationOrientedAssignmentProcedureALWABP1(maxCycleTime, workerRuleCriteria, ruleCriteria, ruleSecondaryCriteria);
                maxCycleTime++;
            }

            return solution;
        }

        private ALWABPSolution? StationOrientedAssignmentProcedureALWABP1(int maxCycleTime, 
            WorkerPriorityRule.RuleCriteria workerRuleCriteria, TaskPriorityRule.RuleCriteria ruleCriteria, 
            TaskPriorityRule.RuleSecondaryCriteria ruleSecondaryCriteria = TaskPriorityRule.RuleSecondaryCriteria.None)
        {
            if (CurrentInstance == null)
                return null;

            Dictionary<int, int> workstationTimes = new();
            Dictionary<int, int> workstationWorker = new();
            Dictionary<int, List<int>> workerTasks = new();

            int currentWorkstation = 0;

            workstationTimes.Add(currentWorkstation, 0);

            List<int> unassignedWorkers = CurrentInstance.GetWorkersList();
            List<int> unassignedTasks = CurrentInstance.GetTasksList();

            while (unassignedWorkers.Any() && unassignedTasks.Any())
            {
                int? worker = WorkerPriorityRule.GetNextWorker(CurrentInstance, unassignedTasks, unassignedWorkers, workerRuleCriteria);

                if (worker == null) break;

                List<int> availableTasks = CurrentInstance.GetAssignableTasks(worker.Value, unassignedTasks);
                int[]? orderedUnassignedTasks = TaskPriorityRule.Apply(CurrentInstance, availableTasks, ruleCriteria, ruleSecondaryCriteria, worker);

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
            }

            if (!unassignedTasks.Any()) // Check workers assignment too?
                return new ALWABPSolution(workstationTimes, workstationWorker, workerTasks);
            else
                return null;
        }
    }
}
