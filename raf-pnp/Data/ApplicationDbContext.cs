using Microsoft.EntityFrameworkCore;
using raf_pnp.Models;

namespace raf_pnp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<RafCase> Cases { get; set; }
        public DbSet<CaseDocument> CaseDocuments { get; set; }
        public DbSet<ExpertAppointment> ExpertAppointments { get; set; }
        public DbSet<CaseActivity> CaseActivities { get; set; }
        
        // Task and Team Management
        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<RafTask> Tasks { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }
        public DbSet<TaskAttachment> TaskAttachments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<WhatsAppMessage> WhatsAppMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Client
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasIndex(e => e.IdNumber).IsUnique();
                entity.HasMany(c => c.Cases)
                      .WithOne(r => r.Client)
                      .HasForeignKey(r => r.ClientId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure RafCase
            modelBuilder.Entity<RafCase>(entity =>
            {
                entity.HasIndex(e => e.CaseNumber).IsUnique();
                entity.HasMany(c => c.Documents)
                      .WithOne(d => d.Case)
                      .HasForeignKey(d => d.CaseId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(c => c.ExpertAppointments)
                      .WithOne(e => e.Case)
                      .HasForeignKey(e => e.CaseId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(c => c.Activities)
                      .WithOne(a => a.Case)
                      .HasForeignKey(a => a.CaseId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(c => c.Tasks)
                      .WithOne(t => t.Case)
                      .HasForeignKey(t => t.CaseId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(c => c.AssignedTeam)
                      .WithMany(t => t.AssignedCases)
                      .HasForeignKey(c => c.AssignedTeamId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasMany(u => u.TeamMemberships)
                      .WithOne(tm => tm.User)
                      .HasForeignKey(tm => tm.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(u => u.AssignedTasks)
                      .WithOne(t => t.AssignedToUser)
                      .HasForeignKey(t => t.AssignedToUserId)
                      .OnDelete(DeleteBehavior.SetNull);
                entity.HasMany(u => u.CreatedTasks)
                      .WithOne(t => t.CreatedByUser)
                      .HasForeignKey(t => t.CreatedByUserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Team
            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasIndex(e => e.Name);
                entity.HasOne(t => t.LeadUser)
                      .WithMany(u => u.LeadingTeams)
                      .HasForeignKey(t => t.LeadUserId)
                      .OnDelete(DeleteBehavior.SetNull);
                entity.HasMany(t => t.TeamMembers)
                      .WithOne(tm => tm.Team)
                      .HasForeignKey(tm => tm.TeamId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(t => t.Tasks)
                      .WithOne(task => task.Team)
                      .HasForeignKey(task => task.TeamId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure TeamMember
            modelBuilder.Entity<TeamMember>(entity =>
            {
                entity.HasIndex(e => new { e.TeamId, e.UserId }).IsUnique();
            });

            // Configure RafTask
            modelBuilder.Entity<RafTask>(entity =>
            {
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.DueDate);
                entity.HasIndex(e => e.AssignedToUserId);
                entity.HasIndex(e => e.TeamId);
                entity.HasMany(t => t.Comments)
                      .WithOne(c => c.Task)
                      .HasForeignKey(c => c.TaskId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(t => t.Attachments)
                      .WithOne(a => a.Task)
                      .HasForeignKey(a => a.TaskId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Notification
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.IsRead);
                entity.HasIndex(e => e.CreatedDate);
                entity.HasOne(n => n.User)
                      .WithMany(u => u.Notifications)
                      .HasForeignKey(n => n.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure WhatsAppMessage
            modelBuilder.Entity<WhatsAppMessage>(entity =>
            {
                entity.HasIndex(e => e.ToPhoneNumber);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.CreatedDate);
            });
        }
    }
}
