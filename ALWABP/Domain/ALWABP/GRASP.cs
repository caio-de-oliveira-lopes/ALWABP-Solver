using ALWABP.Domain.Base;
using static ALWABP.Domain.ALWABP.TaskPriorityRule;

namespace ALWABP.Domain.ALWABP
{
    public class GRASP : Heuristic
    {
        private ALWABPInstance? CurrentInstance { get; set; }

        public GRASP()
            : base(nameof(GRASP))
        {

        }

        public ALWABPSolution Construct(ALWABPInstance instance, RuleCriteria ruleCriteria, RuleSecondaryCriteria ruleSecondaryCriteria = RuleSecondaryCriteria.None)
        {
            CurrentInstance = instance;
            ALWABPSolution? solution = null;
            int maxCycleTime = 250; // TODO: Define good start value for maxCycleTime

            while (solution is null)
            {
                solution = StationOrientedAssignmentProcedureALWABP1(maxCycleTime, ruleCriteria, ruleSecondaryCriteria);
                maxCycleTime++;
            }

            return solution;
        }

        private ALWABPSolution? StationOrientedAssignmentProcedureALWABP1(int maxCycleTime, RuleCriteria ruleCriteria, RuleSecondaryCriteria ruleSecondaryCriteria = RuleSecondaryCriteria.None)
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

            // TODO: Apply worker ordering
            List<int> orderedUnassignedWorkers = unassignedWorkers.ToList();

            foreach (int worker in orderedUnassignedWorkers)
            {
                List<int> availableTasks = CurrentInstance.GetAssignableTasks(worker, unassignedTasks);
                int[]? orderedUnassignedTasks = Apply(CurrentInstance, availableTasks, ruleCriteria, ruleSecondaryCriteria, worker);

                if (orderedUnassignedTasks == null) continue;

                workerTasks.Add(worker, new List<int>()); // Initialize the list of tasks to a worker

                foreach (var task in orderedUnassignedTasks)
                {
                    var taskTime = CurrentInstance.GetTaskTime(task, worker);

                    if (taskTime == null) continue;

                    if (workstationTimes[currentWorkstation] + taskTime <= maxCycleTime)
                    {
                        workerTasks[worker].Add(task); // Assign the task to a worker
                        unassignedTasks.Remove(task); // Remove the task from the unassigned list

                        workstationTimes[currentWorkstation] += taskTime.Value; // Increase the workstation time
                    }
                }

                if (workerTasks[worker].Any())
                {
                    workstationWorker.Add(currentWorkstation, worker); // Assign the worker to the workstation
                    unassignedWorkers.Remove(worker); // Remove the worker from the unassigned list

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
