using System.ComponentModel;

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
}