using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PreEmptive.Dotfuscator.Samples.Core;
using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Extensions;
using PreEmptive.Dotfuscator.Samples.Core.Lib;
using PreEmptive.Dotfuscator.Samples.Core.Services;
using ConfigurationManager = PreEmptive.Dotfuscator.Samples.Core.Lib.ConfigurationManager;

namespace PreEmptive.Dotfuscator.Samples.WinForms
{
    public class MainForm : Form
    {
        private readonly IWorkflowExecutor _workflowExecutor;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            button1 = new Button();
            comboBox1 = new ComboBox();
            panel1 = new Panel();
            SuspendLayout();
            // 
            // button1
            // 
            resources.ApplyResources(button1, "button1");
            button1.Name = "button1";
            button1.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            resources.ApplyResources(comboBox1, "comboBox1");
            comboBox1.Name = "comboBox1";
            // 
            // panel1
            // 
            resources.ApplyResources(panel1, "panel1");
            panel1.Name = "panel1";
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            Controls.Add(panel1);
            Controls.Add(comboBox1);
            Controls.Add(button1);
            Name = "MainForm";
            ResumeLayout(false);

        }

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
                ConfigurationManager.Builder
                    .AddJsonFile($"Core\\{Constants.CoreAppsettings}")
                    .AddJsonFile("appsettings.json");

                Application.Run(new MainForm());
            }
        }
        private Button button1;
        private ComboBox comboBox1;
        private Panel panel1;
    }
}
