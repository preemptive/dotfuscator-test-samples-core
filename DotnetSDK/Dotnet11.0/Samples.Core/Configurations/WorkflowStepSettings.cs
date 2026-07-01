namespace PreEmptive.Dotfuscator.Samples.Core.Configurations
{
    public class WorkflowStepSettings
    {
        public string Name { get; set; } = null!;

        public string Type { get; set; } = null!;

        public Dictionary<string, object> Arguments { get; set; } = new();
    }
}
