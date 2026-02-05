using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using raf_pnp.Data;
using raf_pnp.Models;

namespace raf_pnp.Pages.Cases
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RafCase RafCase { get; set; } = new();

        public SelectList ClientList { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadClientsAsync();
            
            // Set default MMI date (12 months from now as placeholder)
            RafCase.AccidentDate = DateTime.Today;
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadClientsAsync();
                return Page();
            }

            // Set default values
            RafCase.DateOpened = DateTime.Now;
            RafCase.Status = CaseStatus.ClientIntake;
            
            // Calculate MMI date (12 months from accident)
            RafCase.MmiDate = RafCase.AccidentDate.AddMonths(12);

            _context.Cases.Add(RafCase);
            
            // Add initial activity
            var activity = new CaseActivity
            {
                CaseId = RafCase.Id,
                ActivityType = ActivityType.StatusChanged,
                Title = "Case Created",
                Description = $"New RAF case created. Case Number: {RafCase.CaseNumber}",
                ActivityDate = DateTime.Now,
                CreatedBy = "System"
            };

            await _context.SaveChangesAsync();

            // Now add activity with the saved case ID
            activity.CaseId = RafCase.Id;
            _context.CaseActivities.Add(activity);
            await _context.SaveChangesAsync();

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
