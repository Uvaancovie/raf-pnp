using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace raf_pnp.Models
{
    public enum TeamRole
    {
        [Display(Name = "Member")]
        Member,
        [Display(Name = "Team Lead")]
        Lead,
        [Display(Name = "Administrator")]
        Admin
    }

    public class Team
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Team Name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Team Lead")]
        public int? LeadUserId { get; set; }

        [ForeignKey("LeadUserId")]
        public User? LeadUser { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
        public ICollection<RafTask> Tasks { get; set; } = new List<RafTask>();
        public ICollection<RafCase> AssignedCases { get; set; } = new List<RafCase>();
    }

    public class TeamMember
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Team")]
        public int TeamId { get; set; }

        [ForeignKey("TeamId")]
        public Team Team { get; set; } = null!;

        [Required]
        [Display(Name = "User")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        [Required]
        [Display(Name = "Role")]
        public TeamRole Role { get; set; } = TeamRole.Member;

        [Display(Name = "Joined Date")]
        public DateTime JoinedDate { get; set; } = DateTime.Now;

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
    }
}
