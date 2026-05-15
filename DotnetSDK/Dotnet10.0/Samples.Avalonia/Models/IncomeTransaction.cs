namespace PreEmptive.Dotfuscator.Samples.AvaloniaApp.Models;

public class IncomeTransaction : TransactionBase
{
    public override TransactionType Type => TransactionType.Income;

    public override string DisplaySummary() =>
        $"[Income] {Date:yyyy-MM-dd} | +{Amount:C} | {Note}";
}