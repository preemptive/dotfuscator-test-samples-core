using System;

namespace PreEmptive.Dotfuscator.Samples.Core.Configurations
{
    public class WorkflowSettings
    {
        public WorkflowStepSettings[] Steps { get; set; } = Array.Empty<WorkflowStepSettings>();
    }
}
