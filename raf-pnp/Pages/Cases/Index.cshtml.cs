using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using raf_pnp.Data;
using raf_pnp.Models;

namespace raf_pnp.Pages.Cases
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<RafCase> Cases { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public CaseStatus? StatusFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; } = "newest";

        public async Task OnGetAsync()
        {
            var query = _context.Cases
                .Include(c => c.Client)
                .AsQueryable();

            // Apply search filter
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                query = query.Where(c =>
                    c.CaseNumber.Contains(SearchTerm) ||
                    (c.Client != null && (c.Client.FirstName.Contains(SearchTerm) || c.Client.LastName.Contains(SearchTerm))) ||
                    (c.AssignedAttorney != null && c.AssignedAttorney.Contains(SearchTerm)));
            }

            // Apply status filter
            if (StatusFilter.HasValue)
            {
                query = query.Where(c => c.Status == StatusFilter.Value);
            }

            // Apply sorting
            query = SortBy switch
            {
                "oldest" => query.OrderBy(c => c.DateOpened),
                "casenumber" => query.OrderBy(c => c.CaseNumber),
                "client" => query.OrderBy(c => c.Client!.LastName).ThenBy(c => c.Client!.FirstName),
                _ => query.OrderByDescending(c => c.DateOpened)
            };

            Cases = await query.ToListAsync();
        }
    }
}
