
namespace PreEmptive.Dotfuscator.Samples.Core.Extensions
{
    public static class EnumerableExtensions
    {
        // C# 14 extension members feature demo
        // 1️ Instance-style extension block
        extension<T>(IEnumerable<T> source)
        {
            public bool IsEmpty => !source.Any();

            //public T this[int index] => source.Skip(index).First();

            public IEnumerable<T> WhereEven(Func<T, bool> predicate)
            {
                //Console.WriteLine("WhereEven extension method called.");
                return source.Where(predicate);
            }
        }

        // 2️ Type-only extension block
        extension<T>(IEnumerable<T>)
        {
            public static IEnumerable<T> Combine(IEnumerable<T> first, IEnumerable<T> second)
            {
                //Console.WriteLine("Combine static extension method called.");
                return first.Concat(second);
            }

            public static IEnumerable<T> Empty => Enumerable.Empty<T>();
        }
    }


}
