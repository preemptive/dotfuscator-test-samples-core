using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using System;

namespace PreEmptive.Dotfuscator.Samples.Core.Extensions
{
    internal static class ArgumentsCollectorExtensions
    {
        public static void PushDefaultArguments(this IArgumentsCollector collector)
        {
            if (collector == null)
                throw new ArgumentNullException(nameof(collector));

            var workflow = SettingsProvider.Settings.Workflow;
            var workflowSteps = workflow.Steps;
            if (workflowSteps == null)
                return;

            foreach (var step in workflowSteps)
            {
                if (step.Arguments == null)
                    continue;

                foreach (var argument in step.Arguments)
                {
                    collector.PushArgument(step.Name, argument.Key, argument.Value);
                }
            }
        }
    }
}
