using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using raf_pnp.Data;
using raf_pnp.Models;

namespace raf_pnp.Pages.Clients
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Client? Client { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Client = await _context.Clients
                .Include(c => c.Cases)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (Client == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
