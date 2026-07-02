namespace PreEmptive.Dotfuscator.Samples.Avalonia.Lib.Extensions;

public static class DecimalExtensions
{
    public static string ToCurrencyString(this decimal value) =>
        value.ToString("C2");

    public static string ToSignedCurrencyString(this decimal value) =>
        value >= 0 ? $"+{value:C2}" : value.ToString("C2");

    public static bool IsPositive(this decimal value) => value > 0;
    public static bool IsNegative(this decimal value) => value < 0;
}