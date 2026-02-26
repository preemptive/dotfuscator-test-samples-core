using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Models;
using System.Threading;
using System.Threading.Tasks;

namespace PreEmptive.Dotfuscator.Samples.Core.Services.StepProcessors
{
    public class RedundantInterfaceRemovalStepProcessor : StepProcessorBase
    {
        protected override Task<StepResult> ExecuteInternalAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var instance = new Sample2InterfaceImpl();
            var result = instance.GetSampleValue();

            return Task.FromResult(StepResult.Success(value: result, message: $"Redundant Interface Removal Step Result: {result}"));
        }
    }
}
