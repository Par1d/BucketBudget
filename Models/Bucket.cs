using System.ComponentModel;
using BucketBudget.Exceptions;

namespace BucketBudget.Models;

public class Bucket
{
    public int Id { get; set; }
    public string? Name { get; set; }
    [DisplayName("Drop Amount")]
    public decimal DropAmount { get; set; }
    [DisplayName("Max Balance")]
    public decimal MaxBalance { get; set; }
    public decimal Balance { get; set; }
    public int Ordinal { get; set; }

    public IEnumerable<Transaction> Transactions { get; set; }

    public void Deposit(decimal deposit)
    {
        if (deposit <= 0)
            throw new ArgumentException("Value must be positive", nameof(deposit));

        if (deposit > MaxBalance - Balance)
            throw new OverDepositException(this, deposit);

        Balance += deposit;
    }

    public void Withdraw(decimal withdrawal)
    {
        if (withdrawal <= 0)
            throw new ArgumentException("Value must be positive", nameof(withdrawal));

        if (withdrawal > Balance)
            throw new InsufficientBalanceException(this, withdrawal);

        Balance -= withdrawal;
    }
}