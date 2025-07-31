using System.Diagnostics;
using System.Text.RegularExpressions;
using PreEmptive.Dotfuscator.Samples.Common.ConsoleApp.Classes;

namespace PreEmptive.Dotfuscator.Samples.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            

            string current = Directory.GetCurrentDirectory();
            string projectPath = Path.GetFullPath(Path.Combine(current, @"..\..\..\"));
            int processId = Process.GetCurrentProcess().Id;

            Console.WriteLine("Current Path = {0}", current);
            Console.WriteLine("ProjectPath =  {0}", projectPath);
            Console.WriteLine("Process ID  =  {0}", processId);

            // Call FileProcessor logic (correct static or instance call depending on your definition)
            var processor = new FileProcessor();
            processor.Process();

            // Prepare log path
            string logFolder = Path.Combine(projectPath, "Resources");
            string logPath = Path.Combine(logFolder, "log.txt");

            // Ensure the directory exists
            Directory.CreateDirectory(logFolder);

            // Write to log
            File.WriteAllText(logPath, $"Current Path: {current}{Environment.NewLine}" +
                                       $"Project Path: {projectPath}{Environment.NewLine}" +
                                       "File Written Successfully");

            Console.WriteLine("Processes executed. Log file created.");


            // Implementation call for abstract static members
            var sysHandler = SystemProcessHandler.CreateFromId(1001);
            var bizHandler = BusinessProcessHandler.CreateFromId(2002);

            sysHandler.DisplayProcessDetails();
            bizHandler.DisplayProcessDetails();


        }
    }
}
