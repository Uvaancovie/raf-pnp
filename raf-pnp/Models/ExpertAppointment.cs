using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace raf_pnp.Models
{
    public enum ExpertType
    {
        [Display(Name = "Neurosurgeon")]
        Neurosurgeon,
        [Display(Name = "Maxillofacial Surgeon")]
        MaxillofacialSurgeon,
        [Display(Name = "Clinical Psychologist")]
        ClinicalPsychologist,
        [Display(Name = "Occupational Therapist")]
        OccupationalTherapist,
        [Display(Name = "Industrial Psychologist")]
        IndustrialPsychologist,
        [Display(Name = "Actuary")]
        Actuary,
        [Display(Name = "Orthopaedic Surgeon")]
        OrthopaedicSurgeon,
        [Display(Name = "General Practitioner")]
        GeneralPractitioner,
        [Display(Name = "Physiotherapist")]
        Physiotherapist,
        [Display(Name = "Other")]
        Other
    }

    public enum AppointmentStatus
    {
        [Display(Name = "Scheduled")]
        Scheduled,
        [Display(Name = "Completed")]
        Completed,
        [Display(Name = "Cancelled")]
        Cancelled,
        [Display(Name = "Report Received")]
        ReportReceived,
        [Display(Name = "Pending")]
        Pending
    }

    public class ExpertAppointment
    {
        public int Id { get; set; }

        [Required]
        public int CaseId { get; set; }

        [ForeignKey("CaseId")]
        public RafCase? Case { get; set; }

        [Required]
        [Display(Name = "Expert Type")]
        public ExpertType ExpertType { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Expert Name")]
        public string ExpertName { get; set; } = string.Empty;

        [StringLength(200)]
        [Display(Name = "Practice Name")]
        public string? PracticeName { get; set; }

        [StringLength(20)]
        [Display(Name = "Contact Number")]
        public string? ContactNumber { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        [Display(Name = "Appointment Date")]
        [DataType(DataType.DateTime)]
        public DateTime? AppointmentDate { get; set; }

        [Display(Name = "Status")]
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        [Display(Name = "Report Received Date")]
        [DataType(DataType.Date)]
        public DateTime? ReportReceivedDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Expert Fee")]
        [DataType(DataType.Currency)]
        public decimal? ExpertFee { get; set; }

        [Display(Name = "Fee Paid")]
        public bool FeePaid { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }
    }
}
