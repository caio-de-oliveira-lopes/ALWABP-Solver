namespace ALWABP.Domain.Base
{
    public abstract class Instance : Entity
    {
        private static int InstanceCounter { get; set; } = 0;
        public InstanceType Type { get; private set; }
        public FileManager Manager { get; private set; }
        public int Tasks { get; private set; }
        public Dictionary<GraphDirection, (int, int)[]> PrecedenceGraph { get; private set; }
        public Dictionary<int, List<int>> ImmediateFollowers { get; private set; }
        public Dictionary<int, List<int>> Followers { get; private set; }

        public Instance(InstanceType type, FileManager manager, int tasks, (int, int)[] precedenceGraph)
            : base(InstanceCounter, $"{manager.InputFileName}_({InstanceCounter})")
        {
            InstanceCounter++;
            Type = type;
            Manager = manager;
            Tasks = tasks;
            PrecedenceGraph = new Dictionary<GraphDirection, (int, int)[]>()
            {
                { GraphDirection.Forward, precedenceGraph },
                { GraphDirection.Backward, precedenceGraph.Select(x => (x.Item2, x.Item1)).ToArray() }
            };
            ImmediateFollowers = new Dictionary<int, List<int>>();
            Followers = new Dictionary<int, List<int>>();
        }

        public enum GraphDirection
        {
            Forward,
            Backward
        }

        public void ComputeData(GraphDirection graphDirection)
        {
            ComputeImmediateFollowers(graphDirection);
            ComputeFollowers();
        }

        public enum InstanceType
        {
            SALBP,
            ALWABP
        }

        public List<int> GetTasksList()
        {
            return Enumerable.Range(0, Tasks).ToList();
        }

        private void ComputeImmediateFollowers(GraphDirection graphDirection)
        {
            ImmediateFollowers.Clear();
            GetTasksList().ForEach(t => ImmediateFollowers.Add(t, new List<int>()));

            foreach ((int, int) pair in PrecedenceGraph[graphDirection])
                ImmediateFollowers[pair.Item1].Add(pair.Item2);
        }

        private void ComputeFollowers()
        {
            Followers.Clear();
            foreach (int task in GetTasksList())
                Followers.Add(task, new List<int>(GetFollowers(task)));
        }

        private List<int> GetFollowers(int task)
        {
            List<int> followers = new(ImmediateFollowers[task]);
            followers.ToList().ForEach(x => followers.AddRange(GetFollowers(x)));

            return followers.Distinct().ToList();
        }
    }


}
