using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace raf_pnp.Models
{
    public enum TaskStatus
    {
        [Display(Name = "Not Started")]
        NotStarted,
        [Display(Name = "In Progress")]
        InProgress,
        [Display(Name = "Blocked")]
        Blocked,
        [Display(Name = "Under Review")]
        UnderReview,
        [Display(Name = "Completed")]
        Completed,
        [Display(Name = "Cancelled")]
        Cancelled
    }

    public enum TaskPriority
    {
        [Display(Name = "Low")]
        Low,
        [Display(Name = "Medium")]
        Medium,
        [Display(Name = "High")]
        High,
        [Display(Name = "Urgent")]
        Urgent
    }

    // Named RafTask to avoid conflict with System.Threading.Tasks.Task
    public class RafTask
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Title")]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        // Case relationship
        [Display(Name = "Related Case")]
        public int? CaseId { get; set; }

        [ForeignKey("CaseId")]
        public RafCase? Case { get; set; }

        // Team relationship
        [Display(Name = "Team")]
        public int? TeamId { get; set; }

        [ForeignKey("TeamId")]
        public Team? Team { get; set; }

        // Assignment
        [Display(Name = "Assigned To")]
        public int? AssignedToUserId { get; set; }

        [ForeignKey("AssignedToUserId")]
        public User? AssignedToUser { get; set; }

        [Required]
        [Display(Name = "Created By")]
        public int CreatedByUserId { get; set; }

        [ForeignKey("CreatedByUserId")]
        public User CreatedByUser { get; set; } = null!;

        // Status and priority
        [Required]
        [Display(Name = "Status")]
        public TaskStatus Status { get; set; } = TaskStatus.NotStarted;

        [Required]
        [Display(Name = "Priority")]
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        // RAF workflow integration
        [Display(Name = "Related Case Status")]
        public CaseStatus? RelatedCaseStatus { get; set; }

        [Display(Name = "Workflow Task")]
        public bool IsWorkflowTask { get; set; } = false;

        // Dates
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }

        [Display(Name = "Completed Date")]
        [DataType(DataType.Date)]
        public DateTime? CompletedDate { get; set; }

        // Navigation properties
        public ICollection<TaskComment> Comments { get; set; } = new List<TaskComment>();
        public ICollection<TaskAttachment> Attachments { get; set; } = new List<TaskAttachment>();

        // Computed properties
        [Display(Name = "Is Overdue")]
        public bool IsOverdue => DueDate.HasValue && DueDate.Value < DateTime.Now && Status != TaskStatus.Completed;

        [Display(Name = "Days Until Due")]
        public int? DaysUntilDue => DueDate.HasValue ? (DueDate.Value - DateTime.Now).Days : null;
    }

    public class TaskComment
    {
        public int Id { get; set; }

        [Required]
        public int TaskId { get; set; }

        [ForeignKey("TaskId")]
        public RafTask Task { get; set; } = null!;

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        [Required]
        [StringLength(1000)]
        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }

    public class TaskAttachment
    {
        public int Id { get; set; }

        [Required]
        public int TaskId { get; set; }

        [ForeignKey("TaskId")]
        public RafTask Task { get; set; } = null!;

        [Required]
        [StringLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string FilePath { get; set; } = string.Empty;

        [StringLength(100)]
        public string? FileType { get; set; }

        public long FileSize { get; set; }

        public int UploadedByUserId { get; set; }

        [ForeignKey("UploadedByUserId")]
        public User UploadedByUser { get; set; } = null!;

        public DateTime UploadedDate { get; set; } = DateTime.Now;
    }
}
