using Microsoft.EntityFrameworkCore;
using raf_pnp.Data;
using raf_pnp.Models;
using TaskStatus = raf_pnp.Models.TaskStatus;

namespace raf_pnp.Services
{
    public interface IWhatsAppService
    {
        Task<bool> SendNotificationAsync(string phoneNumber, string message, int? taskId = null, int? caseId = null, int? userId = null);
        Task<bool> SendTaskAssignment(User user, RafTask task);
        Task<bool> SendVerificationCode(string phoneNumber, string code);
        Task<bool> SendDeadlineReminder(User user, RafTask task);
        Task<bool> SendCaseUpdate(User user, RafCase rafCase, string updateMessage);
        Task<List<WhatsAppMessage>> GetRecentMessagesAsync(int count = 50);
        Task<List<WhatsAppMessage>> GetMessagesByPhoneAsync(string phoneNumber);
    }

    public class MockWhatsAppService : IWhatsAppService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MockWhatsAppService> _logger;

        public MockWhatsAppService(ApplicationDbContext context, ILogger<MockWhatsAppService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> SendNotificationAsync(string phoneNumber, string message, int? taskId = null, int? caseId = null, int? userId = null)
        {
            return await SaveMockMessage(phoneNumber, message, taskId, caseId, userId);
        }

        public async Task<bool> SendTaskAssignment(User user, RafTask task)
        {
            if (!user.WhatsAppNotificationsEnabled || string.IsNullOrEmpty(user.PhoneNumber))
                return false;

            var message = $"🎯 *New Task Assigned*\n" +
                         $"Task: {task.Title}\n" +
                         $"Priority: {task.Priority}\n" +
                         $"Due: {(task.DueDate.HasValue ? task.DueDate.Value.ToString("MMM dd, yyyy") : "Not set")}\n" +
                         $"View: https://localhost:5001/Tasks/Details?id={task.Id}";

            return await SaveMockMessage(user.PhoneNumber, message, taskId: task.Id, userId: user.Id);
        }

        public async Task<bool> SendVerificationCode(string phoneNumber, string code)
        {
            var message = $"🔐 *RAF System Verification*\n\n" +
                         $"Your verification code is: *{code}*\n" +
                         $"Valid for 10 minutes.\n\n" +
                         $"(Simulated - not actually sent)";

            return await SaveMockMessage(phoneNumber, message);
        }

        public async Task<bool> SendDeadlineReminder(User user, RafTask task)
        {
            if (!user.WhatsAppNotificationsEnabled || string.IsNullOrEmpty(user.PhoneNumber) || !task.DueDate.HasValue)
                return false;

            var daysUntilDue = (task.DueDate.Value - DateTime.Now).Days;
            var message = $"⏰ *Deadline Approaching*\n" +
                         $"Task: {task.Title}\n" +
                         $"Due in: {daysUntilDue} day(s)\n";

            if (task.CaseId.HasValue)
            {
                var rafCase = await _context.Cases.FindAsync(task.CaseId.Value);
                if (rafCase != null)
                {
                    message += $"Case: {rafCase.CaseNumber}\n";
                }
            }

            message += $"Action required!\n" +
                      $"View: https://localhost:5001/Tasks/Details?id={task.Id}";

            return await SaveMockMessage(user.PhoneNumber, message, taskId: task.Id, userId: user.Id);
        }

        public async Task<bool> SendCaseUpdate(User user, RafCase rafCase, string updateMessage)
        {
            if (!user.WhatsAppNotificationsEnabled || string.IsNullOrEmpty(user.PhoneNumber))
                return false;

            var message = $"📋 *Case Status Updated*\n" +
                         $"Case: {rafCase.CaseNumber}\n" +
                         $"{updateMessage}\n" +
                         $"View: https://localhost:5001/Cases/Details?id={rafCase.Id}";

            return await SaveMockMessage(user.PhoneNumber, message, caseId: rafCase.Id, userId: user.Id);
        }

        public async Task<List<WhatsAppMessage>> GetRecentMessagesAsync(int count = 50)
        {
            return await _context.WhatsAppMessages
                .OrderByDescending(m => m.CreatedDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<WhatsAppMessage>> GetMessagesByPhoneAsync(string phoneNumber)
        {
            return await _context.WhatsAppMessages
                .Where(m => m.ToPhoneNumber == phoneNumber)
                .OrderByDescending(m => m.CreatedDate)
                .ToListAsync();
        }

        private async Task<bool> SaveMockMessage(string to, string body, int? taskId = null, int? caseId = null, int? userId = null)
        {
            try
            {
                var whatsAppMessage = new WhatsAppMessage
                {
                    ToPhoneNumber = to,
                    MessageBody = body,
                    CreatedDate = DateTime.Now,
                    Status = WhatsAppMessageStatus.Sent,
                    DeliveredDate = DateTime.Now.AddSeconds(2), // Simulate 2 second delivery
                    TaskId = taskId,
                    CaseId = caseId,
                    UserId = userId,
                    IsSimulated = true
                };

                _context.WhatsAppMessages.Add(whatsAppMessage);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"[MOCK WHATSAPP] Message 'sent' to {to}: {body.Substring(0, Math.Min(50, body.Length))}...");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save mock WhatsApp message");
                return false;
            }
        }
    }

    // This will be implemented when you have Twilio credentials
    public class TwilioWhatsAppService : IWhatsAppService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TwilioWhatsAppService> _logger;
        private readonly IConfiguration _configuration;

        public TwilioWhatsAppService(
            ApplicationDbContext context,
            ILogger<TwilioWhatsAppService> logger,
            IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        public Task<bool> SendNotificationAsync(string phoneNumber, string message, int? taskId = null, int? caseId = null, int? userId = null)
        {
            throw new NotImplementedException("Twilio WhatsApp service not yet configured. Please add Twilio API keys to use this service.");
        }

        public Task<bool> SendTaskAssignment(User user, RafTask task)
        {
            throw new NotImplementedException("Twilio WhatsApp service not yet configured.");
        }

        public Task<bool> SendVerificationCode(string phoneNumber, string code)
        {
            throw new NotImplementedException("Twilio WhatsApp service not yet configured.");
        }

        public Task<bool> SendDeadlineReminder(User user, RafTask task)
        {
            throw new NotImplementedException("Twilio WhatsApp service not yet configured.");
        }

        public Task<bool> SendCaseUpdate(User user, RafCase rafCase, string updateMessage)
        {
            throw new NotImplementedException("Twilio WhatsApp service not yet configured.");
        }

        public Task<List<WhatsAppMessage>> GetRecentMessagesAsync(int count = 50)
        {
            throw new NotImplementedException();
        }

        public Task<List<WhatsAppMessage>> GetMessagesByPhoneAsync(string phoneNumber)
        {
            throw new NotImplementedException();
        }
    }
}
