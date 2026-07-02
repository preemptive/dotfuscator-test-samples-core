using System.Reflection;
using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Attributes;
using PreEmptive.Dotfuscator.Samples.Core.Services;

namespace PreEmptive.Dotfuscator.Samples.Core.Models
{
    public record StepContext
    {
        public StepMetadata Metadata { get; }

        private IStepProcessor Processor { get; }

        public StepContext(StepMetadata metadata, IStepProcessor processor)
        {
            Metadata = metadata;
            Processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        public Task<StepResult> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            Setup();
            return Processor.ExecuteAsync(cancellationToken);
        }

        private void Setup()
        {
            CollectArguments();
        }

        private void CollectArguments()
        {
            var args = ArgumentsCollector.Instance.GetArguments(Metadata.Name);

            foreach (var propertyInfo in Processor.GetType().GetProperties()
                         .Where(x => x.IsDefined(typeof(InputArgumentAttribute), false))
                         .Where(pi => args.ContainsKey(pi.Name)))
            {
                propertyInfo.SetValue(Processor, args[propertyInfo.Name]);
            }
        }

        /// <returns></returns>
        private bool CanPrepare()
        {
            return Assembly.GetExecutingAssembly() != null;
        }
    }
}