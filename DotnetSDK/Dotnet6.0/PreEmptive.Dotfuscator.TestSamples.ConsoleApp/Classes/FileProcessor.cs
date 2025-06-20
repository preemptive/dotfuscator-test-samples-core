using PreEmptive.Dotfuscator.TestSamples.ConsoleApp.Abstracts;
using System.Reflection;

namespace PreEmptive.Dotfuscator.TestSamples.ConsoleApp.Classes
{
    public class FileProcessor : ProcessorBase
    {
        public override void Process()
        {
            Console.WriteLine("Reading from embedded resource:");
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("PreEmptive.Dotfuscator.TestSamples.ConsoleApp.Resources.Sample.txt");
            using var reader = new StreamReader(stream!);
            
            Console.WriteLine(reader.ReadToEnd());
            Console.ReadLine();
        }
    }
}