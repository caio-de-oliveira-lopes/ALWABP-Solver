using ALWABP.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALWABP.Domain.ALWABP
{
    public class GRASP : Heuristic
    {
        private ALWABPInstance? CurrentInstance { get; set; }

        public GRASP() 
            : base(nameof(GRASP))
        {

        }

        public void Construct(ALWABPInstance instance)
        {
            CurrentInstance = instance;
            ALWABPSolution? solution = null;
            int maxCycleTime = 0;

            while (solution is null)
            {
                maxCycleTime++;
                solution = StationOrientedAssignmentProcedureALWABP1(maxCycleTime);
            }
        }

        private ALWABPSolution? StationOrientedAssignmentProcedureALWABP1(int maxCycleTime)
        {
            if (CurrentInstance == null)
                return null;

            int currentWorkstation = 0;
            int currentWorkstationTime = 0;
            var unassignedWorkers = Enumerable.Range(0, CurrentInstance.Workers).ToArray();
            var unassignedTasks = Enumerable.Range(0, CurrentInstance.Tasks).ToArray();

            Dictionary<int, int[]> WorkerTaskRelationDict = new();

            foreach(var worker in unassignedWorkers)
            {

            }

            return new ALWABPSolution();
        }
    }
}
