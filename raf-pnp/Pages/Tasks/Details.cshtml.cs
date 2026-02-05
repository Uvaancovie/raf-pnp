using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using raf_pnp.Data;
using raf_pnp.Models;
using TaskStatus = raf_pnp.Models.TaskStatus;

namespace raf_pnp.Pages.Tasks
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public RafTask Task { get; set; } = null!;
        public List<TaskComment> Comments { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            Task = await _context.Tasks
                .Include(t => t.AssignedToUser)
                .Include(t => t.CreatedByUser)
                .Include(t => t.Team)
                .Include(t => t.Case)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Task == null)
                return NotFound();

            Comments = await _context.TaskComments
                .Include(c => c.User)
                .Where(c => c.TaskId == id)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();

            return Page();
        }
    }
}
