using PreEmptive.Dotfuscator.Samples.ConsoleApp.Abstracts;
using PreEmptive.Dotfuscator.Samples.ConsoleApp.Interfaces;
using System.Reflection;

namespace PreEmptive.Dotfuscator.Samples.ConsoleApp.Classes
{
    public class FileProcessor : ProcessorBase
    {
        public override void Process()
        {
            Console.WriteLine("Reading from embedded resource:");
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("PreEmptive.Dotfuscator.Samples.ConsoleApp.Resources.Sample.txt");
            using var reader = new StreamReader(stream!);
            
            Console.WriteLine(reader.ReadToEnd());
            
        }
    }

    // abstract static implementation
    public class SystemProcessHandler : IProcessFactory<SystemProcessHandler>
    {
        private int ProcessId;

        private SystemProcessHandler(int processId)
        {
            ProcessId = processId;
        }

        public static SystemProcessHandler CreateFromId(int processId)
        {
            return new SystemProcessHandler(processId);
        }

        public void DisplayProcessDetails()
        {
            Console.WriteLine($"[System] Process ID: {ProcessId}, Status: Running, CPU: 14%");
        }
    }

    public class BusinessProcessHandler : IProcessFactory<BusinessProcessHandler>
    {
        private int ProcessId;

        private BusinessProcessHandler(int processId)
        {
            ProcessId = processId;
        }

        public static BusinessProcessHandler CreateFromId(int processId)
        {
            return new BusinessProcessHandler(processId);
        }

        public void DisplayProcessDetails()
        {
            Console.WriteLine($"[Business] Process ID: {ProcessId}, Stage: Completed");
        }
    }
}