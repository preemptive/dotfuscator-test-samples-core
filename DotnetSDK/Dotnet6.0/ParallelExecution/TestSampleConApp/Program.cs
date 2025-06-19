using System.Text.RegularExpressions;
using MyApp.Classes;

namespace TestSampleConApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            var processor = new FileProcessor();
            FileProcessor.Process();
            

        }
    }
}
