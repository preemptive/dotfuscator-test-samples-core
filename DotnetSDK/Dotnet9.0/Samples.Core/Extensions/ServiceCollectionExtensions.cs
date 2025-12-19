using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using PreEmptive.Dotfuscator.Samples.Core.Abstractions;

namespace PreEmptive.Dotfuscator.Samples.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStepsProcessors(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var processorTypes = assembly.GetTypes()
                .Where(x => !x.IsAbstract && x.GetType() != typeof(IStepProcessor))
                .Where(x => typeof(IStepProcessor).IsAssignableFrom(x));
            foreach (var processorType in processorTypes)
            {
                services.AddTransient(typeof(IStepProcessor), processorType);
            }

            return services;
        }
    }
}
