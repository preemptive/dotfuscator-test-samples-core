namespace PreEmptive.Dotfuscator.Samples.Core.Models
{
    public sealed class StepResult
    {
        public bool IsSuccess { get; }

        public string Message { get; }

        public object Value { get; }

        private StepResult(bool isSuccess, object value, string message)
        {
            IsSuccess = isSuccess;
            Value = value;
            Message = message;
        }

        public static StepResult Success(object value = null, string message = null)
        {
            return new StepResult(true, value, message);
        }

        public static StepResult Failure(string message)
        {
            return new StepResult(false, null, message);
        }
    }
}
