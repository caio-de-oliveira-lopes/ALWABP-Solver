using ALWABP.Domain.Base;
using static ALWABP.Domain.Base.Instance;

namespace ALWABP.Domain.ALWABP
{
    public class ALWABPSolution : Solution
    {
        public Dictionary<int, int> WorkstationsCicleTimes { get; private set; }
        public Dictionary<int, int> AssignedWorkers { get; private set; }
        public Dictionary<int, List<int>> WorkerTasks { get; private set; }
        public GraphDirection GraphDirection { get; private set; }
        public WorkerPriorityRule.RuleCriteria WorkerRuleCriteria { get; private set; }
        public TaskPriorityRule.RuleCriteria TaskRuleCriteria { get; private set; }
        public TaskPriorityRule.RuleSecondaryCriteria TaskRuleSecondaryCriteria { get; private set; }

        public ALWABPSolution(Dictionary<int, int> workstationsCicleTimes, Dictionary<int, int> assignedWorkers, 
            Dictionary<int, List<int>> workerTasks, GraphDirection graphDirection, WorkerPriorityRule.RuleCriteria workerRuleCriteria,
            TaskPriorityRule.RuleCriteria ruleCriteria, TaskPriorityRule.RuleSecondaryCriteria ruleSecondaryCriteria)
            : base(workstationsCicleTimes.Values.Max())
        {
            WorkstationsCicleTimes = workstationsCicleTimes;
            AssignedWorkers = assignedWorkers;
            WorkerTasks = workerTasks;
            GraphDirection = graphDirection;
            WorkerRuleCriteria = workerRuleCriteria;
            TaskRuleCriteria = ruleCriteria;
            TaskRuleSecondaryCriteria = ruleSecondaryCriteria;
        }
    }
}
