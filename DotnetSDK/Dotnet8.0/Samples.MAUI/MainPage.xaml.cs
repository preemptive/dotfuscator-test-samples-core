using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Extensions;
using PreEmptive.Dotfuscator.Samples.Core.Lib;
using PreEmptive.Dotfuscator.Samples.Core.Services;
using PreEmptive.Dotfuscator.Samples.Core.Services.StepProcessors;

namespace Samples.Maui
{
    public partial class MainPage : ContentPage
    {
        private readonly IWorkflowExecutor _workflowExecutor;

        public MainPage()
        {
            InitializeComponent();
            _workflowExecutor = new WorkflowExecutor(new MessageCollectorStepProcessor());
        }

        private async void OnRunClicked(object? sender, EventArgs e)
        {
            var steps = StepsContextFactory.Create(ServiceManager.ServiceProvider.GetRequiredService<IEnumerable<IStepProcessor>>());

            ArgumentsCollector.Instance.PushDefaultArguments();
            ArgumentsCollector.Instance.PushArgument("WriteToTextFile", nameof(WriteToFileStepProcessor.OutputPath), FileSystem.Current.AppDataDirectory);

            await _workflowExecutor.ExecuteAsync(steps);

            var output = MessageCollectorStepProcessor.CollectOutput();
            await DisplayAlert("Execution result", output, "OK");
        }
    }

}
