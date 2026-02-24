using PreEmptive.Dotfuscator.Samples.Core.Models;

namespace PreEmptive.Dotfuscator.Samples.Core.Abstractions
{
    public interface IStepResultProcessor
    {
        void Process(StepMetadata metadata, StepResult stepResult);
    }
}
