using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using raf_pnp.Data;
using raf_pnp.Models;
using TaskStatus = raf_pnp.Models.TaskStatus;

namespace raf_pnp.Pages.Tasks
{
    public class BoardModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public BoardModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<RafTask> NotStartedTasks { get; set; } = new();
        public List<RafTask> InProgressTasks { get; set; } = new();
        public List<RafTask> BlockedTasks { get; set; } = new();
        public List<RafTask> UnderReviewTasks { get; set; } = new();
        public List<RafTask> CompletedTasks { get; set; } = new();

        public List<Team> Teams { get; set; } = new();
        public List<User> Users { get; set; } = new();

        public async Task OnGetAsync(int? teamId = null, int? assignedToUserId = null, int? caseId = null)
        {
            var query = _context.Tasks
                .Include(t => t.AssignedToUser)
                .Include(t => t.Team)
                .Include(t => t.Case)
                .AsQueryable();

            if (teamId.HasValue)
                query = query.Where(t => t.TeamId == teamId.Value);

            if (assignedToUserId.HasValue)
                query = query.Where(t => t.AssignedToUserId == assignedToUserId.Value);

            if (caseId.HasValue)
                query = query.Where(t => t.CaseId == caseId.Value);

            var allTasks = await query.OrderByDescending(t => t.Priority)
                                     .ThenBy(t => t.DueDate)
                                     .ToListAsync();

            // Group tasks by status
            NotStartedTasks = allTasks.Where(t => t.Status == TaskStatus.NotStarted).ToList();
            InProgressTasks = allTasks.Where(t => t.Status == TaskStatus.InProgress).ToList();
            BlockedTasks = allTasks.Where(t => t.Status == TaskStatus.Blocked).ToList();
            UnderReviewTasks = allTasks.Where(t => t.Status == TaskStatus.UnderReview).ToList();
            CompletedTasks = allTasks.Where(t => t.Status == TaskStatus.Completed).Take(10).ToList();

            // Load teams and users for filters
            Teams = await _context.Teams.Where(t => t.IsActive).OrderBy(t => t.Name).ToListAsync();
            Users = await _context.Users.Where(u => u.IsActive).OrderBy(u => u.FullName).ToListAsync();
        }
    }
}
