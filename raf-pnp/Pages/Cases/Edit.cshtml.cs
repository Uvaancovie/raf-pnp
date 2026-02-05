using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using raf_pnp.Data;
using raf_pnp.Models;

namespace raf_pnp.Pages.Cases
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RafCase RafCase { get; set; } = null!;

        public SelectList ClientList { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            RafCase = await _context.Cases.FindAsync(id) ?? new RafCase();

            if (RafCase.Id == 0)
            {
                return NotFound();
            }

            await LoadClientsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadClientsAsync();
                return Page();
            }

            _context.Attach(RafCase).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Cases.AnyAsync(c => c.Id == RafCase.Id))
                {
                    return NotFound();
                }
                throw;
            }

            return RedirectToPage("./Details", new { id = RafCase.Id });
        }

        private async Task LoadClientsAsync()
        {
            var clients = await _context.Clients
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .Select(c => new { c.Id, FullName = c.FirstName + " " + c.LastName + " (" + c.IdNumber + ")" })
                .ToListAsync();

            ClientList = new SelectList(clients, "Id", "FullName");
        }
    }
}
