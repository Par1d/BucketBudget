using System.ComponentModel;
using BucketBudget.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BucketBudget.Pages;

public class TransferModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public SelectList Options { get; set; }
    [BindProperty]
    [DisplayName("From Bucket")]
    public int FromId { get; set; }
    [BindProperty]
    [DisplayName("To Bucket")]

    public int ToId { get; set; }
    [BindProperty]
    public decimal Amount { get; set; }

    public TransferModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task OnGet()
    {
        var buckets = await _context.Buckets.ToArrayAsync();
        Options = new SelectList(buckets, nameof(Bucket.Id), nameof(Bucket.Name));
    }

    public async Task<IActionResult> OnPost()
    {
        var from = await _context.Buckets.FindAsync(FromId);
        var to = await _context.Buckets.FindAsync(ToId);

        from.Balance -= Amount;
        to.Balance += Amount;

        await _context.Transactions.AddRangeAsync(new[] {
            new Transaction {
                BucketId = to.Id,
                Location = "Buckets",
                Date = DateTime.Today,
                Description = $"Transfer from {from.Name}",
                Amount = Amount
            },
            new Transaction {
                BucketId = from.Id,
                Location = "Buckets",
                Date = DateTime.Today,
                Description = $"Transfer to {to.Name}",
                Amount = -Amount
            }
        });

        await _context.SaveChangesAsync();

        return RedirectToPage("/Index");
    }
}

