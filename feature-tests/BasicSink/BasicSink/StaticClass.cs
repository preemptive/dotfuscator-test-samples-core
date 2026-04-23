namespace BasicSink;

internal static class StaticClass
{
    delegate void CustomSinkDelegate(bool value);

    private static bool _fieldSink;
    private static bool SinkProperty { get; set; }
    private static void SinkMethod(bool value)
    {
        Console.WriteLine($"StaticClass.SinkMethod. Value: {value}");
    }
    private static CustomSinkDelegate _customSinkDelegate = (value) => { Console.WriteLine($"StaticClass._customSinkDelegate. Value: {value}"); };
    private static Action<bool> _actionSink = (value) => { Console.WriteLine($"StaticClass._actionSink. Value: {value}"); };

    internal static void ExecWithMethodSink()
    {
        Console.WriteLine("StaticClass.ExecWithMethodSink: Doing something...");
        Console.WriteLine();
    }
    internal static void ExecWithFieldSink()
    {
        Console.WriteLine($"_fieldSink: {_fieldSink}");
        Console.WriteLine("StaticClass.ExecWithFieldSink: Doing something...");
        Console.WriteLine();
    }

    internal static void ExecWithPropertySink()
    {
        Console.WriteLine($"SinkProperty: {SinkProperty}");
        Console.WriteLine("StaticClass.ExecWithPropertySink: Doing something...");
        Console.WriteLine();
    }

    internal static void ExecWithMethodArgumentSink(Action<bool> sink)
    {
        Console.WriteLine("StaticClass.ExecWithMethodArgumentSink: Doing something...");
        Console.WriteLine();
    }

    internal static void ExecWithDelegateSink()
    {
        Console.WriteLine("StaticClass.ExecWithDelegateSink: Doing something...");
        Console.WriteLine();
    }

    internal static void ExecWithCustomDelegateSink()
    {
        Console.WriteLine("StaticClass.ExecWithCustomDelegateSink: Doing something...");
        Console.WriteLine();
    }
}
