using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALWABP.Domain.ALWABP;
using ALWABP.Domain.Base;
using ALWABP.Utils;

namespace ALWABP.DataManagers
{
    public static class Writer
    {
        public static void WriteOutputFile<T>(T instance) where T : Instance
        {
            if (typeof(T) == typeof(ALWABPInstance))
                WriteALWABPOutput(instance as ALWABPInstance);
        }

        public static void WriteALWABPOutput(ALWABPInstance? instance)
        {
            if (instance == null) return;


        }
    }
}
