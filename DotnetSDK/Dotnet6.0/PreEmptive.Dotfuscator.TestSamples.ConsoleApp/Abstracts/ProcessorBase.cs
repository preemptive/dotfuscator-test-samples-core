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
    public abstract class ProcessHandler
    {
        // Abstract method to be implemented by subclasses
        public abstract void DisplayProcessDetails(int processId);
    }


}