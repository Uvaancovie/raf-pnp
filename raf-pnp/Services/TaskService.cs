using Microsoft.EntityFrameworkCore;
using raf_pnp.Data;
using raf_pnp.Models;
using TaskStatus = raf_pnp.Models.TaskStatus;

namespace raf_pnp.Services
{
    public interface ITaskService
    {
        Task<RafTask?> GetTaskByIdAsync(int id);
        Task<List<RafTask>> GetAllTasksAsync(TaskStatus? status = null, int? assignedToUserId = null, int? teamId = null, int? caseId = null);
        Task<List<RafTask>> GetUserTasksAsync(int userId);
        Task<List<RafTask>> GetOverdueTasksAsync();
        Task<RafTask> CreateTaskAsync(RafTask task);
        Task<RafTask> UpdateTaskAsync(RafTask task);
        Task<bool> DeleteTaskAsync(int id);
        Task<bool> AssignTaskAsync(int taskId, int userId);
        Task<bool> UpdateTaskStatusAsync(int taskId, TaskStatus newStatus);
        Task<bool> AddCommentAsync(int taskId, int userId, string comment);
        Task<List<TaskComment>> GetTaskCommentsAsync(int taskId);
        Task<RafTask> CreateWorkflowTaskAsync(int caseId, CaseStatus caseStatus, string title, string description, int? assignToUserId = null);
    }

    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TaskService> _logger;

        public TaskService(ApplicationDbContext context, ILogger<TaskService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<RafTask?> GetTaskByIdAsync(int id)
        {
            return await _context.Tasks
                .Include(t => t.AssignedToUser)
                .Include(t => t.CreatedByUser)
                .Include(t => t.Team)
                .Include(t => t.Case)
                .Include(t => t.Comments)
                    .ThenInclude(c => c.User)
                .Include(t => t.Attachments)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<RafTask>> GetAllTasksAsync(TaskStatus? status = null, int? assignedToUserId = null, int? teamId = null, int? caseId = null)
        {
            var query = _context.Tasks
                .Include(t => t.AssignedToUser)
                .Include(t => t.CreatedByUser)
                .Include(t => t.Team)
                .Include(t => t.Case)
                .AsQueryable();

            if (status.HasValue)
                query = query.Where(t => t.Status == status.Value);

            if (assignedToUserId.HasValue)
                query = query.Where(t => t.AssignedToUserId == assignedToUserId.Value);

            if (teamId.HasValue)
                query = query.Where(t => t.TeamId == teamId.Value);

            if (caseId.HasValue)
                query = query.Where(t => t.CaseId == caseId.Value);

            return await query.OrderByDescending(t => t.Priority)
                             .ThenBy(t => t.DueDate)
                             .ToListAsync();
        }

        public async Task<List<RafTask>> GetUserTasksAsync(int userId)
        {
            return await _context.Tasks
                .Include(t => t.Case)
                .Include(t => t.Team)
                .Where(t => t.AssignedToUserId == userId &&
                           t.Status != TaskStatus.Completed &&
                           t.Status != TaskStatus.Cancelled)
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.DueDate)
                .ToListAsync();
        }

        public async Task<List<RafTask>> GetOverdueTasksAsync()
        {
            return await _context.Tasks
                .Include(t => t.AssignedToUser)
                .Include(t => t.Case)
                .Where(t => t.DueDate.HasValue &&
                           t.DueDate.Value < DateTime.Now &&
                           t.Status != TaskStatus.Completed &&
                           t.Status != TaskStatus.Cancelled)
                .OrderBy(t => t.DueDate)
                .ToListAsync();
        }

        public async Task<RafTask> CreateTaskAsync(RafTask task)
        {
            task.CreatedDate = DateTime.Now;

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Task created: {task.Title} (ID: {task.Id})");
            return task;
        }

        public async Task<RafTask> UpdateTaskAsync(RafTask task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Task updated: {task.Title} (ID: {task.Id})");
            return task;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return false;

            // Mark as cancelled instead of deleting
            task.Status = TaskStatus.Cancelled;
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Task cancelled: {task.Title} (ID: {task.Id})");
            return true;
        }

        public async Task<bool> AssignTaskAsync(int taskId, int userId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
                return false;

            task.AssignedToUserId = userId;
            if (task.Status == TaskStatus.NotStarted)
            {
                task.Status = TaskStatus.InProgress;
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Task {taskId} assigned to user {userId}");
            return true;
        }

        public async Task<bool> UpdateTaskStatusAsync(int taskId, TaskStatus newStatus)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
                return false;

            task.Status = newStatus;
            if (newStatus == TaskStatus.Completed)
            {
                task.CompletedDate = DateTime.Now;
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Task {taskId} status updated to {newStatus}");
            return true;
        }

        public async Task<bool> AddCommentAsync(int taskId, int userId, string comment)
        {
            var taskComment = new TaskComment
            {
                TaskId = taskId,
                UserId = userId,
                Comment = comment,
                CreatedDate = DateTime.Now
            };

            _context.TaskComments.Add(taskComment);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Comment added to task {taskId} by user {userId}");
            return true;
        }

        public async Task<List<TaskComment>> GetTaskCommentsAsync(int taskId)
        {
            return await _context.TaskComments
                .Include(c => c.User)
                .Where(c => c.TaskId == taskId)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
        }

        public async Task<RafTask> CreateWorkflowTaskAsync(int caseId, CaseStatus caseStatus, string title, string description, int? assignToUserId = null)
        {
            var rafCase = await _context.Cases
                .Include(c => c.AssignedTeam)
                .FirstOrDefaultAsync(c => c.Id == caseId);

            if (rafCase == null)
                throw new ArgumentException($"Case {caseId} not found");

            // Determine priority based on case status
            var priority = caseStatus switch
            {
                CaseStatus.ClientIntake => TaskPriority.High,
                CaseStatus.SummonsIssued => TaskPriority.Urgent,
                CaseStatus.StatutoryWaitingPeriod => TaskPriority.Medium,
                _ => TaskPriority.Medium
            };

            // Calculate due date based on case status
            DateTime? dueDate = caseStatus switch
            {
                CaseStatus.ClientIntake => DateTime.Now.AddDays(7),
                CaseStatus.InitialLodgement => DateTime.Now.AddDays(14),
                CaseStatus.ExpertAppointments => DateTime.Now.AddDays(30),
                CaseStatus.ComplianceLodgement => DateTime.Now.AddDays(14),
                CaseStatus.SummonsIssued => DateTime.Now.AddDays(10),
                _ => DateTime.Now.AddDays(30)
            };

            var task = new RafTask
            {
                Title = title,
                Description = description,
                CaseId = caseId,
                TeamId = rafCase.AssignedTeamId,
                AssignedToUserId = assignToUserId,
                CreatedByUserId = 1, // System user - adjust as needed
                Priority = priority,
                Status = TaskStatus.NotStarted,
                IsWorkflowTask = true,
                RelatedCaseStatus = caseStatus,
                DueDate = dueDate,
                CreatedDate = DateTime.Now
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Workflow task created for case {caseId}: {title}");
            return task;
        }
    }
}
