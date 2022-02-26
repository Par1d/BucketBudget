using BucketBudget.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BucketBudget.Pages;

public class BucketModel : PageModel
{
    private readonly ApplicationDbContext _context;

    [BindProperty(SupportsGet = true)]
    public string Id { get; set; }
    [BindProperty]
    public Bucket Bucket { get; set; }

    public BucketModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnGet()
    {
        if (Id == "new")
        {
            Bucket = new Bucket();
        }
        else if (int.TryParse(Id, out int bucketId))
        {
            Bucket = await _context.Buckets.FindAsync(bucketId);
        }

        if (Bucket is null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (Bucket is null)
        {
            return BadRequest();
        }

        if (Bucket.Id == 0)
        {
            var nextOrdinal = await _context.Buckets.MaxAsync(b => (int?)b.Ordinal);
            Bucket.Ordinal = nextOrdinal is null ? 0 : (int)nextOrdinal + 1;

            await _context.Buckets.AddAsync(Bucket);
        }
        else
        {
            var existing = await _context.Buckets.FindAsync(Bucket.Id);

            if (existing is null)
                return BadRequest();

            existing.Name = Bucket.Name;
            existing.DropAmount = Bucket.DropAmount;
            existing.MaxBalance = Bucket.MaxBalance;
        }

        await _context.SaveChangesAsync();

        return RedirectToPage("Index");
    }
}
