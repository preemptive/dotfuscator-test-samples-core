using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Models;
using System;

namespace PreEmptive.Dotfuscator.Samples.Core.Services
{
    public class ConsoleOutputStepProcessor : IStepResultProcessor
    {
        public void Process(StepMetadata metadata, StepResult stepResult)
        {
            if (stepResult == null)
                throw new ArgumentNullException(nameof(stepResult));

            if (!stepResult.IsSuccess)
            {
                Console.WriteLine($"{metadata.Name} has FAILED due to REASON {stepResult.Message}");
                return;
            }

            Console.WriteLine($"{metadata.Name} has SUCCEEDED.");
            if (stepResult.Value != null)
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
