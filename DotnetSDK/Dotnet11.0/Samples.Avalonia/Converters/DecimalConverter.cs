using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace PreEmptive.Dotfuscator.Samples.AvaloniaApp.Converters;

public class DecimalConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is decimal d ? d.ToString(culture) : string.Empty;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return null; // Empty string → null (safe for decimal?)

            if (decimal.TryParse(str, NumberStyles.Any, culture, out var result))
                return result;
        }
        return null;
    }
}