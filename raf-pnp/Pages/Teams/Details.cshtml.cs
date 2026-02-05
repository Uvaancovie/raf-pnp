using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using raf_pnp.Data;
using raf_pnp.Models;
using TaskStatus = raf_pnp.Models.TaskStatus;

namespace raf_pnp.Pages.Teams
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Team Team { get; set; } = null!;
        public List<TeamMember> TeamMembers { get; set; } = new();
        public List<RafCase> AssignedCases { get; set; } = new();
        public List<RafTask> TeamTasks { get; set; } = new();
        public List<User> AvailableUsers { get; set; } = new();
        public TeamStatistics Statistics { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            Team = await _context.Teams
                .Include(t => t.LeadUser)
                .Include(t => t.TeamMembers.Where(tm => tm.IsActive))
                    .ThenInclude(tm => tm.User)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (Team == null)
                return NotFound();

            TeamMembers = await _context.TeamMembers
                .Include(tm => tm.User)
                .Where(tm => tm.TeamId == id && tm.IsActive)
                .OrderByDescending(tm => tm.Role)
                .ThenBy(tm => tm.User.FullName)
                .ToListAsync();

            AssignedCases = await _context.Cases
                .Include(c => c.Client)
                .Where(c => c.AssignedTeamId == id)
                .OrderByDescending(c => c.DateOpened)
                .Take(10)
                .ToListAsync();

            TeamTasks = await _context.Tasks
                .Include(t => t.AssignedToUser)
                .Include(t => t.Case)
                .Where(t => t.TeamId == id)
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.DueDate)
                .Take(20)
                .ToListAsync();

            // Get users not already in the team
            var teamMemberIds = TeamMembers.Select(tm => tm.UserId).ToList();
            AvailableUsers = await _context.Users
                .Where(u => u.IsActive && !teamMemberIds.Contains(u.Id))
                .OrderBy(u => u.FullName)
                .ToListAsync();

            // Calculate statistics
            var totalTasks = await _context.Tasks.Where(t => t.TeamId == id).CountAsync();
            var activeTasks = await _context.Tasks
                .Where(t => t.TeamId == id && t.Status != TaskStatus.Completed && t.Status != TaskStatus.Cancelled)
                .CountAsync();
            var completedTasks = await _context.Tasks
                .Where(t => t.TeamId == id && t.Status == TaskStatus.Completed)
                .CountAsync();
            var overdueTasks = await _context.Tasks
                .Where(t => t.TeamId == id && t.DueDate.HasValue && t.DueDate.Value < DateTime.Now && 
                           t.Status != TaskStatus.Completed && t.Status != TaskStatus.Cancelled)
                .CountAsync();

            Statistics = new TeamStatistics
            {
                ActiveCases = AssignedCases.Count,
                TotalTasks = totalTasks,
                ActiveTasks = activeTasks,
                CompletedTasks = completedTasks,
                OverdueTasks = overdueTasks,
                CompletionRate = totalTasks > 0 ? (double)completedTasks / totalTasks * 100 : 0
            };

            return Page();
        }
    }
}
