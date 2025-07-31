using Samples.Common.Abstracts;
using System.Reflection;

namespace Samples.Common.Classes
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
    class SystemProcessHandler : ProcessHandler
    {
        public override void DisplayProcessDetails(int processId)
        {
            // Simulate fetching process info (in a real app, use System.Diagnostics)
            Console.WriteLine($"[SystemProcessHandler] Process ID: {processId}");
            Console.WriteLine("Status: Running");
            Console.WriteLine("Memory Usage: 120 MB");
        }
    }

    class BusinessProcessHandler : ProcessHandler
    {
        public override void DisplayProcessDetails(int processId)
        {
            // Simulate fetching business process info
            Console.WriteLine($"[BusinessProcessHandler] Process ID: {processId}");
            Console.WriteLine("Stage: Approval");
            Console.WriteLine("Assigned To: John Doe");
        }
    }
}