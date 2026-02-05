using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace raf_pnp.Models
{
    public enum NotificationType
    {
        [Display(Name = "Task Assigned")]
        TaskAssigned,
        [Display(Name = "Task Due Soon")]
        TaskDueSoon,
        [Display(Name = "Task Overdue")]
        TaskOverdue,
        [Display(Name = "Task Completed")]
        TaskCompleted,
        [Display(Name = "Task Commented")]
        TaskCommented,
        [Display(Name = "Case Status Changed")]
        CaseStatusChanged,
        [Display(Name = "Deadline Approaching")]
        DeadlineApproaching,
        [Display(Name = "Team Update")]
        TeamUpdate,
        [Display(Name = "System Update")]
        SystemUpdate
    }

    public enum NotificationChannel
    {
        [Display(Name = "In-App")]
        InApp,
        [Display(Name = "Email")]
        Email,
        [Display(Name = "WhatsApp")]
        WhatsApp,
        [Display(Name = "All Channels")]
        All
    }

    public class Notification
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "User")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        [Required]
        [StringLength(200)]
        [Display(Name = "Title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        [Display(Name = "Message")]
        public string Message { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Type")]
        public NotificationType Type { get; set; }

        // Reference to related entity
        [Display(Name = "Related Task")]
        public int? TaskId { get; set; }

        [ForeignKey("TaskId")]
        public RafTask? Task { get; set; }

        [Display(Name = "Related Case")]
        public int? CaseId { get; set; }

        [ForeignKey("CaseId")]
        public RafCase? Case { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Read")]
        public bool IsRead { get; set; } = false;

        [Display(Name = "Read Date")]
        public DateTime? ReadDate { get; set; }

        // For action items
        [StringLength(500)]
        [Display(Name = "Action URL")]
        public string? ActionUrl { get; set; }

        [Display(Name = "Requires Action")]
        public bool RequiresAction { get; set; } = false;
    }
}
