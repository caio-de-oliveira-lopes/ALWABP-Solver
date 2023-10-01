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
        public Solution? Solution { get; protected set; }

        public Instance(InstanceType type, FileManager manager, int tasks, (int, int)[] precedenceGraph)
            : base(InstanceCounter, $"{manager.InputFileName}_({InstanceCounter})")
        {
            InstanceCounter++;
            Type = type;
            Manager = manager;
            Tasks = tasks;
            PrecedenceGraph = precedenceGraph;
        }

        public enum InstanceType
        {
            SALBP,
            ALWABP
        }

        public bool HasSolution()
        {
            return Solution != null;
        }
    }


}
