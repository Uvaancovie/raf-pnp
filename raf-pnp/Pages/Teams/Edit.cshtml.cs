using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using raf_pnp.Data;
using raf_pnp.Models;

namespace raf_pnp.Pages.Teams
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Team Team { get; set; } = null!;

        public SelectList UserSelectList { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            Team = await _context.Teams
                .FirstOrDefaultAsync(t => t.Id == id);

            if (Team == null)
                return NotFound();

            await LoadUsers();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadUsers();
                return Page();
            }

            _context.Attach(Team).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TeamExists(Team.Id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToPage("./Details", new { id = Team.Id });
        }

        private async Task LoadUsers()
        {
            var users = await _context.Users
                .Where(u => u.IsActive)
                .OrderBy(u => u.FullName)
                .ToListAsync();

            UserSelectList = new SelectList(users, "Id", "FullName", Team.LeadUserId);
        }

        private async Task<bool> TeamExists(int id)
        {
            return await _context.Teams.AnyAsync(t => t.Id == id);
        }
    }
}
