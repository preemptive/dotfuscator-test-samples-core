using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Models;
using PreEmptive.Dotfuscator.Samples.AvaloniaApp.Services.Interfaces;

namespace PreEmptive.Dotfuscator.Samples.AvaloniaApp.ViewModels;

public partial class TransactionListViewModel : ObservableObject
{
    private readonly ITransactionRepository _repository;
    private readonly ITransactionFactory _factory;

    public ObservableCollection<TransactionBase> Transactions => _repository.Transactions;

    [ObservableProperty]
    private TransactionViewModel newTransaction = new();

    [ObservableProperty]
    private string? validationError;

    public IEnumerable<Category> Categories =>
        Enum.GetValues(typeof(Category)).Cast<Category>();

    public TransactionListViewModel(ITransactionRepository repository, ITransactionFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    [RelayCommand]
    private void AddTransaction()
    {
        try
        {
            var date = NewTransaction.Date?.DateTime ?? DateTime.Now;
            var amount = NewTransaction.Amount ?? 0m;
            var note = NewTransaction.Note;

            TransactionBase transaction = NewTransaction.IsExpense
                ? _factory.CreateExpense(date, amount, NewTransaction.Category ?? Category.Other, note)
                : _factory.CreateIncome(date, amount, note);

            _repository.Add(transaction);
            ValidationError = null;
            NewTransaction = new TransactionViewModel();
        }
        catch (InvalidOperationException ex)
        {
            ValidationError = ex.Message;
        }
    }

    [RelayCommand]
    private void RemoveTransaction(TransactionBase? transaction)
    {
        if (transaction != null)
            _repository.Remove(transaction);
    }
}