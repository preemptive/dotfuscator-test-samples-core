using PreEmptive.Dotfuscator.Samples.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PreEmptive.Dotfuscator.Samples.Core.Abstractions
{
    public abstract class StepProcessorBase : IStepProcessor
    {
        public Task<StepResult> ExecuteAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            string message;
            if (!ValidateArguments(out message))
            {
                // message is guaranteed non-null if validation fails
                return Task.FromResult(StepResult.Failure(message));
            }

            return ExecuteInternalAsync(cancellationToken);
        }

        protected abstract Task<StepResult> ExecuteInternalAsync(CancellationToken cancellationToken = default(CancellationToken));

        private bool ValidateArguments(out string message)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(this, serviceProvider: null, items: null);
            Validator.TryValidateObject(this, validationContext, validationResults, validateAllProperties: true);

            if (validationResults.Any())
            {
                message = validationResults.Select(x => x.ErrorMessage).Aggregate((x, y) => $"{x}\n{y}");
                return false;
            }

            message = string.Empty;
            return true;
        }
    }
}