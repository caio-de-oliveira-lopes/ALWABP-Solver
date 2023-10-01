using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALWABP.Domain.Base
{
    public abstract class Heuristic : Entity
    {
        private static int InstanceCounter { get; set; } = 0;

        public Heuristic(string name)
            : base(InstanceCounter, $"{name}_({InstanceCounter})")
        {
            InstanceCounter++;
        }
    }
}
