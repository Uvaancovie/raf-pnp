using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace raf_pnp.Models
{
    public enum WhatsAppMessageStatus
    {
        [Display(Name = "Queued")]
        Queued,
        [Display(Name = "Sent")]
        Sent,
        [Display(Name = "Delivered")]
        Delivered,
        [Display(Name = "Read")]
        Read,
        [Display(Name = "Failed")]
        Failed
    }

    public class WhatsAppMessage
    {
        public int Id { get; set; }

        [Required]
        [Phone]
        [StringLength(20)]
        [Display(Name = "To Phone Number")]
        public string ToPhoneNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        [Display(Name = "Message Body")]
        public string MessageBody { get; set; } = string.Empty;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Status")]
        public WhatsAppMessageStatus Status { get; set; } = WhatsAppMessageStatus.Queued;

        [Display(Name = "Delivered Date")]
        public DateTime? DeliveredDate { get; set; }

        [Display(Name = "Read Date")]
        public DateTime? ReadDate { get; set; }

        // Reference to related entities
        [Display(Name = "Related Task")]
        public int? TaskId { get; set; }

        [ForeignKey("TaskId")]
        public RafTask? Task { get; set; }

        [Display(Name = "Related Case")]
        public int? CaseId { get; set; }

        [ForeignKey("CaseId")]
        public RafCase? Case { get; set; }

        [Display(Name = "User")]
        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Display(Name = "Simulated (Offline Mode)")]
        public bool IsSimulated { get; set; } = true;

        [StringLength(100)]
        [Display(Name = "External Message ID")]
        public string? ExternalMessageId { get; set; }

        [StringLength(500)]
        [Display(Name = "Error Message")]
        public string? ErrorMessage { get; set; }
    }
}
