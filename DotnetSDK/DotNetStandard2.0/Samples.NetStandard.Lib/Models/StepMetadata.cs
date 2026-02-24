using System;

namespace PreEmptive.Dotfuscator.Samples.Core.Models
{
    public readonly struct StepMetadata
    {
        public string Name { get; }

        public StepMetadata(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException(nameof(name));
            Name = name;
        }
    }
}
