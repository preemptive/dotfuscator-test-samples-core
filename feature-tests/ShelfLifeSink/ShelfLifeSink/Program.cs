using ShelfLifeSink;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Press any key to start...");
        Console.ReadKey();
        Console.WriteLine("Executing...");
        Console.WriteLine("------------------------------");

        var svc = new MyService();
        svc.ExecWithMethodSink();
        svc.ExecWithStringMethodSink();
        svc.ExecWithFieldSink();
        svc.ExecWithPropertySink();
        svc.ExecWithDelegateSink();
        svc.ExecWithMethodArgumentSink(
            (warning) => Console.WriteLine($"Method argument sink - Warning: {warning}"),
            (expired) => Console.WriteLine($"Method argument sink - Expired: {expired}"));
        svc.ExecWithStringMethodArgumentSink(
            (warningDate, expiredDate) => Console.WriteLine($"String method argument sink - Warning Date: {warningDate}, Expired Date: {expiredDate}"));
    }
}