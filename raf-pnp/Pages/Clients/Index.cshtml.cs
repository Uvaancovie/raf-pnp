using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using raf_pnp.Data;
using raf_pnp.Models;

namespace raf_pnp.Pages.Clients
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Client> Clients { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.Clients
                .Include(c => c.Cases)
                .AsQueryable();

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                query = query.Where(c =>
                    c.FirstName.Contains(SearchTerm) ||
                    c.LastName.Contains(SearchTerm) ||
                    c.IdNumber.Contains(SearchTerm) ||
                    (c.Email != null && c.Email.Contains(SearchTerm)) ||
                    (c.PhoneNumber != null && c.PhoneNumber.Contains(SearchTerm)));
            }

            Clients = await query
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .ToListAsync();
        }
    }
}
