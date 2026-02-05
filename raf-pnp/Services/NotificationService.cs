using Microsoft.EntityFrameworkCore;
using raf_pnp.Data;
using raf_pnp.Models;
using TaskStatus = raf_pnp.Models.TaskStatus;

namespace raf_pnp.Services
{
    public interface INotificationService
    {
        Task<Notification> CreateNotificationAsync(int userId, string title, string message, NotificationType type, int? taskId = null, int? caseId = null, string? actionUrl = null);
        Task<List<Notification>> GetUserNotificationsAsync(int userId, bool unreadOnly = false);
        Task<int> GetUnreadCountAsync(int userId);
        Task<bool> MarkAsReadAsync(int notificationId);
        Task<bool> MarkAllAsReadAsync(int userId);
        Task<bool> DeleteNotificationAsync(int notificationId);
        Task NotifyTaskAssignedAsync(RafTask task);
        Task NotifyTaskDueSoonAsync(RafTask task);
        Task NotifyCaseStatusChangedAsync(RafCase rafCase, CaseStatus oldStatus);
    }

    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<NotificationService> _logger;
        private readonly IWhatsAppService _whatsAppService;

        public NotificationService(
            ApplicationDbContext context,
            ILogger<NotificationService> logger,
            IWhatsAppService whatsAppService)
        {
            _context = context;
            _logger = logger;
            _whatsAppService = whatsAppService;
        }

        public async Task<Notification> CreateNotificationAsync(
            int userId,
            string title,
            string message,
            NotificationType type,
            int? taskId = null,
            int? caseId = null,
            string? actionUrl = null)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                Type = type,
                TaskId = taskId,
                CaseId = caseId,
                ActionUrl = actionUrl,
                RequiresAction = !string.IsNullOrEmpty(actionUrl),
                CreatedDate = DateTime.Now,
                IsRead = false
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            // Send WhatsApp notification if user has it enabled
            var user = await _context.Users.FindAsync(userId);
            if (user != null && user.WhatsAppNotificationsEnabled && 
                (user.PreferredNotificationChannel == NotificationChannel.WhatsApp ||
                 user.PreferredNotificationChannel == NotificationChannel.All))
            {
                await SendWhatsAppNotificationAsync(user, title, message, actionUrl);
            }

            _logger.LogInformation($"Notification created for user {userId}: {title}");
            return notification;
        }

        public async Task<List<Notification>> GetUserNotificationsAsync(int userId, bool unreadOnly = false)
        {
            var query = _context.Notifications
                .Include(n => n.Task)
                .Include(n => n.Case)
                .Where(n => n.UserId == userId);

            if (unreadOnly)
            {
                query = query.Where(n => !n.IsRead);
            }

            return await query
                .OrderByDescending(n => n.CreatedDate)
                .Take(50) // Limit to last 50 notifications
                .ToListAsync();
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _context.Notifications
                .CountAsync(n => n.UserId == userId && !n.IsRead);
        }

        public async Task<bool> MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null)
                return false;

            notification.IsRead = true;
            notification.ReadDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> MarkAllAsReadAsync(int userId)
        {
            var unreadNotifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();

            foreach (var notification in unreadNotifications)
            {
                notification.IsRead = true;
                notification.ReadDate = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation($"Marked {unreadNotifications.Count} notifications as read for user {userId}");

            return true;
        }

        public async Task<bool> DeleteNotificationAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null)
                return false;

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task NotifyTaskAssignedAsync(RafTask task)
        {
            if (!task.AssignedToUserId.HasValue)
                return;

            var actionUrl = $"/Tasks/Details?id={task.Id}";
            var message = $"You have been assigned a new task: {task.Title}";

            if (task.DueDate.HasValue)
            {
                message += $" (Due: {task.DueDate.Value:MMM dd, yyyy})";
            }

            await CreateNotificationAsync(
                task.AssignedToUserId.Value,
                "New Task Assigned",
                message,
                NotificationType.TaskAssigned,
                taskId: task.Id,
                actionUrl: actionUrl
            );
        }

        public async Task NotifyTaskDueSoonAsync(RafTask task)
        {
            if (!task.AssignedToUserId.HasValue || !task.DueDate.HasValue)
                return;

            var daysUntilDue = (task.DueDate.Value - DateTime.Now).Days;
            var actionUrl = $"/Tasks/Details?id={task.Id}";
            var message = $"Task '{task.Title}' is due in {daysUntilDue} day(s)";

            await CreateNotificationAsync(
                task.AssignedToUserId.Value,
                "Task Due Soon",
                message,
                NotificationType.TaskDueSoon,
                taskId: task.Id,
                actionUrl: actionUrl
            );
        }

        public async Task NotifyCaseStatusChangedAsync(RafCase rafCase, CaseStatus oldStatus)
        {
            // Notify team members if case has an assigned team
            if (rafCase.AssignedTeamId.HasValue)
            {
                var teamMembers = await _context.TeamMembers
                    .Where(tm => tm.TeamId == rafCase.AssignedTeamId.Value && tm.IsActive)
                    .Select(tm => tm.UserId)
                    .ToListAsync();

                var message = $"Case {rafCase.CaseNumber} status changed from {oldStatus} to {rafCase.Status}";
                var actionUrl = $"/Cases/Details?id={rafCase.Id}";

                foreach (var userId in teamMembers)
                {
                    await CreateNotificationAsync(
                        userId,
                        "Case Status Updated",
                        message,
                        NotificationType.CaseStatusChanged,
                        caseId: rafCase.Id,
                        actionUrl: actionUrl
                    );
                }
            }
        }

        private async Task SendWhatsAppNotificationAsync(User user, string title, string message, string? actionUrl)
        {
            if (string.IsNullOrEmpty(user.PhoneNumber))
                return;

            var whatsAppMessage = $"🔔 *{title}*\n\n{message}";
            if (!string.IsNullOrEmpty(actionUrl))
            {
                whatsAppMessage += $"\n\nView: https://localhost:5001{actionUrl}";
            }

            try
            {
                // This will use the MockWhatsAppService in development
                await _whatsAppService.SendNotificationAsync(user.PhoneNumber, whatsAppMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send WhatsApp notification to {user.PhoneNumber}");
            }
        }
    }
}
