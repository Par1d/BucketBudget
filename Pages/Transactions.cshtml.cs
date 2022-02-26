using BucketBudget.Exceptions;
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
    public string ErrorMessage { get; set; }
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
        try
        {
            var bucket = await _context.Buckets.FindAsync(Id);
            bucket.Withdraw(Transaction.Amount);

            Transaction.Date = Transaction.Date.Date;
            Transaction.Amount = -Transaction.Amount;

            await _context.Transactions.AddAsync(Transaction);
            await _context.SaveChangesAsync();
            ModelState.Clear();
        }
        catch (InsufficientBalanceException e)
        {
            ErrorMessage = "Bucket has an insufficient balance for this withdrawal";
        }

        return await OnGet();
    }
}

