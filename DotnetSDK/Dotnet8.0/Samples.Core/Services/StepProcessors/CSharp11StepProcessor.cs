#define MyType
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using PreEmptive.Dotfuscator.Samples.Core.Abstractions;
using PreEmptive.Dotfuscator.Samples.Core.Models;
using static System.Net.Mime.MediaTypeNames;

namespace PreEmptive.Dotfuscator.Samples.Core.Services.StepProcessors
{
	file class FileLocalTypesClass { }
	internal class CSharp11StepProcessor : StepProcessorBase
	{
		static StringBuilder staticMessage = new StringBuilder();
		public string? result = "\u001b[32mC# 11: Success\u001b[0m";
		protected override async Task<StepResult> ExecuteInternalAsync(CancellationToken cancellationToken = default)
		{
			var message = new StringBuilder();
			var ga = new GenericAttributes();
			ga.GenericAttributesMethod();
			GenericMathSupportMethod();
			NumericIntPtrUIntPtrMethod();
			NewlineStringInterpolations();
			ListPatternsMethod();
			MethodGroupConversionMethod();
			StringLiteralsMethod();
			AutoDefaultStructMethod();
			SpanPatternMethod();
			ExtendedNameofScopeMethod();
			Utf8StringLiteralMethod();
			RequiredMembersMethod();
			RefFieldMethod();
			FileLocalTypesMethod();
			message.Append(staticMessage.ToString() + "\n");
			message.Append(result);
			return StepResult.Success(message: $"\nResult : \n{message}\n");
		}

		//C# 11: Generic attributes
		[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
		public class AuthorAttribute<T> : Attribute
		{
			public T Name;
			public string Version;

			public AuthorAttribute(T name)
			{
				Name = name;
				Version = "1.0";
			}
		}

		[Author<String>("John Doe")]
		public class GenericAttributes
		{
			public void GenericAttributesMethod()
			{
				var type = typeof(GenericAttributes);
				//var attrs = type.GetCustomAttributes(false);
				var attrs = type.GetCustomAttributes(typeof(AuthorAttribute<string>), inherit: false);
				foreach (AuthorAttribute<String> attr in attrs)
				{
					staticMessage.Append($"---C# 11: Generic attributes - {attr.Name}, {attr.Version}\n");
				}


			}
		}

		//C# 11: Generic Math Support
		void GenericMathSupportMethod()
		{
			staticMessage.Append($"---C# 11: Generic Math Support - {Sum(new[] { new MyNumber(1), new MyNumber(2), new MyNumber(3) })}\n");

		}
		public interface IAddable<TSelf> where TSelf : IAddable<TSelf>
		{
			static abstract TSelf operator +(TSelf a, TSelf b);
		}
		public readonly struct MyNumber(int value) : IAddable<MyNumber>
		{
			public int Value { get; } = value;
			public static MyNumber operator +(MyNumber a, MyNumber b) => new(a.Value + b.Value);
			public override string ToString() => Value.ToString();
		}

		static T Sum<T>(IEnumerable<T> items) where T : IAddable<T>
		{
			T total = default!;
			foreach (var x in items) total = total + x;
			return total;
		}

		//C# 11: Numeric IntPtr and UIntPtr
		void NumericIntPtrUIntPtrMethod()
		{
			nint signedPtrSized = (nint)123;
			nuint unsignedPtrSized = (nuint)456;

			IntPtr asIntPtr = (IntPtr)signedPtrSized;
			UIntPtr asUIntPtr = (UIntPtr)unsignedPtrSized;

			staticMessage.Append($"---C# 11: Numeric IntPtr and UIntPtr - {asIntPtr},{asUIntPtr}\n");
		}

		//C# 11: Newlines in string interpolation expressions
		void NewlineStringInterpolations()
		{
			var name = "john";
			var age = 25;

			var text = $"""
					   Name : {name}
					   Age  : {age}
					   """;
			staticMessage.Append($"---C# 11: Newlines in string interpolation expressions. - {text}\n");
		}

		//C# 11: List Patterns
		void ListPatternsMethod()
		{
			int[] numbers = { 1, 2, 3, 4, 5 };
			if (numbers is [1, 2, .. var rest])
			{
				staticMessage.Append($"---C# 11: List Patterns - {string.Join(", ", rest)}\n");
			}
		}

		//C# 11: Improved method group conversion to delegate
		static int Square(int x) => x * x;

		void MethodGroupConversionMethod()
		{
			for (int i = 0; i < 3; i++)
			{
				UseFunc(Square);
			}

			staticMessage.Append("---C# 11: Improved method group conversion to delegate - passed method group multiple times\n");
		}

		static void UseFunc(Func<int, int> f)
		{
			_ = f(5);
		}

		//C# 11: String Literals
		void StringLiteralsMethod()
		{
			var json = """
			 "name": "Alice",
			 "age": 30
			""";
			staticMessage.Append($"---C# 11: String Literals - {json}\n");
		}

		//C# 11: Auto-default struct
		public readonly struct Measurement
		{
			public double Value { get; init; }

			public Measurement(double value)
			{
				Value = value;
			}
		}
		public static void AutoDefaultStructMethod()
		{
			var m1 = new Measurement(5);
			var m2 = new Measurement();
			staticMessage.Append($"---C# 11: Auto default struct - {m1.Value}, {m2.Value}\n");

		}

		//C# 11: Span Pattern
		void SpanPatternMethod()
		{
			ReadOnlySpan<char> span1 = "GET".AsSpan();
			ReadOnlySpan<char> span2 = "POST".AsSpan();
			if (span1 is "GET")
				staticMessage.Append("---C# 11: Span pattern - span1 matched \"GET\"\n");

		}

		//C# 11: Extended nameof scope
		public class MyType
		{
			public void DoWork() { }
		}

		[System.Diagnostics.Conditional(nameof(MyType))]
		void ExtendedNameofScopeMethod()
		{
			staticMessage.Append($"---C# 11: Extended nameof scope - {nameof(MyType)}\n");
		}

		//C# 11: Utf8 String Literal
		void Utf8StringLiteralMethod()
		{
			ReadOnlySpan<byte> hello = "Hello"u8;
			ReadOnlySpan<byte> rupee = "₹"u8;

			staticMessage.Append(
				$"---C# 11: UTF-8 string literals - {hello.Length}, {rupee.Length}\n"
			);
		}

		//C# 11: Required Members
		public class RequiredMembersClass
		{
			public required string Name { get; init; }
			public int Age { get; init; }
		}
		void RequiredMembersMethod()
		{
			var person = new RequiredMembersClass
			{
				Name = "Alice",
				Age = 30
			};
			staticMessage.Append($"---C# 11: Required members - {person.Name}, {person.Age}\n");

		}

		//C# 11: Ref Field
		ref struct RefBox
		{
			public ref int Value;
			public RefBox(ref int value) => Value = ref value;
		}

		static void RefFieldMethod()
		{
			int x = 1;
			var box = new RefBox(ref x);
			box.Value = 42;

			staticMessage.Append($"---C# 11: Ref fields - {x}\n");
		}

		//C# 11: File Local Types
		void FileLocalTypesMethod()
		{
			var f = new FileLocalTypesClass();
			staticMessage.Append($"---C# 11: File Local Types- success\n");
		}

	}
}
