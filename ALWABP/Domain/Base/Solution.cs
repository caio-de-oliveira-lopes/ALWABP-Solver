namespace ALWABP.Domain.Base
{
    public abstract class Solution : Entity
    {
        private static int InstanceCounter { get; set; } = 0;
        public int MaxCycleTime { get; private set; }
        public long ExecutionTimeMs { get; private set; }
        private bool Feasible { get; set; }

        public Solution(int maxCycleTime)
            : base(InstanceCounter, $"Sol_({InstanceCounter})")
        {
            InstanceCounter++;
            MaxCycleTime = maxCycleTime;
            ExecutionTimeMs = 0;
            Feasible = true;
        }

        public void SetExecutionTimeMs(long executionTimeMs)
        {
            ExecutionTimeMs = executionTimeMs;
        }

        public void SetInfeasible()
        {
            Feasible = false;
        }

        public bool IsFeasible()
        {
            return Feasible;
        }
    }
}
