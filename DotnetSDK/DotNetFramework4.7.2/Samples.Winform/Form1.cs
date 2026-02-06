using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Lib;
using PreEmptive.Dotfuscator.Samples.Core.Services;

namespace PreEmptive.Dotfuscator.Samples.Winform
{
    public partial class Form1 : Form
    {
		private readonly IWorkflowExecutor _workflowExecutor;
		public Form1()
        {
            InitializeComponent();
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
    }
}
