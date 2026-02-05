using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using raf_pnp.Data;
using raf_pnp.Models;

namespace raf_pnp.Pages.Clients
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Client Client { get; set; } = new();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Client.DateCreated = DateTime.Now;
            _context.Clients.Add(Client);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Details", new { id = Client.Id });
        }
    }
}
