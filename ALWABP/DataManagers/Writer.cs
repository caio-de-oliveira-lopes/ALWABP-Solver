using ALWABP.Domain.ALWABP;
using ALWABP.Domain.Base;
using System.Text.Json;
using System.Text.Json.Serialization;

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

            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(instance.OutputAsDictionary(), options);
            //var solutions = instance.Solutions.Values.Select(x => x.Id).ToList();

            File.WriteAllText(instance.Manager.GetFullOutputPath(), json);
            Console.WriteLine($"\nFinished Writing Output For Instance {instance.Name}.\n");
        }
    }
}
