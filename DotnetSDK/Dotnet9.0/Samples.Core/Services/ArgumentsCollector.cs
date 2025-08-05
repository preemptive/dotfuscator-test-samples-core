using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using System.Collections.Concurrent;

namespace PreEmptive.Dotfuscator.Samples.Core.Services
{
    internal sealed class ArgumentsCollector : IArgumentsCollector
    {
        private static readonly Lazy<ArgumentsCollector> _instance =
            new(() => new ArgumentsCollector());

        public static ArgumentsCollector Instance => _instance.Value;

        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, object>> _argumentsPool;

        private ArgumentsCollector()
        {
            _argumentsPool = new ConcurrentDictionary<string, ConcurrentDictionary<string, object>>();
        }

        public Dictionary<string, object> GetArguments(string? stepName)
        {
            if (string.IsNullOrEmpty(stepName)) return [];

            if (_argumentsPool.TryGetValue(stepName, out var arguments))
            {
                return new Dictionary<string, object>(arguments);
            }

            return [];
        }


        public void PushArgument(string typeName, string name, object value)
        {
            var stepArguments = _argumentsPool.GetOrAdd(typeName, _ => new ConcurrentDictionary<string, object>());
            stepArguments.AddOrUpdate(name, value, (key, oldValue) => value);
        }

        public bool HasArguments()
            => !_argumentsPool.IsEmpty;
    }
}
