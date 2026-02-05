using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace raf_pnp.Models
{
    public enum DocumentType
    {
        [Display(Name = "Accident Report")]
        AccidentReport,
        [Display(Name = "Hospital Records")]
        HospitalRecords,
        [Display(Name = "Medical Records")]
        MedicalRecords,
        [Display(Name = "Employment Records")]
        EmploymentRecords,
        [Display(Name = "Fee Agreement")]
        FeeAgreement,
        [Display(Name = "Contingency Fee Agreement")]
        ContingencyFeeAgreement,
        [Display(Name = "RAF 1 Form")]
        Raf1Form,
        [Display(Name = "RAF 4 Form")]
        Raf4Form,
        [Display(Name = "Expert Report")]
        ExpertReport,
        [Display(Name = "Summons")]
        Summons,
        [Display(Name = "Plea")]
        Plea,
        [Display(Name = "Notice of Intention to Defend")]
        NoticeOfIntention,
        [Display(Name = "Court Order")]
        CourtOrder,
        [Display(Name = "Correspondence")]
        Correspondence,
        [Display(Name = "Other")]
        Other
    }

    public class CaseDocument
    {
        public int Id { get; set; }

        [Required]
        public int CaseId { get; set; }

        [ForeignKey("CaseId")]
        public RafCase? Case { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Document Name")]
        public string DocumentName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Document Type")]
        public DocumentType DocumentType { get; set; }

        [StringLength(500)]
        [Display(Name = "File Path")]
        public string? FilePath { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Display(Name = "Date Uploaded")]
        public DateTime DateUploaded { get; set; } = DateTime.Now;

        [Display(Name = "Date Received")]
        [DataType(DataType.Date)]
        public DateTime? DateReceived { get; set; }

        [Display(Name = "Uploaded By")]
        [StringLength(100)]
        public string? UploadedBy { get; set; }
    }
}
