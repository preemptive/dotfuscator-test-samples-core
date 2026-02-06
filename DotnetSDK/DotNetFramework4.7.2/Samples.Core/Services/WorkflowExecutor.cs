using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Extensions;
using PreEmptive.Dotfuscator.Samples.Core.Models;

namespace PreEmptive.Dotfuscator.Samples.Core.Services
{
    public class WorkflowExecutor : IWorkflowExecutor
    {
        private readonly IStepResultProcessor resultProcessor;

        public WorkflowExecutor(IStepResultProcessor resultProcessor)
        {
            this.resultProcessor = resultProcessor ?? throw new ArgumentNullException(nameof(resultProcessor));
        }

        public async Task ExecuteAsync(IEnumerable<StepContext> steps, CancellationToken cancellationToken = default)
        {
            if (steps == null)
                throw new ArgumentNullException(nameof(steps));

            if (!ArgumentsCollector.Instance.HasArguments())
            {
                ArgumentsCollector.Instance.PushDefaultArguments();
            }

            try
            {
                foreach (var step in steps)
                {
                    var stepResult = await step.ExecuteAsync(cancellationToken);
                    resultProcessor.Process(step.Metadata, stepResult);
                }
            }
            catch (Exception ex)
            {
                //TODO: Replace this console writeline with some sort of logger.
                Console.WriteLine(Constants.ErrorCodes.InternalError, ex);
            }

        }
    }
}
