using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Models;
using PreEmptive.Dotfuscator.Samples.Core.Services;

namespace PreEmptive.Dotfuscator.Samples.Blazor;

public class WorkflowService
{
    private readonly IWorkflowExecutor _workflowExecutor;

    public WorkflowService()
    {
        _workflowExecutor = new WorkflowExecutor(new MessageCollectorStepProcessor());
    }

    public async Task<string> RunAsync(IEnumerable<StepContext> steps)
    {
        await _workflowExecutor.ExecuteAsync(steps);
        return MessageCollectorStepProcessor.CollectOutput();
    }
}
