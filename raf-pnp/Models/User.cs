using System.ComponentModel.DataAnnotations;

namespace raf_pnp.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(256)]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [StringLength(20)]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "WhatsApp Notifications")]
        public bool WhatsAppNotificationsEnabled { get; set; } = false;

        [Display(Name = "Phone Verified")]
        public bool PhoneNumberVerified { get; set; } = false;

        [Display(Name = "Preferred Notification Channel")]
        public NotificationChannel PreferredNotificationChannel { get; set; } = NotificationChannel.InApp;

        [StringLength(50)]
        [Display(Name = "Role")]
        public string? Role { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        public ICollection<TeamMember> TeamMemberships { get; set; } = new List<TeamMember>();
        public ICollection<Team> LeadingTeams { get; set; } = new List<Team>();
        public ICollection<RafTask> AssignedTasks { get; set; } = new List<RafTask>();
        public ICollection<RafTask> CreatedTasks { get; set; } = new List<RafTask>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
