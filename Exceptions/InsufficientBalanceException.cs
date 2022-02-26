using System.Runtime.Serialization;
using BucketBudget.Models;

namespace BucketBudget.Exceptions;

[Serializable]
public class InsufficientBalanceException : Exception
{
    public InsufficientBalanceException() { }
    public InsufficientBalanceException(string message)
        : base(message) { }
    public InsufficientBalanceException(string message, Exception inner)
        : base(message, inner) { }
    protected InsufficientBalanceException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }

    public InsufficientBalanceException(Bucket bucket, decimal requestedWithdrawal)
        : base(BucketMessage(bucket, requestedWithdrawal)) { }

    private static string BucketMessage(Bucket bucket, decimal requestedWithdrawal)
    {
        return $"Bucket {bucket.Name} has an insufficient balance for requested withdrawal.\r\n" +
            $"Current balance: {bucket.Balance.ToString("c")}, requested withdrawal {requestedWithdrawal.ToString("c")}";
    }
}