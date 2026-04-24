namespace ShelfLifeSink;

internal class MyService
{
    private bool _warningSinkField;
    private bool _expiredSinkField;
    private bool WarningSinkProperty { get; set; }
    private bool ExpiredSinkProperty { get; set; }
    private Action<bool> WarningSinkDelegate = (warning) => { Console.WriteLine($"MyService.WarningSinkDelegate called with warning = {warning}"); };
    private Action<bool> ExpiredSinkDelegate = (expired) => { Console.WriteLine($"MyService.ExpiredSinkDelegate called with expired = {expired}"); };

    private void WarningSinkMethod(bool warning)
    {
        Console.WriteLine($"MyService.WarningSinkMethod called with warning = {warning}");
    }
    private void ExpiredSinkMethod(bool expired)
    {
        Console.WriteLine($"MyService.ExpiredSinkMethod called with expired = {expired}");
    }

    private void StringSinkMethod(string warningDate, string expiredDate)
    {
        Console.WriteLine($"MyService.StringSinkMethod called with warningDate = {warningDate}, expiredDate = {expiredDate}");
    }

    internal void ExecWithMethodSink()
    {
        Console.WriteLine("MyService.ExecWithMethodSink: Doing something...");
        Console.WriteLine();
    }

    internal void ExecWithStringMethodSink()
    {
        Console.WriteLine("MyService.ExecWithStringMethodSink: Doing something...");
        Console.WriteLine();
    }

    internal void ExecWithFieldSink()
    {
        Console.WriteLine($"_warningSinkField = {_warningSinkField}");
        Console.WriteLine($"_expiredSinkField = {_expiredSinkField}");
        Console.WriteLine("MyService.ExecWithFieldSink: Doing something...");
        Console.WriteLine();
    }

    internal void ExecWithPropertySink()
    {
        Console.WriteLine($"WarningSinkProperty = {WarningSinkProperty}");
        Console.WriteLine($"ExpiredSinkProperty = {ExpiredSinkProperty}");
        Console.WriteLine("MyService.ExecWithPropertySink: Doing something...");
        Console.WriteLine();
    }

    internal void ExecWithDelegateSink()
    {
        Console.WriteLine("MyService.ExecWithDelegateSink: Doing something...");
        Console.WriteLine();
    }

    internal void ExecWithMethodArgumentSink(Action<bool> warning, Action<bool> expired)
    {
        Console.WriteLine("MyService.ExecWithMethodArgumentSink: Doing something...");
        Console.WriteLine();
    }

    internal void ExecWithStringMethodArgumentSink(Action<string, string> sink)
    {
        Console.WriteLine("MyService.ExecWithStringMethodArgumentSink: Doing something...");
        Console.WriteLine();
    }
}
