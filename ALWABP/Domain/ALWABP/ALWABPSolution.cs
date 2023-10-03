using ALWABP.Domain.Base;

namespace ALWABP.Domain.ALWABP
{
    public class ALWABPSolution : Solution
    {
        public Dictionary<int, int> WorkstationsCicleTimes { get; private set; }
        public Dictionary<int, int> AssignedWorkers { get; private set; }
        public Dictionary<int, List<int>> WorkerTasks { get; private set; }
        public int MaxCycleTime { get; private set; }

        public ALWABPSolution(Dictionary<int, int> workstationsCicleTimes, Dictionary<int, int> assignedWorkers, Dictionary<int, List<int>> workerTasks)
            : base()
        {
            WorkstationsCicleTimes = workstationsCicleTimes;
            AssignedWorkers = assignedWorkers;
            WorkerTasks = workerTasks;
            MaxCycleTime = workstationsCicleTimes.Values.Max();
        }
    }
}
