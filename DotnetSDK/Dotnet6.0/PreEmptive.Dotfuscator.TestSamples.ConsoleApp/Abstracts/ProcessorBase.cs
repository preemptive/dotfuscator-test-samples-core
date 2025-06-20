using PreEmptive.Dotfuscator.TestSamples.ConsoleApp.Interfaces;

namespace PreEmptive.Dotfuscator.TestSamples.ConsoleApp.Abstracts
{
    public abstract class ProcessorBase : IProcessor
    {
        public virtual void Process()
        {
            Console.WriteLine("Default processing from base class.");
        }
    }
}