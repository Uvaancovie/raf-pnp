using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using raf_pnp.Data;
using raf_pnp.Models;

namespace raf_pnp.Pages.Teams
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Team> Teams { get; set; } = new();
        public Dictionary<int, TeamStatistics> TeamStats { get; set; } = new();

        public async Task OnGetAsync()
        {
            Teams = await _context.Teams
                .Include(t => t.LeadUser)
                .Include(t => t.TeamMembers.Where(tm => tm.IsActive))
                    .ThenInclude(tm => tm.User)
                .Where(t => t.IsActive)
                .OrderBy(t => t.Name)
                .ToListAsync();

            // Calculate statistics for each team
            foreach (var team in Teams)
            {
                var activeCases = await _context.Cases
                    .Where(c => c.AssignedTeamId == team.Id)
                    .CountAsync();

                var totalTasks = await _context.Tasks
                    .Where(t => t.TeamId == team.Id)
                    .CountAsync();

                var activeTasks = await _context.Tasks
                    .Where(t => t.TeamId == team.Id && 
                               t.Status != raf_pnp.Models.TaskStatus.Completed && 
                               t.Status != raf_pnp.Models.TaskStatus.Cancelled)
                    .CountAsync();

                var completedTasks = await _context.Tasks
                    .Where(t => t.TeamId == team.Id && 
                               t.Status == raf_pnp.Models.TaskStatus.Completed)
                    .CountAsync();

                TeamStats[team.Id] = new TeamStatistics
                {
                    ActiveCases = activeCases,
                    TotalTasks = totalTasks,
                    ActiveTasks = activeTasks,
                    CompletedTasks = completedTasks
                };
            }
        }
    }
}
