using MyApp.Abstracts;
using System.Reflection;

namespace MyApp.Classes
{
    public class FileProcessor : ProcessorBase
    {
        static public void Process()
        {
            Console.WriteLine("Reading from embedded resource:");
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("PreEmptive.Dotfuscator.TestSamples.Console.Resources.Sample.txt");
            using var reader = new StreamReader(stream!);
            
            Console.WriteLine(reader.ReadToEnd());
            Console.ReadLine();
        }
    }
}