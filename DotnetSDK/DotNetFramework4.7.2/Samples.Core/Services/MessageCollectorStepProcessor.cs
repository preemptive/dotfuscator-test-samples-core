using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Models;
using System;
using System.Text;

namespace PreEmptive.Dotfuscator.Samples.Core.Services
{
    public class MessageCollectorStepProcessor : IStepResultProcessor
    {
        private static StringBuilder stringBuilder = new StringBuilder();

        public static string CollectOutput()
        {
            var result = stringBuilder.ToString();
            stringBuilder.Clear();
            return result;
        }

        public void Process(StepMetadata metadata, StepResult stepResult)
        {
            if (stepResult == null)
                throw new ArgumentNullException(nameof(stepResult));

            if (!stepResult.IsSuccess)
            {
                stringBuilder.AppendLine($"{metadata.Name} has FAILED due to REASON {stepResult.Message}");
                return;
            }

            stringBuilder.AppendLine($"{metadata.Name} has SUCCEEDED.");
            if (stepResult.Value != null)
            {
                stringBuilder.AppendLine($"----- VALUE: {stepResult.Value}");
            }

            if (!string.IsNullOrEmpty(stepResult.Message))
            {
                stringBuilder.AppendLine($"----- MESSAGE: {stepResult.Message}");
            }
        }
    }
}
