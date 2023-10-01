using ALWABP.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            int maxCycleTime = 0;

            while (solution is null)
            {
                maxCycleTime++;
                solution = StationOrientedAssignmentProcedureALWABP1(maxCycleTime, ruleCriteria, ruleSecondaryCriteria);
            }

            return solution;
        }

        private ALWABPSolution? StationOrientedAssignmentProcedureALWABP1(int maxCycleTime, RuleCriteria ruleCriteria, RuleSecondaryCriteria ruleSecondaryCriteria = RuleSecondaryCriteria.None)
        {
            if (CurrentInstance == null)
                return null;

            Dictionary<int, int> workstationTimes = new();
            Dictionary<int, List<int>> workstationWorkers = new();
            Dictionary<int, List<int>> workerTasks = new();

            int currentWorkstation = 0;

            workstationTimes.Add(currentWorkstation, 0);
            workstationWorkers.Add(currentWorkstation, new List<int>());

            var unassignedWorkers = CurrentInstance.GetWorkersList();
            var unassignedTasks = CurrentInstance.GetTasksList();

            // TODO: Apply worker ordering
            var orderedUnassignedWorkers = unassignedWorkers;

            foreach (var worker in orderedUnassignedWorkers)
            {
                workerTasks.Add(worker, new List<int>());
                workstationWorkers[currentWorkstation].Add(worker);

                var availableTasks = CurrentInstance.GetAssignableTasks(worker, unassignedTasks);
                var orderedUnassignedTasks = Apply(CurrentInstance, availableTasks, ruleCriteria, ruleSecondaryCriteria, worker);
            }

            return new ALWABPSolution();
        }
    }
}
