using Microsoft.AspNetCore.Mvc;
using raf_pnp.Models;
using raf_pnp.Services;
using TaskStatus = raf_pnp.Models.TaskStatus;

namespace raf_pnp.Pages.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly INotificationService _notificationService;
        private readonly ILogger<TasksController> _logger;

        public TasksController(
            ITaskService taskService,
            INotificationService notificationService,
            ILogger<TasksController> logger)
        {
            _taskService = taskService;
            _notificationService = notificationService;
            _logger = logger;
        }

        // GET: api/tasks
        [HttpGet]
        public async Task<ActionResult<List<RafTask>>> GetTasks(
            [FromQuery] TaskStatus? status = null,
            [FromQuery] int? assignedToUserId = null,
            [FromQuery] int? teamId = null,
            [FromQuery] int? caseId = null)
        {
            var tasks = await _taskService.GetAllTasksAsync(status, assignedToUserId, teamId, caseId);
            return Ok(tasks);
        }

        // GET: api/tasks/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<RafTask>> GetTask(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
                return NotFound(new { message = "Task not found" });

            return Ok(task);
        }

        // GET: api/tasks/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<RafTask>>> GetUserTasks(int userId)
        {
            var tasks = await _taskService.GetUserTasksAsync(userId);
            return Ok(tasks);
        }

        // GET: api/tasks/overdue
        [HttpGet("overdue")]
        public async Task<ActionResult<List<RafTask>>> GetOverdueTasks()
        {
            var tasks = await _taskService.GetOverdueTasksAsync();
            return Ok(tasks);
        }

        // POST: api/tasks
        [HttpPost]
        public async Task<ActionResult<RafTask>> CreateTask([FromBody] CreateTaskRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var task = new RafTask
            {
                Title = request.Title,
                Description = request.Description,
                CaseId = request.CaseId,
                TeamId = request.TeamId,
                AssignedToUserId = request.AssignedToUserId,
                CreatedByUserId = request.CreatedByUserId,
                Priority = request.Priority,
                Status = TaskStatus.NotStarted,
                DueDate = request.DueDate
            };

            var createdTask = await _taskService.CreateTaskAsync(task);

            // Send notification if assigned
            if (createdTask.AssignedToUserId.HasValue)
            {
                await _notificationService.NotifyTaskAssignedAsync(createdTask);
            }

            return CreatedAtAction(nameof(GetTask), new { id = createdTask.Id }, createdTask);
        }

        // PUT: api/tasks/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<RafTask>> UpdateTask(int id, [FromBody] UpdateTaskRequest request)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
                return NotFound(new { message = "Task not found" });

            task.Title = request.Title ?? task.Title;
            task.Description = request.Description ?? task.Description;
            task.Priority = request.Priority ?? task.Priority;
            task.DueDate = request.DueDate ?? task.DueDate;

            var updatedTask = await _taskService.UpdateTaskAsync(task);
            return Ok(updatedTask);
        }

        // DELETE: api/tasks/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var result = await _taskService.DeleteTaskAsync(id);
            if (!result)
                return NotFound(new { message = "Task not found" });

            return NoContent();
        }

        // POST: api/tasks/{id}/assign
        [HttpPost("{id}/assign")]
        public async Task<IActionResult> AssignTask(int id, [FromBody] AssignTaskRequest request)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
                return NotFound(new { message = "Task not found" });

            var result = await _taskService.AssignTaskAsync(id, request.UserId);
            if (!result)
                return BadRequest(new { message = "Failed to assign task" });

            // Send notification
            var updatedTask = await _taskService.GetTaskByIdAsync(id);
            if (updatedTask != null)
            {
                await _notificationService.NotifyTaskAssignedAsync(updatedTask);
            }

            return Ok(new { message = "Task assigned successfully" });
        }

        // PUT: api/tasks/{id}/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] UpdateTaskStatusRequest request)
        {
            var result = await _taskService.UpdateTaskStatusAsync(id, request.Status);
            if (!result)
                return NotFound(new { message = "Task not found" });

            return Ok(new { message = "Task status updated successfully" });
        }

        // POST: api/tasks/{id}/comments
        [HttpPost("{id}/comments")]
        public async Task<IActionResult> AddComment(int id, [FromBody] AddCommentRequest request)
        {
            var result = await _taskService.AddCommentAsync(id, request.UserId, request.Comment);
            if (!result)
                return NotFound(new { message = "Task not found" });

            return Ok(new { message = "Comment added successfully" });
        }

        // GET: api/tasks/{id}/comments
        [HttpGet("{id}/comments")]
        public async Task<ActionResult<List<TaskComment>>> GetTaskComments(int id)
        {
            var comments = await _taskService.GetTaskCommentsAsync(id);
            return Ok(comments);
        }
    }

    // Request models
    public class CreateTaskRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? CaseId { get; set; }
        public int? TeamId { get; set; }
        public int? AssignedToUserId { get; set; }
        public int CreatedByUserId { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime? DueDate { get; set; }
    }

    public class UpdateTaskRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public TaskPriority? Priority { get; set; }
        public DateTime? DueDate { get; set; }
    }

    public class AssignTaskRequest
    {
        public int UserId { get; set; }
    }

    public class UpdateTaskStatusRequest
    {
        public TaskStatus Status { get; set; }
    }

    public class AddCommentRequest
    {
        public int UserId { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
