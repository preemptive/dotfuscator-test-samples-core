using Samples.Common.Interfaces;

namespace Samples.Common.Abstracts
{
    public abstract class ProcessorBase : IProcessor
    {
        public virtual void Process()
        {
            Console.WriteLine("Default processing from base class.");
        }
    }
}