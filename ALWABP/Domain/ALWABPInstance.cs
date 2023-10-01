using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALWABP.Domain
{
    public class ALWABPInstance : Instance
    {
        public ALWABPInstance(string inputFileName, string inputFilePath, string outputFilePath) 
            : base(InstanceType.ALWABP, inputFileName, inputFilePath, outputFilePath) 
        {

        }
    }
}
