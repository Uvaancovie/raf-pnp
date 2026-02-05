using Microsoft.EntityFrameworkCore;
using raf_pnp.Data;
using raf_pnp.Models;
using TaskStatus = raf_pnp.Models.TaskStatus;

namespace raf_pnp.Services
{
    public interface ITeamService
    {
        Task<Team?> GetTeamByIdAsync(int id);
        Task<List<Team>> GetAllTeamsAsync(bool includeInactive = false);
        Task<List<Team>> GetTeamsByLeadAsync(int leadUserId);
        Task<Team> CreateTeamAsync(Team team);
        Task<Team> UpdateTeamAsync(Team team);
        Task<bool> DeleteTeamAsync(int id);
        Task<bool> AddTeamMemberAsync(int teamId, int userId, TeamRole role);
        Task<bool> RemoveTeamMemberAsync(int teamId, int userId);
        Task<bool> UpdateTeamMemberRoleAsync(int teamId, int userId, TeamRole newRole);
        Task<List<TeamMember>> GetTeamMembersAsync(int teamId);
        Task<Dictionary<string, object>> GetTeamStatisticsAsync(int teamId);
        Task<bool> AssignTeamToCaseAsync(int teamId, int caseId);
        Task<bool> UnassignTeamFromCaseAsync(int caseId);
    }

    public class TeamService : ITeamService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TeamService> _logger;

        public TeamService(ApplicationDbContext context, ILogger<TeamService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Team?> GetTeamByIdAsync(int id)
        {
            return await _context.Teams
                .Include(t => t.LeadUser)
                .Include(t => t.TeamMembers)
                    .ThenInclude(tm => tm.User)
                .Include(t => t.AssignedCases)
                .Include(t => t.Tasks)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Team>> GetAllTeamsAsync(bool includeInactive = false)
        {
            var query = _context.Teams
                .Include(t => t.LeadUser)
                .Include(t => t.TeamMembers)
                    .ThenInclude(tm => tm.User)
                .AsQueryable();

            if (!includeInactive)
            {
                query = query.Where(t => t.IsActive);
            }

            return await query.OrderBy(t => t.Name).ToListAsync();
        }

        public async Task<List<Team>> GetTeamsByLeadAsync(int leadUserId)
        {
            return await _context.Teams
                .Include(t => t.TeamMembers)
                    .ThenInclude(tm => tm.User)
                .Where(t => t.LeadUserId == leadUserId && t.IsActive)
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        public async Task<Team> CreateTeamAsync(Team team)
        {
            team.CreatedDate = DateTime.Now;
            team.IsActive = true;

            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Team created: {team.Name} (ID: {team.Id})");
            return team;
        }

        public async Task<Team> UpdateTeamAsync(Team team)
        {
            _context.Teams.Update(team);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Team updated: {team.Name} (ID: {team.Id})");
            return team;
        }

        public async Task<bool> DeleteTeamAsync(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
                return false;

            // Soft delete
            team.IsActive = false;
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Team deactivated: {team.Name} (ID: {team.Id})");
            return true;
        }

        public async Task<bool> AddTeamMemberAsync(int teamId, int userId, TeamRole role)
        {
            // Check if already exists
            var existing = await _context.TeamMembers
                .FirstOrDefaultAsync(tm => tm.TeamId == teamId && tm.UserId == userId);

            if (existing != null)
            {
                // Reactivate if inactive
                if (!existing.IsActive)
                {
                    existing.IsActive = true;
                    existing.Role = role;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false; // Already active member
            }

            var teamMember = new TeamMember
            {
                TeamId = teamId,
                UserId = userId,
                Role = role,
                JoinedDate = DateTime.Now,
                IsActive = true
            };

            _context.TeamMembers.Add(teamMember);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"User {userId} added to team {teamId} as {role}");
            return true;
        }

        public async Task<bool> RemoveTeamMemberAsync(int teamId, int userId)
        {
            var teamMember = await _context.TeamMembers
                .FirstOrDefaultAsync(tm => tm.TeamId == teamId && tm.UserId == userId);

            if (teamMember == null)
                return false;

            // Soft delete
            teamMember.IsActive = false;
            await _context.SaveChangesAsync();

            _logger.LogInformation($"User {userId} removed from team {teamId}");
            return true;
        }

        public async Task<bool> UpdateTeamMemberRoleAsync(int teamId, int userId, TeamRole newRole)
        {
            var teamMember = await _context.TeamMembers
                .FirstOrDefaultAsync(tm => tm.TeamId == teamId && tm.UserId == userId && tm.IsActive);

            if (teamMember == null)
                return false;

            teamMember.Role = newRole;
            await _context.SaveChangesAsync();

            _logger.LogInformation($"User {userId} role updated to {newRole} in team {teamId}");
            return true;
        }

        public async Task<List<TeamMember>> GetTeamMembersAsync(int teamId)
        {
            return await _context.TeamMembers
                .Include(tm => tm.User)
                .Where(tm => tm.TeamId == teamId && tm.IsActive)
                .OrderByDescending(tm => tm.Role)
                .ThenBy(tm => tm.User.FullName)
                .ToListAsync();
        }

        public async Task<Dictionary<string, object>> GetTeamStatisticsAsync(int teamId)
        {
            var team = await _context.Teams
                .Include(t => t.TeamMembers.Where(tm => tm.IsActive))
                .Include(t => t.AssignedCases)
                .Include(t => t.Tasks)
                .FirstOrDefaultAsync(t => t.Id == teamId);

            if (team == null)
                return new Dictionary<string, object>();

            var activeTasks = team.Tasks.Count(t => t.Status != TaskStatus.Completed && t.Status != TaskStatus.Cancelled);
            var completedTasks = team.Tasks.Count(t => t.Status == TaskStatus.Completed);
            var overdueTasks = team.Tasks.Count(t => t.IsOverdue);

            return new Dictionary<string, object>
            {
                { "TotalMembers", team.TeamMembers.Count },
                { "ActiveCases", team.AssignedCases.Count },
                { "TotalTasks", team.Tasks.Count },
                { "ActiveTasks", activeTasks },
                { "CompletedTasks", completedTasks },
                { "OverdueTasks", overdueTasks },
                { "CompletionRate", team.Tasks.Count > 0 ? (double)completedTasks / team.Tasks.Count * 100 : 0 }
            };
        }

        public async Task<bool> AssignTeamToCaseAsync(int teamId, int caseId)
        {
            var rafCase = await _context.Cases.FindAsync(caseId);
            if (rafCase == null)
                return false;

            rafCase.AssignedTeamId = teamId;
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Team {teamId} assigned to case {caseId}");
            return true;
        }

        public async Task<bool> UnassignTeamFromCaseAsync(int caseId)
        {
            var rafCase = await _context.Cases.FindAsync(caseId);
            if (rafCase == null)
                return false;

            rafCase.AssignedTeamId = null;
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Team unassigned from case {caseId}");
            return true;
        }
    }
}
