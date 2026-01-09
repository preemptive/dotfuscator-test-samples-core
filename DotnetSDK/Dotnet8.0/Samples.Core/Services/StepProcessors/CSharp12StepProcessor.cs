using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Models;
using PreEmptive.Dotfuscator.Samples.Core.Resources;

namespace PreEmptive.Dotfuscator.Samples.Core.Services.StepProcessors
{
	// C# 12: Alias any type 
	using IntPair = (int X, int Y);
	internal class CSharp12StepProcessor : StepProcessorBase
	{
		static StringBuilder staticMessage = new StringBuilder();
		public string? result = "\u001b[32mC# 12: Success\u001b[0m";
		protected override async Task<StepResult> ExecuteInternalAsync(CancellationToken cancellationToken = default)
		{
			var message = new StringBuilder();
			var primaryConstructorClass = new PrimaryConstructorClass("Greeting from c#12");
			primaryConstructorClass.primaryConstructorMethod();
			CollectionExpressionMethod();
			InlineArrayMethod();
			//DefaultLambdaParameterMethod();
			RefReadonlyMethod();
#pragma warning disable TS0001
			ExperimenalAttributesMethod(); // Experimental Attributes feature will give a warning if used without pragma
#pragma warning restore TS0001
			message.Append(staticMessage.ToString() + "\n");
			message.Append(result + "\n");
			return StepResult.Success(message: $"\nResult : \n{message}\n");
		}

		// C# 12: primary constructor on a normal class
		public class PrimaryConstructorClass(String message)
		{
			public void primaryConstructorMethod()
			{
				staticMessage.Append($"---C# 12: primary constructor on a normal class - {message}\n");
			}
		}

		// C# 12: Collection expressions
		public void CollectionExpressionMethod()
		{
			int[] a = [1, 2, 3];
			int[] b = [4, 5, 6];

			int[] combined = [.. a, .. b, 7, 8];  // 1,2,3,4,5,6,7,8
			staticMessage.Append($"---C# 12: Collection Expression - {string.Join(", ", combined)}\n");
		}

		// C# 12: Inline Arrays
		public void InlineArrayMethod()
		{
			var buffer = new IntBuffer5();

			for (int i = 0; i < 5; i++)
			{
				buffer[i] = (i + 1) * 10;
			}

			staticMessage.Append("---C# 12: Inline Array - ");

			for (int i = 0; i < 5; i++)
			{
				staticMessage.Append(buffer[i]);
				if (i < 4) staticMessage.Append(", "); // 10,20,30,40,50
			}

			staticMessage.Append("\n");
		}

		[InlineArray(5)]
		public struct IntBuffer5
		{
			private int _element0;
		}

		// C# 12: Optional parameters in lambda expressions 
		// This feature is currently commented out because currently dotfuscator 7.5.0 does not fully support it yet.
		//public void DefaultLambdaParameterMethod()
		//{
		//	var add = (int x, int y = 10) => x + y;
		//	staticMessage.Append($"---C# 12: Optional parameters in lambda expressions - {add(5)}\n");
		//}

		// C# 12: ref readonly parameters 
		public void RefReadonlyMethod()
		{
			var data = 10;
			Print(ref data);
		}

		static void Print(ref readonly int d)
		{
			staticMessage.Append($"---C# 12: ref readonly parameters - {d}\n");
		}

		// C# 12: Alias any type 
		public void AliasAnyTypeMethod()
		{
			// alias at the top of the file
			IntPair p = (100, 200);
			staticMessage.Append($"---C# 12: Alias any type - {p.X}\n");
		}

		// C# 12: Experimental Attributes 
		[Experimental("TS0001")]
		public void ExperimenalAttributesMethod()
		{

			staticMessage.Append($"---C# 12: Experimental Attributes - Checked.\n");
		}
	}
}
