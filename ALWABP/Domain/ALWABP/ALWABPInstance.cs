using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALWABP.Domain.Base;

namespace ALWABP.Domain.ALWABP
{
    public class ALWABPInstance : Instance
    {
        public int Workers { get; private set; }
        public int?[,] Matrix { get; private set; }

        public ALWABPInstance(FileManager fileManager, int workers, int tasks, int?[,] matrix, (int, int)[] precedenceGraph)
            : base(InstanceType.ALWABP, fileManager, tasks, precedenceGraph)
        {
            Workers = workers;
            Matrix = matrix;
        }
    }
}
