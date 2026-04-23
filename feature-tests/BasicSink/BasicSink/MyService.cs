using System;
using System.Collections.Generic;
using System.Text;

namespace BasicSink;

internal class MyService
{
    private bool _sinkField;
    private bool SinkProperty { get; set; }
    private void SinkMethod(bool value)
    {
        Console.WriteLine($"MyService.SinkMethod: Value: {value}");
    }

    internal void ExecWithMethodSink()
    {
        Console.WriteLine("MyService.ExecWithMethodSink: Doing something...");
        Console.WriteLine();
    }

    internal void ExecWithFieldSink()
    {
        Console.WriteLine($"_sinkField: {_sinkField}");
        Console.WriteLine("MyService.ExecWithFieldSink: Doing something...");
        Console.WriteLine();
    }

    internal void ExecWithPropertySink()
    {
        Console.WriteLine($"SinkProperty: {SinkProperty}");
        Console.WriteLine("MyService.ExecWithPropertySink: Doing something...");
        Console.WriteLine();
    }

    internal void ExecWithMethodArgumentSink(Action<bool> sink)
    {
        Console.WriteLine("MyService.ExecWithMethodArgumentSink: Doing something...");
        Console.WriteLine();
    }
}