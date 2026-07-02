using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Models;

namespace PreEmptive.Dotfuscator.Samples.Core.Services
{
    public class ConsoleOutputStepProcessor : IStepResultProcessor
    {
        public void Process(StepMetadata metadata, StepResult stepResult)
        {
            ArgumentNullException.ThrowIfNull(stepResult);

            if (!stepResult.IsSuccess)
            {
                Console.WriteLine($"{metadata.Name} has FAILED due to REASON {stepResult.Message}");
                return;
            }

            Console.WriteLine($"{metadata.Name} has SUCCEEDED.");
            if (stepResult.Value is not null)
            {
                Console.WriteLine($"----- VALUE: {stepResult.Value}");
            }

            if (!string.IsNullOrEmpty(stepResult.Message))
            {
                Console.WriteLine($"----- MESSAGE: {stepResult.Message}");
            }
        }
    }
}
