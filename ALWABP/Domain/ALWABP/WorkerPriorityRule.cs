namespace ALWABP.Domain.ALWABP
{
    public static class WorkerPriorityRule
    {
        public enum RuleCriteria
        {
            MaxTasks,
            MinBWA,
            MinRLB
        }

        public static int? GetNextWorker(ALWABPInstance instance, List<int> unassignedTasks, List<int> unassignedWorkers, RuleCriteria ruleCriteria)
        {
            // Rule 1
            if (ruleCriteria == RuleCriteria.MaxTasks)
                return unassignedWorkers.OrderByDescending(x => instance.GetMaxTasks(x, unassignedTasks)).FirstOrDefault();

            // Rule 2
            //if (ruleCriteria == RuleCriteria.MinBWA)
                //return unassignedWorkers.OrderBy(x => instance.GetMinBWA(x, unassignedWorkers, unassignedTasks)).FirstOrDefault();


            // Rule 3
            if (ruleCriteria == RuleCriteria.MinRLB)
                return unassignedWorkers.OrderBy(x => instance.GetMinRLB(x, unassignedWorkers.ToList(), unassignedTasks)).FirstOrDefault();

            return null;
        }
    }
}
