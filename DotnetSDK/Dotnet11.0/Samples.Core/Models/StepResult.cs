namespace PreEmptive.Dotfuscator.Samples.Core.Models
{
    public sealed record StepResult
    {
        public bool IsSuccess { get; }

        public string? Message { get; }

        public object? Value { get; }

        private StepResult(bool isSuccess, object? value, string? message)
        {
            IsSuccess = isSuccess;
            Value = value;
            Message = message;
        }

        public static StepResult Success(object? value = null, string? message = null)
            => new(true, value, message);

        public static StepResult Failure(string message)
            => new(false, default, message);
    }
}
