namespace ALWABP.Domain.ALWABP
{
    public static class TaskPriorityRule
    {
        public enum RuleCriteria
        {
            MaxF,
            MaxIF,
            MaxTime,
            MinTime,
            MaxPW,
            MinD,
            MinR,
            MaxFTime,
            MaxIFTime,
            MinRank
        }

        public enum RuleSecondaryCriteria
        {
            None,
            Min,
            Max,
            Average
        }

        public static int[]? Apply(ALWABPInstance instance, List<int> unassignedTasks, RuleCriteria ruleCriteria,
            RuleSecondaryCriteria ruleSecondaryCriteria = RuleSecondaryCriteria.None, int? worker = null)
        {
            // Rule 1
            if (ruleCriteria == RuleCriteria.MaxF)
                return unassignedTasks.OrderByDescending(x => instance.Followers[x].Count).ToArray();

            // Rule 2
            if (ruleCriteria == RuleCriteria.MaxIF)
                return unassignedTasks.OrderByDescending(x => instance.ImmediateFollowers[x].Count).ToArray();

            if (ruleCriteria == RuleCriteria.MaxTime)
            {
                // Rule 3
                if (ruleSecondaryCriteria == RuleSecondaryCriteria.Min)
                    return unassignedTasks.OrderByDescending(x => instance.GetMinTaskTime(x)).ToArray();

                // Rule 4
                if (ruleSecondaryCriteria == RuleSecondaryCriteria.Max)
                    return unassignedTasks.OrderByDescending(x => instance.GetMaxTaskTime(x)).ToArray();

                // Rule 5
                if (ruleSecondaryCriteria == RuleSecondaryCriteria.Average)
                    return unassignedTasks.OrderByDescending(x => instance.GetAverageTaskTime(x)).ToArray();
            }

            if (ruleCriteria == RuleCriteria.MinTime)
            {
                // Rule 6
                if (ruleSecondaryCriteria == RuleSecondaryCriteria.Min)
                    return unassignedTasks.OrderBy(x => instance.GetMinTaskTime(x)).ToArray();

                // Rule 7
                if (ruleSecondaryCriteria == RuleSecondaryCriteria.Max)
                    return unassignedTasks.OrderBy(x => instance.GetMaxTaskTime(x)).ToArray();

                // Rule 8
                if (ruleSecondaryCriteria == RuleSecondaryCriteria.Average)
                    return unassignedTasks.OrderBy(x => instance.GetAverageTaskTime(x)).ToArray();
            }

            if (ruleCriteria == RuleCriteria.MaxPW)
            {
                // Rule 9
                if (ruleSecondaryCriteria == RuleSecondaryCriteria.Min)
                    return unassignedTasks.OrderByDescending(x => instance.GetMinPositionalWeight(x)).ToArray();

                // Rule 10
                if (ruleSecondaryCriteria == RuleSecondaryCriteria.Max)
                    return unassignedTasks.OrderByDescending(x => instance.GetMaxPositionalWeight(x)).ToArray();

                // Rule 11
                if (ruleSecondaryCriteria == RuleSecondaryCriteria.Average)
                    return unassignedTasks.OrderByDescending(x => instance.GetAveragePositionalWeight(x)).ToArray();
            }

            if (!worker.HasValue) return null;

            // Rule 12
            if (ruleCriteria == RuleCriteria.MinD)
                return unassignedTasks.OrderBy(x => instance.GetDifferenceToBestWorker(x, worker.Value)).ToArray();

            // Rule 13
            if (ruleCriteria == RuleCriteria.MinR)
                return unassignedTasks.OrderBy(x => instance.GetRatioToBestWorker(x, worker.Value)).ToArray();

            // Rule 14
            if (ruleCriteria == RuleCriteria.MaxFTime)
                return unassignedTasks.OrderByDescending(x => instance.GetNumberOfFollowersPerTime(x, worker.Value)).ToArray();

            // Rule 15
            if (ruleCriteria == RuleCriteria.MaxIFTime)
                return unassignedTasks.OrderByDescending(x => instance.GetNumberOfImmediateFollowersPerTime(x, worker.Value)).ToArray();

            // Rule 16
            if (ruleCriteria == RuleCriteria.MinRank)
                return unassignedTasks.OrderBy(x => instance.GetRank(x, worker.Value)).ToArray();

            return null;
        }

        public static List<RuleSecondaryCriteria> GetSecondaryCriterias(RuleCriteria criteria)
        {
            List<RuleSecondaryCriteria> secondaryCriterias = new();

            if (criteria == RuleCriteria.MaxTime
                || criteria == RuleCriteria.MinTime
                || criteria == RuleCriteria.MaxPW)
            {
                secondaryCriterias.AddRange(new List<RuleSecondaryCriteria>()
                    {
                        RuleSecondaryCriteria.Max,
                        RuleSecondaryCriteria.Min,
                        RuleSecondaryCriteria.Average
                    });
            }
            else
            {
                secondaryCriterias.Add(RuleSecondaryCriteria.None);
            }

            return secondaryCriterias;
        }
    }
}
