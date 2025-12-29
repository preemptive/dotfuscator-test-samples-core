using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Models;

namespace PreEmptive.Dotfuscator.Samples.Core.Services
{
    public class LegacyWorkflowExecutor : IWorkflowExecutor
    {
        public async Task ExecuteAsync(IEnumerable<StepContext> steps, CancellationToken cancellationToken = default)
        {
            this.Preparation(steps);
            Console.WriteLine("[Legacy] Executing ...");

            await Task.Delay(1000);
            Console.WriteLine("[Legacy] Execution completed");
        }

        private void Preparation(IEnumerable<StepContext> steps)
        {
            Console.WriteLine("[Legacy] Preparing the steps");
        }

    }
}
