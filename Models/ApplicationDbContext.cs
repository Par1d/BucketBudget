using Microsoft.EntityFrameworkCore;

namespace BucketBudget.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Bucket> Buckets { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
}