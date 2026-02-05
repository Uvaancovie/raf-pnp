using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using raf_pnp.Data;
using raf_pnp.Models;

namespace raf_pnp.Pages.Reports
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public int TotalCases { get; set; }
        public int ActiveCases { get; set; }
        public int ClosedCases { get; set; }
        public int TotalClients { get; set; }
        public int AwaitingMmi { get; set; }
        public int In120DayPeriod { get; set; }
        public int PendingExperts { get; set; }
        public int InLitigation { get; set; }
        public int FinalisedThisYear { get; set; }

        public List<CaseStatusCount> CasesByStatus { get; set; } = new();
        public List<DeadlineItem> ApproachingDeadlines { get; set; } = new();

        public async Task OnGetAsync()
        {
            TotalCases = await _context.Cases.CountAsync();
            ActiveCases = await _context.Cases.CountAsync(c => 
                c.Status != CaseStatus.Closed && c.Status != CaseStatus.Finalised);
            ClosedCases = await _context.Cases.CountAsync(c => 
                c.Status == CaseStatus.Closed || c.Status == CaseStatus.Finalised);
            TotalClients = await _context.Clients.CountAsync();

            AwaitingMmi = await _context.Cases.CountAsync(c => c.Status == CaseStatus.MmiWaitingPeriod);
            In120DayPeriod = await _context.Cases.CountAsync(c => c.Status == CaseStatus.StatutoryWaitingPeriod);
            PendingExperts = await _context.Cases.CountAsync(c => c.Status == CaseStatus.ExpertAppointments);
            InLitigation = await _context.Cases.CountAsync(c => 
                c.Status == CaseStatus.SummonsIssued || 
                c.Status == CaseStatus.PajaApplication ||
                c.Status == CaseStatus.PleadingPhase ||
                c.Status == CaseStatus.PreTrialPhase ||
                c.Status == CaseStatus.TrialPhase);
            
            var startOfYear = new DateTime(DateTime.Now.Year, 1, 1);
            FinalisedThisYear = await _context.Cases.CountAsync(c => 
                c.Status == CaseStatus.Finalised && c.DateClosed >= startOfYear);

            // Cases by status
            var statusGroups = await _context.Cases
                .GroupBy(c => c.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            CasesByStatus = statusGroups.Select(s => new CaseStatusCount
            {
                Status = s.Status,
                StatusName = GetStatusDisplayName(s.Status),
                Count = s.Count
            }).OrderBy(s => s.Status).ToList();

            // Approaching deadlines
            var today = DateTime.Today;
            var next60Days = today.AddDays(60);

            var casesWithMmiDeadline = await _context.Cases
                .Include(c => c.Client)
                .Where(c => c.MmiDate.HasValue && c.MmiDate <= next60Days && c.MmiDate >= today)
                .Where(c => c.Status == CaseStatus.MmiWaitingPeriod)
                .ToListAsync();

            var casesWithStatutoryDeadline = await _context.Cases
                .Include(c => c.Client)
                .Where(c => c.StatutoryExpiryDate.HasValue && c.StatutoryExpiryDate <= next60Days && c.StatutoryExpiryDate >= today)
                .Where(c => c.Status == CaseStatus.StatutoryWaitingPeriod)
                .ToListAsync();

            ApproachingDeadlines = casesWithMmiDeadline.Select(c => new DeadlineItem
            {
                CaseId = c.Id,
                CaseNumber = c.CaseNumber,
                ClientName = c.Client?.FullName ?? "Unknown",
                DeadlineType = "MMI Date",
                DeadlineDate = c.MmiDate!.Value,
                DaysRemaining = (c.MmiDate!.Value - today).Days
            }).Concat(casesWithStatutoryDeadline.Select(c => new DeadlineItem
            {
                CaseId = c.Id,
                CaseNumber = c.CaseNumber,
                ClientName = c.Client?.FullName ?? "Unknown",
                DeadlineType = "120-Day Expiry",
                DeadlineDate = c.StatutoryExpiryDate!.Value,
                DaysRemaining = (c.StatutoryExpiryDate!.Value - today).Days
            }))
            .OrderBy(d => d.DaysRemaining)
            .ToList();
        }

        private string GetStatusDisplayName(CaseStatus status)
        {
            return status switch
            {
                CaseStatus.ClientIntake => "Client Intake",
                CaseStatus.InitialLodgement => "Initial Lodgement",
                CaseStatus.MmiWaitingPeriod => "MMI Waiting",
                CaseStatus.ExpertAppointments => "Expert Appointments",
                CaseStatus.ComplianceLodgement => "Compliance Lodgement",
                CaseStatus.StatutoryWaitingPeriod => "120-Day Wait",
                CaseStatus.SummonsIssued => "Summons Issued",
                CaseStatus.PajaApplication => "PAJA Application",
                CaseStatus.PleadingPhase => "Pleading Phase",
                CaseStatus.PreTrialPhase => "Pre-Trial",
                CaseStatus.TrialPhase => "Trial",
                CaseStatus.Finalised => "Finalised",
                CaseStatus.Closed => "Closed",
                _ => status.ToString()
            };
        }

        public class CaseStatusCount
        {
            public CaseStatus Status { get; set; }
            public string StatusName { get; set; } = string.Empty;
            public int Count { get; set; }
        }

        public class DeadlineItem
        {
            public int CaseId { get; set; }
            public string CaseNumber { get; set; } = string.Empty;
            public string ClientName { get; set; } = string.Empty;
            public string DeadlineType { get; set; } = string.Empty;
            public DateTime DeadlineDate { get; set; }
            public int DaysRemaining { get; set; }
        }
    }
}
