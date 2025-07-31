using PreEmptive.Dotfuscator.Samples.Common.ConsoleApp.Interfaces;

namespace PreEmptive.Dotfuscator.Samples.Common.ConsoleApp.Abstracts
{
    public abstract class ProcessorBase : IProcessor
    {
        public virtual void Process()
        {
            Console.WriteLine("Default processing from base class.");
        }
    }
}