using PreEmptive.Dotfuscator.Samples.Core.Models;

namespace PreEmptive.Dotfuscator.Samples.Core.Abstractions
{
    public interface IStepResultProcessor
    {
        /// <summary>
        /// Abstraction of how a step result is processed. The implementation should be framework specific.
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="stepResult"></param>
        void Process(StepMetadata metadata, StepResult stepResult);
    }
}
