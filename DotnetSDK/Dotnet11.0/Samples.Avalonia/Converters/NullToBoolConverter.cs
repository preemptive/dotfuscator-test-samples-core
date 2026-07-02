using System;
using Avalonia.Data.Converters;

namespace PreEmptive.Dotfuscator.Samples.AvaloniaApp.Converters;

public class NullToBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        => value != null;

    public object ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        => throw new NotImplementedException();
}