using PreEmptive.Dotfuscator.Samples.Core.Abstractions;

namespace PreEmptive.Dotfuscator.Samples.Core.Extensions
{
    internal static class ArgumentsCollectorExtensions
    {
        public static void PushDefaultArguments(this IArgumentsCollector collector)
        {
            ArgumentNullException.ThrowIfNull(collector);

            var workflow = SettingsProvider.Settings.Workflow;
            var workflowSteps = workflow.Steps;
            foreach (var step in workflowSteps)
            {
                foreach (var argument in step.Arguments)
                {
                    collector.PushArgument(step.Name, argument.Key, argument.Value);
                }
            }
        }
    }
}
