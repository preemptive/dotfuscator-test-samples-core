using System.Reflection;
using System.Text;
using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Models;

namespace PreEmptive.Dotfuscator.Samples.Core.Services.StepProcessors
{
    public class ReadEmbeddedResourceMetdataStepProcessor : StepProcessorBase
    {
        protected override Task<StepResult> ExecuteInternalAsync(CancellationToken cancellationToken = default)
        {
            var result = new StringBuilder();
            var isSuccess = true;
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var resourceName in assembly.GetManifestResourceNames())
            {
                using var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream is null)
                {
                    isSuccess = false;
                    result.AppendLine($"Resource: {resourceName} - unable to read the resource");
                }
                else
                {
                    result.AppendLine($"Resource: {resourceName} - Size: {stream.Length}");
                }

            }

            if (!isSuccess)
            {
                return Task.FromResult(StepResult.Failure(result.ToString()));
            }

            return Task.FromResult(StepResult.Success(value: result.ToString()));
        }
    }
}
