using Microsoft.Extensions.DependencyInjection;
using PreEmptive.Dotfuscator.Samples.Core;
using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Extensions;
using PreEmptive.Dotfuscator.Samples.Core.Lib;
using PreEmptive.Dotfuscator.Samples.Core.Services;

namespace PreEmptive.Dotfuscator.Samples.WinForms
{
    public class MainForm : Form
    {
        private readonly IWorkflowExecutor _workflowExecutor;

        public MainForm()
        {
            Text = "WinForms Sample App";
            Width = 400;
            Height = 300;


            _workflowExecutor = new WorkflowExecutor(new MessageCollectorStepProcessor());

            var button = new Button { Text = "Run", Dock = DockStyle.Fill };
            button.Click += async (s, e) =>
            {
                var steps = StepsContextFactory.Create(ServiceManager.ServiceProvider.GetRequiredService<IEnumerable<IStepProcessor>>());
                await _workflowExecutor.ExecuteAsync(steps);

                var output = MessageCollectorStepProcessor.CollectOutput();
                MessageBox.Show(output, "Execution result", MessageBoxButtons.OK);
            };
            Controls.Add(button);
        }

        class Program
        {
            [STAThread]
            static void Main()
            {
                Application.EnableVisualStyles();
                
                ServiceManager.Services.AddStepsProcessors();

                ConfigurationManager.AddSource($"Core\\{Constants.CoreAppsettings}");
                ConfigurationManager.AddSource("appsettings.json");

                Application.Run(new MainForm());
            }
        }
    }
}
