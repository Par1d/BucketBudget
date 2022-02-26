using System.Runtime.Serialization;
using BucketBudget.Models;

namespace BucketBudget.Exceptions;

[Serializable]
public class OverDepositException : Exception
{
    public OverDepositException() { }
    public OverDepositException(string message)
        : base(message) { }
    public OverDepositException(string message, Exception inner)
        : base(message, inner) { }
    protected OverDepositException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }

    public OverDepositException(Bucket bucket, decimal requestedDeposit)
        : base(BucketMessage(bucket, requestedDeposit)) { }

    private static string BucketMessage(Bucket bucket, decimal requestedDeposit)
    {
        return $"Bucket {bucket.Name} does not have available space for the requested deposit.\r\n" +
            $"Current space available: {(bucket.MaxBalance - bucket.Balance).ToString("c")}, requested withdrawal {requestedDeposit.ToString("c")}";
    }
}