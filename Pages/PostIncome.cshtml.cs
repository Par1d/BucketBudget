using System.ComponentModel.DataAnnotations;
using BucketBudget.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BucketBudget.Pages;
public class PostIncomeModel : PageModel
{
    private readonly ApplicationDbContext _context;

    [BindProperty]
    public decimal Amount { get; set; }
    [BindProperty]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; } = DateTime.Today;

    public PostIncomeModel(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> OnPost()
    {
        var remaining = Amount;

        foreach (var bucket in _context.Buckets.OrderBy(b => b.Ordinal))
        {
            var drop = Math.Min(bucket.MaxBalance - bucket.Balance, bucket.DropAmount);
            drop = Math.Min(drop, remaining);

            if (drop > 0)
            {
                await _context.Transactions.AddAsync(new Transaction
                {
                    BucketId = bucket.Id,
                    Location = "Buckets",
                    Description = "Posted Income",
                    Date = Date.Date,
                    Amount = drop
                });

                bucket.Deposit(drop);
                remaining -= drop;
            }
        }

        await _context.SaveChangesAsync();

        if (remaining > 0)
        {
            ModelState.Clear();
            Amount = remaining;
            return Page();
        }

        return RedirectToPage("Index");
    }
}
