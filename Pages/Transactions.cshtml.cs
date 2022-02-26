using BucketBudget.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BucketBudget.Pages;

public class TransactionsModel : PageModel
{
    private readonly ApplicationDbContext _context;

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }
    public Bucket Bucket { get; set; }
    [BindProperty]
    public Transaction Transaction { get; set; }
    public TransactionsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnGet()
    {
        Transaction = new Transaction
        {
            Date = DateTime.Today
        };

        Bucket = await _context.Buckets
            .Include(b => b.Transactions)
            .FirstOrDefaultAsync(b => b.Id == Id);

        if (Bucket is null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        Transaction.Date = Transaction.Date.Date;
        Transaction.Amount = -Math.Abs(Transaction.Amount);

        var bucket = await _context.Buckets.FindAsync(Id);
        bucket.Balance += Transaction.Amount;

        await _context.Transactions.AddAsync(Transaction);
        await _context.SaveChangesAsync();

        ModelState.Clear();
        return await OnGet();
    }
}

