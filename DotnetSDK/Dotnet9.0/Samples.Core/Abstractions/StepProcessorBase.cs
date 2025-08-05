using PreEmptive.Dotfuscator.Samples.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PreEmptive.Dotfuscator.Samples.Core.Abstractions
{
    public abstract class StepProcessorBase : IStepProcessor
    {
        public Task<StepResult> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            if (!ValidateArguments(out var message))
            {
                return Task.FromResult(StepResult.Failure(message!));
            }

            return ExecuteInternalAsync(cancellationToken);
        }

        protected abstract Task<StepResult> ExecuteInternalAsync(CancellationToken cancellationToken = default);

        private bool ValidateArguments(out string? message)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(this, serviceProvider: null, items: null);
            Validator.TryValidateObject(this, validationContext, validationResults, validateAllProperties: true);

            if (validationResults.Any())
            {
                message = validationResults.Select(x => x.ErrorMessage).Aggregate((x, y) => $"{x}\n{y}");
                return false;
            }

            message = null;
            return true;
        }
    }
}