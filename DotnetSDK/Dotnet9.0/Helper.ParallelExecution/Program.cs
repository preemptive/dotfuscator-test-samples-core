using System.Diagnostics;

namespace PreEmptive.Dotfuscator.Samples.Helper.ParallelExecution
{
    internal class Program
    {
        private static string exePath = string.Empty;
        private static string arguments = string.Empty;
        private const int noOfDotInstances = 5;
        static void Main(string[] args)
        {
            exePath = GetDotfuscatorExe(args);
            arguments = GetDotfuscatorArguments(args);
            string obfusctedAssemblyDirectory = GetObfuscatedAssemblyDirectory(args);

            Console.WriteLine("ExePath =  {0}", exePath);
            Console.WriteLine("obfusctedAssemblyDirectory =  {0}", obfusctedAssemblyDirectory);

            var processes = Process.GetProcessesByName("dotfuscator.exe");
            foreach (var process in processes)
            {
                process.Kill(true);
            }

            if (Directory.Exists(obfusctedAssemblyDirectory))
            {
                var directories = Directory.GetDirectories(obfusctedAssemblyDirectory);

                var directories1 = directories.Where(dir => dir.Contains("Dotfuscated")).ToList();
                foreach (var directories2 in directories1)
                {
                    Directory.Delete(directories2, true);
                }
            }

            Task[] tasks = new Task[noOfDotInstances];
            try
            {
                if (!File.Exists(exePath))
                {
                    Console.WriteLine("File {0} does not exist", exePath);
                    return;
                }

                if (!File.Exists(arguments))
                {
                    Console.WriteLine("Config file {0} does not exist", arguments);
                    return;
                }

                for (var i = 0; i < tasks.Length; i++)
                {
                    tasks[i] = new Task(ExecuteDotfuscator);
                }

                for (var i = 0; i < tasks.Length; i++)
                {
                    tasks[i].Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return;
            }
            Task.WaitAll(tasks);

            Console.WriteLine("Processes executed");
            Console.ReadLine();
        }

        private static string GetObfuscatedAssemblyDirectory(string[] args)
        {
            if (args.Length > 0)
            {
                return args[0];
            }

            var current = Directory.GetCurrentDirectory();
            var projectPath = Path.GetFullPath(Path.Combine(current, @"..\..\..\..\Samples.ConsoleApp\"));
            return Path.Combine(projectPath, "bin", "Release", "net9.0");
        }

        private static string GetDotfuscatorArguments(string[] args)
        {
            if (args.Length > 1)
            {
                return args[1];
            }

            var current = Directory.GetCurrentDirectory();
            var projectPath = Path.GetFullPath(Path.Combine(current, @"..\..\..\..\Samples.ConsoleApp\"));
            return projectPath + "TestSample.xml";
        }

        private static string GetDotfuscatorExe(string[] args)
        {
            if (args.Length > 2)
            {
                return args[2];
            }

            var dotfuscatorHome = Environment.GetEnvironmentVariable("DOTFUSCATOR_HOME");
            if (string.IsNullOrEmpty(dotfuscatorHome))
            {
                Console.WriteLine("DOTFUSCATOR_HOME is missing");
            }

            return dotfuscatorHome + "dotfuscator.exe";
        }

        private static void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e != null && e.Data != null)
            {
                var processId = string.Empty;
                var process = sender as Process;
                processId = process?.Id.ToString();
                Console.WriteLine("Process {0}: {1}", processId, e.Data.ToString());
            }
        }

        private static void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e != null && e.Data != null)
            {

                var processId = string.Empty;
                var process = sender as Process;
                processId = process?.Id.ToString();
                Console.WriteLine("Process {0}: {1}", processId, e.Data.ToString());
            }

        }

        private static void ExecuteDotfuscator()
        {
            var process = new Process();
            try
            {
                process = new Process();
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.FileName = exePath;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.OutputDataReceived += Process_OutputDataReceived;
                process.ErrorDataReceived += Process_ErrorDataReceived; ;
                process.Start();
                process.BeginErrorReadLine();
                process.BeginOutputReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return;
            }
            while (process != null && !process.HasExited)
            {
                continue;
            }
            Console.WriteLine("Process executed");
        }
    }
}
