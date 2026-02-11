using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PreEmptive.Dotfuscator.Samples.Core.Models;

namespace PreEmptive.Dotfuscator.Samples.Core.Abstractions
{
    internal interface IWorkflowExecutor
    {
        Task ExecuteAsync(IEnumerable<StepContext> steps, CancellationToken cancellationToken = default(CancellationToken));
    }
}
