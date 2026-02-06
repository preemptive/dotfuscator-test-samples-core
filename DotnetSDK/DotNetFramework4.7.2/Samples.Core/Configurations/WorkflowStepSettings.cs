using System.Collections.Generic;

namespace PreEmptive.Dotfuscator.Samples.Core.Configurations
{
    public class WorkflowStepSettings
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public Dictionary<string, object> Arguments { get; set; }

        public WorkflowStepSettings()
        {
            Name = string.Empty;
            Type = string.Empty;
            Arguments = new Dictionary<string, object>();
        }
    }
}
