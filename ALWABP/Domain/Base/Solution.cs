using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALWABP.Domain.Base
{
    public abstract class Solution : Entity
    {
        private static int InstanceCounter { get; set; } = 0;

        public Solution() 
            : base(InstanceCounter, $"Sol_({InstanceCounter})")
        {
            InstanceCounter++;
        }
    }
}
