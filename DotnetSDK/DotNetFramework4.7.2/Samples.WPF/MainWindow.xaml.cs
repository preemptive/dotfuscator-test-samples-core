using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Lib;
using PreEmptive.Dotfuscator.Samples.Core.Services;

namespace PreEmptive.Dotfuscator.Samples.WPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly IWorkflowExecutor _workflowExecutor;

		public MainWindow()
		{
			InitializeComponent();
			_workflowExecutor = new WorkflowExecutor(new MessageCollectorStepProcessor());
		}

		private async void OnRunClicked(object sender, EventArgs e)
		{
			var steps = StepsContextFactory.Create(ServiceManager.ServiceProvider.GetRequiredService<IEnumerable<IStepProcessor>>());
			await _workflowExecutor.ExecuteAsync(steps);

			var output = MessageCollectorStepProcessor.CollectOutput();
			MessageBox.Show(output, "Execution result", MessageBoxButton.OK);
		}
	}
}