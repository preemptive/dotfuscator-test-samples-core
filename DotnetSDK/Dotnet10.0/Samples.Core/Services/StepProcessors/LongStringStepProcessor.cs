using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PreEmptive.Dotfuscator.Samples.Core.Services.StepProcessors
{
    public class LongStringStepProcessor : StepProcessorBase
    {
        public static readonly string LongStringTest =
           @"This is a very long string for encryption testing.
           Lorem ipsum dolor sit amet, consectetur adipiscing elit.
           Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
           Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.
           Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
           Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

        protected override async Task<StepResult> ExecuteInternalAsync(CancellationToken cancellationToken = default)
        {
            return StepResult.Success(message: $"\n Long String : \n{LongStringTest}\n");
        }
    }
}
