using System.ComponentModel.DataAnnotations;

namespace BucketBudget.Models;

public class Transaction
{
    public int Id { get; set; }
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }
    public string Location { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public int BucketId { get; set; }
    public Bucket Bucket { get; set; }
}