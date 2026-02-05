using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace raf_pnp.Models
{
    public enum CaseStatus
    {
        [Display(Name = "Client Intake")]
        ClientIntake,
        [Display(Name = "Initial Lodgement (Non-Compliance)")]
        InitialLodgement,
        [Display(Name = "MMI Waiting Period")]
        MmiWaitingPeriod,
        [Display(Name = "Expert Appointments")]
        ExpertAppointments,
        [Display(Name = "Compliance Lodgement")]
        ComplianceLodgement,
        [Display(Name = "120-Day Waiting Period")]
        StatutoryWaitingPeriod,
        [Display(Name = "Summons Issued")]
        SummonsIssued,
        [Display(Name = "PAJA Application")]
        PajaApplication,
        [Display(Name = "Pleading Phase")]
        PleadingPhase,
        [Display(Name = "Pre-Trial Phase")]
        PreTrialPhase,
        [Display(Name = "Trial Phase")]
        TrialPhase,
        [Display(Name = "Finalised")]
        Finalised,
        [Display(Name = "Closed")]
        Closed
    }

    public class RafCase
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Case Number")]
        public string CaseNumber { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Client")]
        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        public Client? Client { get; set; }

        [Required]
        [Display(Name = "Accident Date")]
        [DataType(DataType.Date)]
        public DateTime AccidentDate { get; set; }

        [StringLength(500)]
        [Display(Name = "Accident Description")]
        public string? AccidentDescription { get; set; }

        [StringLength(200)]
        [Display(Name = "Accident Location")]
        public string? AccidentLocation { get; set; }

        [Required]
        [Display(Name = "Current Status")]
        public CaseStatus Status { get; set; } = CaseStatus.ClientIntake;

        [Display(Name = "Date Opened")]
        public DateTime DateOpened { get; set; } = DateTime.Now;

        [Display(Name = "Date Closed")]
        public DateTime? DateClosed { get; set; }

        [Display(Name = "Initial Lodgement Date")]
        [DataType(DataType.Date)]
        public DateTime? InitialLodgementDate { get; set; }

        [Display(Name = "Compliance Lodgement Date")]
        [DataType(DataType.Date)]
        public DateTime? ComplianceLodgementDate { get; set; }

        [Display(Name = "MMI Date (Expected)")]
        [DataType(DataType.Date)]
        public DateTime? MmiDate { get; set; }

        [Display(Name = "120-Day Expiry Date")]
        [DataType(DataType.Date)]
        public DateTime? StatutoryExpiryDate { get; set; }

        [Display(Name = "Summons Issue Date")]
        [DataType(DataType.Date)]
        public DateTime? SummonsIssueDate { get; set; }

        [StringLength(100)]
        [Display(Name = "Assigned Attorney")]
        public string? AssignedAttorney { get; set; }

        [StringLength(100)]
        [Display(Name = "Candidate Attorney")]
        public string? CandidateAttorney { get; set; }

        [Display(Name = "Fee Agreement Signed")]
        public bool FeeAgreementSigned { get; set; }

        [Display(Name = "Contingency Fee Agreement")]
        public bool ContingencyFeeAgreement { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Estimated Claim Value")]
        [DataType(DataType.Currency)]
        public decimal? EstimatedClaimValue { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Settlement Amount")]
        [DataType(DataType.Currency)]
        public decimal? SettlementAmount { get; set; }

        [StringLength(2000)]
        public string? Notes { get; set; }

        // Team assignment
        [Display(Name = "Assigned Team")]
        public int? AssignedTeamId { get; set; }

        [ForeignKey("AssignedTeamId")]
        public Team? AssignedTeam { get; set; }

        // Navigation properties
        public ICollection<CaseDocument> Documents { get; set; } = new List<CaseDocument>();
        public ICollection<ExpertAppointment> ExpertAppointments { get; set; } = new List<ExpertAppointment>();
        public ICollection<CaseActivity> Activities { get; set; } = new List<CaseActivity>();
        public ICollection<RafTask> Tasks { get; set; } = new List<RafTask>();

        // Computed properties
        [Display(Name = "Days Since Accident")]
        public int DaysSinceAccident => (DateTime.Now - AccidentDate).Days;

        [Display(Name = "Days Until MMI")]
        public int? DaysUntilMmi => MmiDate.HasValue ? (MmiDate.Value - DateTime.Now).Days : null;

        [Display(Name = "Days Until 120-Day Expiry")]
        public int? DaysUntilStatutoryExpiry => StatutoryExpiryDate.HasValue ? (StatutoryExpiryDate.Value - DateTime.Now).Days : null;
    }
}
