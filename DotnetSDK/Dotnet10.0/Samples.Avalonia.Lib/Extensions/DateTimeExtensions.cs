using System;

namespace PreEmptive.Dotfuscator.Samples.Avalonia.Lib.Extensions;

public static class DateTimeExtensions
{
    public static bool IsCurrentMonth(this DateTime date) =>
        date.Month == DateTime.Now.Month && date.Year == DateTime.Now.Year;

    public static bool IsCurrentYear(this DateTime date) =>
        date.Year == DateTime.Now.Year;

    public static string ToShortDisplay(this DateTime date) =>
        date.ToString("yyyy-MM-dd");

    public static string ToMonthYearDisplay(this DateTime date) =>
        date.ToString("MMMM yyyy");
}