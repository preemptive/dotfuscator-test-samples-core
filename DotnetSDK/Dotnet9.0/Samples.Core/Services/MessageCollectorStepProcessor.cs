using System.Text;
using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Models;

namespace PreEmptive.Dotfuscator.Samples.Core.Services
{
    public class MessageCollectorStepProcessor : IStepResultProcessor
    {
        private static StringBuilder stringBuilder = new();

        public static string CollectOutput()
        {
            var result = stringBuilder.ToString();
            stringBuilder.Clear();
            return result;
        }

        public void Process(StepMetadata metadata, StepResult stepResult)
        {
            ArgumentNullException.ThrowIfNull(stepResult);

            if (!stepResult.IsSuccess)
            {
                stringBuilder.AppendLine($"{metadata.Name} has FAILED due to REASON {stepResult.Message}");
                return;
            }

            stringBuilder.AppendLine($"{metadata.Name} has SUCCEEDED.");
            if (stepResult.Value is not null)
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
