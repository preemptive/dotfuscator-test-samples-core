using System;
using System.Collections.Generic;
using System.Text;

namespace PreEmptive.Dotfuscator.Samples.Core.Services
{
    internal class RemovalStepProcessor
    {
		private int unusedVariable = 100; // Unused variable
		private static readonly string UNUSED_CONSTANT = "Obfuscation Test"; // Unused constant
		private void UnusedMethod() // Unused method
		{
			Console.WriteLine("This method is never used");
		}
		private class UnusedInnerClass // Unused inner class
		{
			void InnerMethod()
			{
				Console.WriteLine("This class may be removed");
			}
		}
	}
}
