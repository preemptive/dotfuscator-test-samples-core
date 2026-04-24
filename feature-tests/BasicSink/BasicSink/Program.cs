using BasicSink;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Press any key to start...");
        Console.ReadKey();

        Console.WriteLine("Executing static methods...");
        Console.WriteLine("------------------------------");
        StaticClass.ExecWithMethodSink();
        StaticClass.ExecWithFieldSink();
        StaticClass.ExecWithPropertySink();
        StaticClass.ExecWithMethodArgumentSink(value => Console.WriteLine($"MethodArgumentSink: Value: {value}"));
        StaticClass.ExecWithDelegateSink();
        StaticClass.ExecWithCustomDelegateSink();
        Console.WriteLine();
        Console.WriteLine("Executing instance methods...");
        Console.WriteLine("------------------------------");
        var svc = new MyService();
        svc.ExecWithMethodSink();
        svc.ExecWithFieldSink();
        svc.ExecWithPropertySink();
        svc.ExecWithMethodArgumentSink(value => Console.WriteLine($"MethodArgumentSink: Value: {value}"));
    }
}