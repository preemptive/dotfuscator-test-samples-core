using System;
using CommunityToolkit.Mvvm.ComponentModel;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Models;

namespace PreEmptive.Dotfuscator.Samples.AvaloniaApp.ViewModels;

public partial class TransactionViewModel : ObservableObject
{
    [ObservableProperty]
    private DateTimeOffset? date = DateTimeOffset.Now;

    [ObservableProperty]
    private decimal? amount;

    [ObservableProperty]
    private Category? category;

    [ObservableProperty]
    private string? note;

    public bool IsIncome
    {
        get => Type == TransactionType.Income;
        set
        {
            Type = value ? TransactionType.Income : TransactionType.Expense;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsExpense));
        }
    }

    public bool IsExpense
    {
        get => Type == TransactionType.Expense;
        set
        {
            Type = value ? TransactionType.Expense : TransactionType.Income;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsIncome));
        }
    }

    public TransactionType Type { get; private set; } = TransactionType.Expense;
}