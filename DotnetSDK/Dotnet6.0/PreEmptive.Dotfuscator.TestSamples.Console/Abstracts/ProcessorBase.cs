using MyApp.Interfaces;

namespace MyApp.Abstracts
{
    public abstract class ProcessorBase : IProcessor
    {
        public virtual void Process()
        {
            Console.WriteLine("Default processing from base class.");
        }
    }
}