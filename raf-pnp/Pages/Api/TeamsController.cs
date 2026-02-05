using Microsoft.AspNetCore.Mvc;
using raf_pnp.Models;
using raf_pnp.Services;

namespace raf_pnp.Pages.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;
        private readonly ILogger<TeamsController> _logger;

        public TeamsController(ITeamService teamService, ILogger<TeamsController> logger)
        {
            _teamService = teamService;
            _logger = logger;
        }

        // GET: api/teams
        [HttpGet]
        public async Task<ActionResult<List<Team>>> GetTeams([FromQuery] bool includeInactive = false)
        {
            var teams = await _teamService.GetAllTeamsAsync(includeInactive);
            return Ok(teams);
        }

        // GET: api/teams/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetTeam(int id)
        {
            var team = await _teamService.GetTeamByIdAsync(id);
            if (team == null)
                return NotFound(new { message = "Team not found" });

            return Ok(team);
        }

        // GET: api/teams/{id}/statistics
        [HttpGet("{id}/statistics")]
        public async Task<ActionResult<Dictionary<string, object>>> GetTeamStatistics(int id)
        {
            var stats = await _teamService.GetTeamStatisticsAsync(id);
            if (stats.Count == 0)
                return NotFound(new { message = "Team not found" });

            return Ok(stats);
        }

        // GET: api/teams/{id}/members
        [HttpGet("{id}/members")]
        public async Task<ActionResult<List<TeamMember>>> GetTeamMembers(int id)
        {
            var members = await _teamService.GetTeamMembersAsync(id);
            return Ok(members);
        }

        // POST: api/teams
        [HttpPost]
        public async Task<ActionResult<Team>> CreateTeam([FromBody] Team team)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdTeam = await _teamService.CreateTeamAsync(team);
            return CreatedAtAction(nameof(GetTeam), new { id = createdTeam.Id }, createdTeam);
        }

        // PUT: api/teams/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<Team>> UpdateTeam(int id, [FromBody] Team team)
        {
            if (id != team.Id)
                return BadRequest(new { message = "ID mismatch" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedTeam = await _teamService.UpdateTeamAsync(team);
            return Ok(updatedTeam);
        }

        // DELETE: api/teams/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var result = await _teamService.DeleteTeamAsync(id);
            if (!result)
                return NotFound(new { message = "Team not found" });

            return NoContent();
        }

        // POST: api/teams/{id}/members
        [HttpPost("{id}/members")]
        public async Task<IActionResult> AddTeamMember(int id, [FromBody] AddTeamMemberRequest request)
        {
            var result = await _teamService.AddTeamMemberAsync(id, request.UserId, request.Role);
            if (!result)
                return BadRequest(new { message = "Failed to add team member" });

            return Ok(new { message = "Team member added successfully" });
        }

        // DELETE: api/teams/{id}/members/{userId}
        [HttpDelete("{id}/members/{userId}")]
        public async Task<IActionResult> RemoveTeamMember(int id, int userId)
        {
            var result = await _teamService.RemoveTeamMemberAsync(id, userId);
            if (!result)
                return NotFound(new { message = "Team member not found" });

            return NoContent();
        }

        // PUT: api/teams/{id}/members/{userId}/role
        [HttpPut("{id}/members/{userId}/role")]
        public async Task<IActionResult> UpdateTeamMemberRole(int id, int userId, [FromBody] UpdateRoleRequest request)
        {
            var result = await _teamService.UpdateTeamMemberRoleAsync(id, userId, request.Role);
            if (!result)
                return NotFound(new { message = "Team member not found" });

            return Ok(new { message = "Role updated successfully" });
        }

        // POST: api/teams/{id}/assign-case
        [HttpPost("{id}/assign-case")]
        public async Task<IActionResult> AssignTeamToCase(int id, [FromBody] AssignCaseRequest request)
        {
            var result = await _teamService.AssignTeamToCaseAsync(id, request.CaseId);
            if (!result)
                return NotFound(new { message = "Case not found" });

            return Ok(new { message = "Team assigned to case successfully" });
        }
    }

    // Request models
    public class AddTeamMemberRequest
    {
        public int UserId { get; set; }
        public TeamRole Role { get; set; }
    }

    public class UpdateRoleRequest
    {
        public TeamRole Role { get; set; }
    }

    public class AssignCaseRequest
    {
        public int CaseId { get; set; }
    }
}
