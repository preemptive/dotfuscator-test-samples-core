using PreEmptive.Dotfuscator.Samples.Core.Configurations; // if WorkflowSettings is in this namespace

namespace PreEmptive.Dotfuscator.Samples.Core.Configurations
{
    public class Settings
    {
        public WorkflowSettings Workflow { get; set; }

        public Settings()
        {
            Workflow = new WorkflowSettings();
        }
    }
}
