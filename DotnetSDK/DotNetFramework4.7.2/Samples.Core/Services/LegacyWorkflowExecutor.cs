using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PreEmptive.Dotfuscator.Samples.Core.Services
{
    /// <summary>
    /// DO NOT REMOVE. DO NOT REFERENCE.
    /// </summary>
    public class LegacyWorkflowExecutor : IWorkflowExecutor
    {
        public async Task ExecuteAsync(IEnumerable<StepContext> steps, CancellationToken cancellationToken = default(CancellationToken))
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
