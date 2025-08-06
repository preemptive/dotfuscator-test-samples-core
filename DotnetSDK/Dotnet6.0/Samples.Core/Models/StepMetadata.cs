namespace PreEmptive.Dotfuscator.Samples.Core.Models
{
    public readonly struct StepMetadata
    {
        /// <summary>
        /// The step name.
        /// </summary>
        public string Name { get; }

        public StepMetadata(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException(nameof(name));

            Name = name;
        }
    }
}