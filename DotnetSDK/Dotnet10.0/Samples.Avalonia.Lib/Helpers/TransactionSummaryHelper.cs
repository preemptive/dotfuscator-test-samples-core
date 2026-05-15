using System;
using System.Collections.Generic;
using System.Linq;

namespace PreEmptive.Dotfuscator.Samples.Avalonia.Lib.Helpers;

public static class TransactionSummaryHelper
{
    public static string BuildSummaryReport(
        string strategyLabel,
        decimal totalIncome,
        decimal totalExpense,
        decimal balance)
    {
        var separator = new string('-', 40);
        return $"""
                Budget Summary — {strategyLabel}
                {separator}
                Total Income  : {totalIncome:C2}
                Total Expense : {totalExpense:C2}
                {separator}
                Balance       : {balance:C2}
                Generated     : {DateTime.Now:yyyy-MM-dd HH:mm:ss}
                """;
    }

    public static string GetBalanceStatus(decimal balance) => balance switch
    {
        > 0 => "Surplus",
        < 0 => "Deficit",
        _   => "Balanced"
    };
}