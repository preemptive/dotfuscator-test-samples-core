namespace PreEmptive.Dotfuscator.Samples.AvaloniaApp.Models;

public class ExpenseTransaction : TransactionBase
{
    public Category? Category { get; set; }

    public override TransactionType Type => TransactionType.Expense;

    public override string DisplaySummary() =>
        $"[Expense] {Date:yyyy-MM-dd} | -{Amount:C} | {Category} | {Note}";
}