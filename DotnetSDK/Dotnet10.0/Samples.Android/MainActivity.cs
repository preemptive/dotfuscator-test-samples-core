using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PreEmptive.Dotfuscator.Samples.Core;
using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Extensions;
using PreEmptive.Dotfuscator.Samples.Core.Lib;
using PreEmptive.Dotfuscator.Samples.Core.Services;
using ConfigurationManager = PreEmptive.Dotfuscator.Samples.Core.Lib.ConfigurationManager;
using PreEmptive.Dotfuscator.Samples.Core.Services.StepProcessors;

namespace PreEmptive.Dotfuscator.Samples.Android
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        Button? MyBtn;

        private readonly IWorkflowExecutor? _workflowExecutor;
        public MainActivity()
        {
            _workflowExecutor = new WorkflowExecutor(new MessageCollectorStepProcessor());
        }
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ServiceManager.Services.AddStepsProcessors();

            SetContentView(Resource.Layout.activity_main);

            var stream = Resources.OpenRawResource(Resource.Raw.appsettings_core);
            ConfigurationManager.Builder.AddJsonStream(stream);

            MyBtn = FindViewById<Button>(Resource.Id.myButton);

            if (MyBtn != null)
                MyBtn.Click += Btn_Click;

        }

        private async void Btn_Click(object? sender, System.EventArgs e)
        {
            var steps = StepsContextFactory.Create(ServiceManager.ServiceProvider.GetRequiredService<IEnumerable<IStepProcessor>>());

            ArgumentsCollector.Instance.PushDefaultArguments();
            ArgumentsCollector.Instance.PushArgument("WriteToTextFile", nameof(WriteToFileStepProcessor.OutputPath), this.FilesDir.AbsolutePath);

            await _workflowExecutor.ExecuteAsync(steps);

            var output = MessageCollectorStepProcessor.CollectOutput();
            new AlertDialog.Builder(this).SetTitle("Execution result").SetMessage(output).SetPositiveButton("OK", (senderAlert, args) => { }).Show();
        }
    }
}