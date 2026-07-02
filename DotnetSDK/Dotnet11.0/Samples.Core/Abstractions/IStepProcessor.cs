using PreEmptive.Dotfuscator.Samples.Core.Models;

namespace PreEmptive.Dotfuscator.Samples.Core.Abstractions
{
    public interface IStepProcessor
    {
        Task<StepResult> ExecuteAsync(CancellationToken cancellationToken = default);
    }
}
