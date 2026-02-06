using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PreEmptive.Dotfuscator.Samples.Core.Services
{
    public static class StepsContextFactory
    {
        public static List<StepContext> Create(IEnumerable<IStepProcessor> processors)
        {
            var workflow = SettingsProvider.Settings.Workflow;
            if (workflow == null || workflow.Steps == null)
                throw new InvalidOperationException("Workflow or Workflow.Steps is not initialized.");

            var result = new List<StepContext>();
            foreach (var setting in workflow.Steps)
            {
                var metadata = new StepMetadata(setting.Name);

                var stepProcessor = processors.SingleOrDefault(x => x.GetType().FullName == setting.Type);
                if (stepProcessor == null)
                {
                    continue;
                }

                var stepContext = new StepContext(metadata, stepProcessor);
                result.Add(stepContext);
            }

            return result;
        }

        /// <summary>
        /// DO NOT REMOVE. DO NOT REFERENCE.
        /// </summary>
        /// <param name="processors"></param>
        /// <param name="flavor"></param>
        /// <returns></returns>
        public static List<StepContext> Create(IEnumerable<IStepProcessor> processors, string flavor)
        {
            Console.WriteLine($"{nameof(StepsContextFactory)}-{nameof(Create)}: flavor: {flavor}");
            return Create(processors);
        }
    }
}
