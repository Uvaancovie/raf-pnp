using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace raf_pnp.Models
{
    public enum ActivityType
    {
        [Display(Name = "Note Added")]
        NoteAdded,
        [Display(Name = "Document Uploaded")]
        DocumentUploaded,
        [Display(Name = "Status Changed")]
        StatusChanged,
        [Display(Name = "Client Contact")]
        ClientContact,
        [Display(Name = "RAF Communication")]
        RafCommunication,
        [Display(Name = "Court Filing")]
        CourtFiling,
        [Display(Name = "Expert Appointment")]
        ExpertAppointment,
        [Display(Name = "Diary Entry")]
        DiaryEntry,
        [Display(Name = "Reminder")]
        Reminder,
        [Display(Name = "Other")]
        Other
    }

    public class CaseActivity
    {
        public int Id { get; set; }

        [Required]
        public int CaseId { get; set; }

        [ForeignKey("CaseId")]
        public RafCase? Case { get; set; }

        [Required]
        [Display(Name = "Activity Type")]
        public ActivityType ActivityType { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000)]
        public string? Description { get; set; }

        [Display(Name = "Activity Date")]
        public DateTime ActivityDate { get; set; } = DateTime.Now;

        [StringLength(100)]
        [Display(Name = "Created By")]
        public string? CreatedBy { get; set; }

        [Display(Name = "Is Reminder")]
        public bool IsReminder { get; set; }

        [Display(Name = "Reminder Date")]
        [DataType(DataType.DateTime)]
        public DateTime? ReminderDate { get; set; }

        [Display(Name = "Reminder Completed")]
        public bool ReminderCompleted { get; set; }
    }
}
