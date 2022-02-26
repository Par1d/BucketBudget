using BucketBudget.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BucketBudget.Pages;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IEnumerable<Bucket> Buckets { get; private set; } = Enumerable.Empty<Bucket>();

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task OnGet()
    {
        Buckets = await _context.Buckets
            .OrderBy(b => b.Ordinal)
            .ToArrayAsync();
    }

    public async Task OnPostMoveUp(int id)
    {
        var bucket = await _context.Buckets.FindAsync(id);
        var above = await _context.Buckets.FirstOrDefaultAsync(b => b.Ordinal == bucket.Ordinal - 1);

        bucket.Ordinal--;
        above.Ordinal++;

        await _context.SaveChangesAsync();
        await OnGet();
    }

    public async Task OnPostMoveDown(int id)
    {
        var bucket = await _context.Buckets.FindAsync(id);
        var below = await _context.Buckets.FirstOrDefaultAsync(b => b.Ordinal == bucket.Ordinal + 1);

        bucket.Ordinal++;
        below.Ordinal--;

        await _context.SaveChangesAsync();
        await OnGet();
    }
}
