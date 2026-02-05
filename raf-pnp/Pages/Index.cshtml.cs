using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using raf_pnp.Data;
using raf_pnp.Models;

namespace raf_pnp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ApplicationDbContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public int TotalCases { get; set; }
        public int ActiveCases { get; set; }
        public int AwaitingMmi { get; set; }
        public int TotalClients { get; set; }
        public List<RafCase> UrgentCases { get; set; } = new();

        public async Task OnGetAsync()
        {
            TotalCases = await _context.Cases.CountAsync();
            ActiveCases = await _context.Cases
                .Where(c => c.Status != CaseStatus.Closed && c.Status != CaseStatus.Finalised)
                .CountAsync();
            AwaitingMmi = await _context.Cases
                .Where(c => c.Status == CaseStatus.MmiWaitingPeriod)
                .CountAsync();
            TotalClients = await _context.Clients.CountAsync();

            // Get cases requiring attention (approaching deadlines or in critical phases)
            UrgentCases = await _context.Cases
                .Include(c => c.Client)
                .Where(c => c.Status != CaseStatus.Closed && c.Status != CaseStatus.Finalised)
                .OrderByDescending(c => c.AccidentDate)
                .Take(5)
                .ToListAsync();
        }
    }
}
