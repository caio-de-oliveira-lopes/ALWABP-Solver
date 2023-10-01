using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALWABP.Domain
{
    public abstract class Instance : Entity
    {
        private static int InstanceCounter { get; set; } = 0;

        public InstanceType Type { get; private set; }

        public FileManager Manager { get; private set; }

        public Instance(InstanceType type, string inputFileName, string inputFilePath, string outputFilePath) 
            : base(InstanceCounter++, inputFileName)
        {
            Type = type;
            Manager = new FileManager(inputFileName, inputFilePath, outputFilePath);
        }

        public enum InstanceType
        {
            SALBP,
            ALWABP
        }
    }


}
