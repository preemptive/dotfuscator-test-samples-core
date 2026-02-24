namespace PreEmptive.Dotfuscator.Samples.Core.Models
{
    public sealed class StepResult
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
            => new StepResult(true, value, message);
    }
}
