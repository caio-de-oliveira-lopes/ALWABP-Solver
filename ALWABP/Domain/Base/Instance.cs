using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALWABP.Domain.Base
{
    public abstract class Instance : Entity
    {
        private static int InstanceCounter { get; set; } = 0;
        public InstanceType Type { get; private set; }
        public FileManager Manager { get; private set; }
        public int Tasks { get; private set; }
        public (int, int)[] PrecedenceGraph { get; private set; }
        public Dictionary<int, List<int>> ImmediateFollowers { get; private set; }
        public Dictionary<int, List<int>> Followers { get; private set; }
        public Solution? Solution { get; protected set; }

        public Instance(InstanceType type, FileManager manager, int tasks, (int, int)[] precedenceGraph)
            : base(InstanceCounter, $"{manager.InputFileName}_({InstanceCounter})")
        {
            InstanceCounter++;
            Type = type;
            Manager = manager;
            Tasks = tasks;
            PrecedenceGraph = precedenceGraph;
            ImmediateFollowers = new Dictionary<int, List<int>>();
            Followers = new Dictionary<int, List<int>>();

            ComputeImmediateFollowers();
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

        public bool HasSolution()
        {
            return Solution != null;
        }

        private void ComputeImmediateFollowers()
        {
            ImmediateFollowers.Clear();
            GetTasksList().ForEach(t => ImmediateFollowers.Add(t, new List<int>()));

            foreach (var pair in PrecedenceGraph) 
                ImmediateFollowers[pair.Item1].Add(pair.Item2);
        }

        private void ComputeFollowers()
        {
            Followers.Clear();
            foreach (var task in GetTasksList())
                Followers.Add(task, new List<int>(GetFollowers(task)));
        }

        private List<int> GetFollowers(int task) 
        {
            var followers = new List<int>(ImmediateFollowers[task]);
            followers.ToList().ForEach(x => followers.AddRange(GetFollowers(x)));

            return followers.Distinct().ToList();
        }
    }


}
