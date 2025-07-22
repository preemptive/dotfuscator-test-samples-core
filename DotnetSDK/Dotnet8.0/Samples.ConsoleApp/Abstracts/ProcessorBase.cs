using PreEmptive.Dotfuscator.Samples.ConsoleApp.Interfaces;

namespace PreEmptive.Dotfuscator.Samples.ConsoleApp.Abstracts
{
    public abstract class ProcessorBase : IProcessor
    {
        public virtual void Process()
        {
            Console.WriteLine("Default processing from base class.");
        }
    }
}