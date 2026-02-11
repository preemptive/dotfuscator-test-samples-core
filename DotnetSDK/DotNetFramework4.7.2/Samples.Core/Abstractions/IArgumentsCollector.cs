using System.Collections.Generic;

namespace PreEmptive.Dotfuscator.Samples.Core.Abstractions
{
    internal interface IArgumentsCollector
    {
        void PushArgument(string typeName, string name, object value);

        Dictionary<string, object> GetArguments(string typeName);

        bool HasArguments();
    }
}
