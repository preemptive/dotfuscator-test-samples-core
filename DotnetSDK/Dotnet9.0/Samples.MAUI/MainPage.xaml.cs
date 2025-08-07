using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Lib;
using PreEmptive.Dotfuscator.Samples.Core.Services;

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
            await _workflowExecutor.ExecuteAsync(steps);

            var output = MessageCollectorStepProcessor.CollectOutput();
            await DisplayAlert("Execution result", output, "OK");
        }
    }
}
