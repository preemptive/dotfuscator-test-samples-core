using Microsoft.Extensions.DependencyInjection;
using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Extensions;
using PreEmptive.Dotfuscator.Samples.Core.Lib;
using PreEmptive.Dotfuscator.Samples.Core.Services;
using CoreConstants = PreEmptive.Dotfuscator.Samples.Core.Constants;
using ConfigurationManager = PreEmptive.Dotfuscator.Samples.Core.Lib.ConfigurationManager;
using Microsoft.Extensions.Configuration;

namespace PreEmptive.Dotfuscator.Samples.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServiceManager.Services.AddStepsProcessors();

            ConfigurationManager.Builder
                .AddJsonFile($"Core/{CoreConstants.CoreAppsettings}")
                .AddJsonFile("appsettings.json");

            var workflow = new WorkflowExecutor(new ConsoleOutputStepProcessor());
            var steps = StepsContextFactory.Create(ServiceManager.ServiceProvider.GetRequiredService<IEnumerable<IStepProcessor>>());

            workflow.ExecuteAsync(steps).GetAwaiter().GetResult();
        }
    }
}
