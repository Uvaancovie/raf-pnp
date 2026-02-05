# RAF Task and Team Management System - Implementation Plan

## Overview

This document outlines the implementation plan for adding comprehensive task and team management capabilities to the RAF operations application, aligned with the RAF process stages and case management workflow.

---

## User Review Required

> [!IMPORTANT]
> **Key Design Decisions:**
> - Team-based structure with hierarchical roles (Admin, Team Lead, Member)
> - Tasks linked to RAF case stages for automatic workflow progression
> - Real-time notification system using SignalR
> - Calendar integration for deadlines and court dates
> - Task templates based on RAF case types and stages

> [!WARNING]
> **Breaking Changes:**
> - Existing Case model will need new navigation properties
> - User model will be extended with team relationships and WhatsApp phone numbers
> - New database tables will be created
> - Twilio API integration requires configuration and API keys (see appsettings)
> - Phone number verification flow will be added to user profile

---

## Proposed Changes

### Component 1: Core Models and Database

#### [NEW] [Team.cs](file:///d:/raf-pnp/raf-pnp/Models/Team.cs)

```csharp
public class Team
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int? LeadUserId { get; set; }
    public User? LeadUser { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
    
    // Navigation properties
    public ICollection<TeamMember> TeamMembers { get; set; }
    public ICollection<Task> Tasks { get; set; }
    public ICollection<Case> AssignedCases { get; set; }
}

public class TeamMember
{
    public int Id { get; set; }
    public int TeamId { get; set; }
    public Team Team { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public TeamRole Role { get; set; }
    public DateTime JoinedDate { get; set; }
    public bool IsActive { get; set; }
}

public enum TeamRole
{
    Member,
    Lead,
    Admin
}
```

#### [NEW] [Task.cs](file:///d:/raf-pnp/raf-pnp/Models/Task.cs)

```csharp
public class Task
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    
    // Case relationship
    public int? CaseId { get; set; }
    public Case? Case { get; set; }
    
    // Team relationship
    public int? TeamId { get; set; }
    public Team? Team { get; set; }
    
    // Assignment
    public int? AssignedToUserId { get; set; }
    public User? AssignedToUser { get; set; }
    public int CreatedByUserId { get; set; }
    public User CreatedByUser { get; set; }
    
    // Status and priority
    public TaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    
    // RAF workflow integration
    public CaseStatus? RelatedCaseStatus { get; set; }
    public bool IsWorkflowTask { get; set; }
    
    // Dates
    public DateTime CreatedDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    
    // Navigation
    public ICollection<TaskComment> Comments { get; set; }
    public ICollection<TaskAttachment> Attachments { get; set; }
    public ICollection<TaskNotification> Notifications { get; set; }
}

public enum TaskStatus
{
    NotStarted,
    InProgress,
    Blocked,
    UnderReview,
    Completed,
    Cancelled
}

public enum TaskPriority
{
    Low,
    Medium,
    High,
    Urgent
}
```

#### [NEW] [Notification.cs](file:///d:/raf-pnp/raf-pnp/Models/Notification.cs)

```csharp
public class Notification
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    
    public string Title { get; set; }
    public string Message { get; set; }
    public NotificationType Type { get; set; }
    
    // Reference to related entity
    public int? TaskId { get; set; }
    public Task? Task { get; set; }
    public int? CaseId { get; set; }
    public Case? Case { get; set; }
    
    public DateTime CreatedDate { get; set; }
    public bool IsRead { get; set; }
    public DateTime? ReadDate { get; set; }
    
    // For action items
    public string? ActionUrl { get; set; }
    public bool RequiresAction { get; set; }
}

public enum NotificationType
{
    TaskAssigned,
    TaskDueSoon,
    TaskOverdue,
    TaskCompleted,
    TaskCommented,
    CaseStatusChanged,
    DeadlineApproaching,
    TeamUpdate,
    SystemUpdate
}

public enum NotificationChannel
{
    InApp,
    Email,
    WhatsApp,
    All
}
```

#### [MODIFY] [User.cs](file:///d:/raf-pnp/raf-pnp/Models/User.cs)

Add WhatsApp notification support:

```csharp
public string? PhoneNumber { get; set; }
public bool WhatsAppNotificationsEnabled { get; set; }
public bool PhoneNumberVerified { get; set; }
public NotificationChannel PreferredNotificationChannel { get; set; }
```

#### [MODIFY] [Case.cs](file:///d:/raf-pnp/raf-pnp/Models/Case.cs)

Add team and task relationships:

```csharp
public int? AssignedTeamId { get; set; }
public Team? AssignedTeam { get; set; }
public ICollection<Task> Tasks { get; set; }
```

---

### Component 2: Services Layer

#### [NEW] [TeamService.cs](file:///d:/raf-pnp/raf-pnp/Services/TeamService.cs)

Handles all team-related operations:
- Create/read/update/delete teams
- Manage team members (add, remove, update roles)
- Get team statistics and performance metrics
- Team assignment to cases

#### [NEW] [TaskService.cs](file:///d:/raf-pnp/raf-pnp/Services/TaskService.cs)

Manages task operations:
- CRUD operations for tasks
- Task assignment and reassignment
- Task status updates with workflow validation
- Automatic task creation based on case status changes
- Task templates for different RAF case types
- Bulk task operations

#### [NEW] [NotificationService.cs](file:///d:/raf-pnp/raf-pnp/Services/NotificationService.cs)

Notification management:
- Create and send notifications
- Real-time push notifications via SignalR
- Email notifications for important events
- Notification preferences management
- Mark as read/unread
- Batch notification operations

#### [NEW] [CalendarService.cs](file:///d:/raf-pnp/raf-pnp/Services/CalendarService.cs)

Calendar integration:
- Sync tasks with calendar
- Court date management
- Deadline tracking
- iCal export functionality
- Calendar view data preparation

#### [NEW] [WhatsAppService.cs](file:///d:/raf-pnp/raf-pnp/Services/WhatsAppService.cs)

WhatsApp integration using Twilio API:
- Send WhatsApp messages for task assignments
- Send deadline reminders
- Send case status updates
- Phone number verification via OTP
- Message template management
- Delivery status tracking
- Batch message sending
- Rate limiting and queue management

---

### Component 3: API Controllers

#### [NEW] [TeamsController.cs](file:///d:/raf-pnp/raf-pnp/Pages/Api/TeamsController.cs)

RESTful endpoints:
- `GET /api/teams` - List all teams
- `GET /api/teams/{id}` - Get team details
- `POST /api/teams` - Create new team
- `PUT /api/teams/{id}` - Update team
- `DELETE /api/teams/{id}` - Delete team
- `POST /api/teams/{id}/members` - Add team member
- `DELETE /api/teams/{id}/members/{userId}` - Remove member

#### [NEW] [TasksController.cs](file:///d:/raf-pnp/raf-pnp/Pages/Api/TasksController.cs)

Task management endpoints:
- `GET /api/tasks` - List tasks with filtering
- `GET /api/tasks/{id}` - Get task details
- `POST /api/tasks` - Create task
- `PUT /api/tasks/{id}` - Update task
- `DELETE /api/tasks/{id}` - Delete task
- `POST /api/tasks/{id}/assign` - Assign task
- `PUT /api/tasks/{id}/status` - Update status
- `POST /api/tasks/{id}/comments` - Add comment

#### [NEW] [NotificationsController.cs](file:///d:/raf-pnp/raf-pnp/Pages/Api/NotificationsController.cs)

Notification endpoints:
- `GET /api/notifications` - Get user notifications
- `PUT /api/notifications/{id}/read` - Mark as read
- `PUT /api/notifications/read-all` - Mark all as read
- `DELETE /api/notifications/{id}` - Delete notification

---

### Component 4: Razor Pages (Frontend)

#### [NEW] [Teams/Index.cshtml](file:///d:/raf-pnp/raf-pnp/Pages/Teams/Index.cshtml)

Team management dashboard showing:
- List of all teams
- Team member counts
- Active cases per team
- Performance metrics
- Create team button

#### [NEW] [Teams/Details.cshtml](file:///d:/raf-pnp/raf-pnp/Pages/Teams/Details.cshtml)

Detailed team view:
- Team information
- Member list with roles
- Assigned cases
- Team tasks
- Performance charts

#### [NEW] [Tasks/Board.cshtml](file:///d:/raf-pnp/raf-pnp/Pages/Tasks/Board.cshtml)

Kanban-style task board:
- Columns for each task status
- Drag-and-drop functionality
- Filter by team, case, assignee
- Quick task creation
- Task cards with key info

#### [NEW] [Tasks/Calendar.cshtml](file:///d:/raf-pnp/raf-pnp/Pages/Tasks/Calendar.cshtml)

Calendar view:
- Monthly/weekly/daily views
- Tasks displayed on due dates
- Court dates from cases
- Deadline indicators
- Click to view/edit tasks

#### [NEW] [Notifications/Index.cshtml](file:///d:/raf-pnp/raf-pnp/Pages/Notifications/Index.cshtml)

Notification center:
- List of notifications
- Filter by type
- Mark as read/unread
- Delete notifications
- Real-time updates

---

### Component 5: Real-time Communication

#### [NEW] [NotificationHub.cs](file:///d:/raf-pnp/raf-pnp/Hubs/NotificationHub.cs)

SignalR hub for real-time notifications:

```csharp
public class NotificationHub : Hub
{
    public async Task SendNotification(int userId, Notification notification)
    {
        await Clients.User(userId.ToString()).SendAsync("ReceiveNotification", notification);
    }
    
    public async Task NotifyTeam(int teamId, string message)
    {
        await Clients.Group($"team_{teamId}").SendAsync("ReceiveTeamUpdate", message);
    }
}
```

---

### Component 6: Workflow Automation

#### [NEW] [WorkflowService.cs](file:///d:/raf-pnp/raf-pnp/Services/WorkflowService.cs)

Automated workflow management:
- Trigger tasks when case status changes
- Auto-assign tasks based on team roles
- Send notifications for workflow events
- Track case progress through RAF stages
- Generate compliance reminders

**Example workflow triggers:**
- Case status → ClientIntake: Create "Gather client documents" task
- Case status → MMIWaiting: Create reminder task for 12-month follow-up
- Case status → ExpertAppointments: Create tasks for each required expert
- Case status → Compliance: Create document preparation tasks

---

## WhatsApp Integration Details

### Configuration Requirements

**Twilio Setup:**

```json
{
  "Twilio": {
    "AccountSid": "your_account_sid",
    "AuthToken": "your_auth_token",
    "WhatsAppNumber": "whatsapp:+14155238886",
    "StatusCallbackUrl": "https://yourapp.com/api/whatsapp/status"
  }
}
```

**NuGet Packages:**
- `Twilio` - Official Twilio SDK for .NET

### WhatsApp Features

**1. Phone Number Management:**
- Users add phone number in profile settings
- OTP verification via WhatsApp
- Country code validation
- Format: E.164 standard (+27XXXXXXXXX for South Africa)

**2. Notification Templates:**

*Task Assignment:*
```
🎯 *New Task Assigned*
Task: {TaskTitle}
Priority: {Priority}
Due: {DueDate}
View: {TaskUrl}
```

*Deadline Reminder:*
```
⏰ *Deadline Approaching*
Task: {TaskTitle}
Due in: {TimeRemaining}
Case: {CaseNumber}
Action required!
```

*Case Update:*
```
📋 *Case Status Updated*
Case: {CaseNumber}
Status: {OldStatus} → {NewStatus}
Assigned Team: {TeamName}
```

**3. User Preferences:**
- Enable/disable WhatsApp notifications
- Choose notification types (tasks only, all updates, urgent only)
- Quiet hours configuration
- Frequency limits (max messages per day)

**4. Implementation Checklist:**
- [ ] Add Twilio NuGet package
- [ ] Configure Twilio credentials in appsettings
- [ ] Create WhatsAppService
- [ ] Add phone number fields to User model
- [ ] Implement OTP verification
- [ ] Create message templates
- [ ] Add notification channel selector to NotificationService
- [ ] Build phone number management UI
- [ ] Test message delivery
- [ ] Implement delivery status tracking
- [ ] Set up error handling and retries
- [ ] Add usage monitoring

### Sample WhatsAppService Implementation

```csharp
public class WhatsAppService
{
    private readonly TwilioRestClient _twilioClient;
    private readonly string _whatsAppNumber;
    
    public async Task<bool> SendTaskAssignment(User user, Task task)
    {
        if (!user.WhatsAppNotificationsEnabled || string.IsNullOrEmpty(user.PhoneNumber))
            return false;
            
        var message = $"🎯 *New Task Assigned*\n" +
                     $"Task: {task.Title}\n" +
                     $"Priority: {task.Priority}\n" +
                     $"Due: {task.DueDate:MMM dd, yyyy}\n" +
                     $"View: https://yourapp.com/tasks/{task.Id}";
        
        return await SendWhatsAppMessage(user.PhoneNumber, message);
    }
    
    public async Task<bool> SendVerificationCode(string phoneNumber, string code)
    {
        var message = $"Your verification code is: *{code}*\n" +
                     $"Valid for 10 minutes.";
        
        return await SendWhatsAppMessage(phoneNumber, message);
    }
    
    private async Task<bool> SendWhatsAppMessage(string to, string body)
    {
        try
        {
            var message = await MessageResource.CreateAsync(
                body: body,
                from: new PhoneNumber(_whatsAppNumber),
                to: new PhoneNumber($"whatsapp:{to}")
            );
            
            return message.Status != MessageResource.StatusEnum.Failed;
        }
        catch (Exception ex)
        {
            // Log error
            return false;
        }
    }
}
```

### Offline WhatsApp Simulator (For Local Development)

**Purpose:** Develop and test WhatsApp features without Twilio API keys.

**Features:**
- Store messages in local database instead of sending
- View all "sent" messages in debug UI
- Simulate delivery status (pending → sent → delivered → read)
- Test notification templates
- Verify phone number validation logic

**Implementation:**

#### [NEW] [WhatsAppMessage.cs](file:///d:/raf-pnp/raf-pnp/Models/WhatsAppMessage.cs)

```csharp
public class WhatsAppMessage
{
    public int Id { get; set; }
    public string ToPhoneNumber { get; set; }
    public string MessageBody { get; set; }
    public DateTime CreatedDate { get; set; }
    public WhatsAppMessageStatus Status { get; set; }
    public DateTime? DeliveredDate { get; set; }
    public DateTime? ReadDate { get; set; }
    
    // Reference to related entities
    public int? TaskId { get; set; }
    public int? CaseId { get; set; }
    public int? UserId { get; set; }
    
    public bool IsSimulated { get; set; } // True when using offline mode
}

public enum WhatsAppMessageStatus
{
    Queued,
    Sent,
    Delivered,
    Read,
    Failed
}
```

#### [NEW] [MockWhatsAppService.cs](file:///d:/raf-pnp/raf-pnp/Services/MockWhatsAppService.cs)

```csharp
public class MockWhatsAppService : IWhatsAppService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<MockWhatsAppService> _logger;
    
    public MockWhatsAppService(ApplicationDbContext context, ILogger<MockWhatsAppService> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<bool> SendTaskAssignment(User user, Task task)
    {
        if (!user.WhatsAppNotificationsEnabled || string.IsNullOrEmpty(user.PhoneNumber))
            return false;
            
        var message = $"🎯 *New Task Assigned*\n" +
                     $"Task: {task.Title}\n" +
                     $"Priority: {task.Priority}\n" +
                     $"Due: {task.DueDate:MMM dd, yyyy}\n" +
                     $"View: https://localhost:5001/tasks/{task.Id}";
        
        return await SaveMockMessage(user.PhoneNumber, message, taskId: task.Id, userId: user.Id);
    }
    
    public async Task<bool> SendVerificationCode(string phoneNumber, string code)
    {
        var message = $"Your RAF System verification code is: *{code}*\n" +
                     $"Valid for 10 minutes.\n" +
                     $"(Simulated - not actually sent)";
        
        return await SaveMockMessage(phoneNumber, message);
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
```

#### [NEW] [WhatsAppDebug.cshtml](file:///d:/raf-pnp/raf-pnp/Pages/Admin/WhatsAppDebug.cshtml)

Debug page to view all simulated messages:
- List of all WhatsApp messages in database
- Filter by phone number, date, status
- View message content
- Simulate status changes (sent → delivered → read)
- Clear all test messages
- Export messages to JSON/CSV

**Configuration:**

```csharp
// In Program.cs - switch between real and mock service
if (builder.Configuration.GetValue<bool>("UseWhatsAppSimulator", true))
{
    builder.Services.AddScoped<IWhatsAppService, MockWhatsAppService>();
}
else
{
    builder.Services.AddScoped<IWhatsAppService, TwilioWhatsAppService>();
}
```

```json
// In appsettings.Development.json
{
  "UseWhatsAppSimulator": true,
  "Twilio": {
    "AccountSid": "not_configured",
    "AuthToken": "not_configured",
    "WhatsAppNumber": "whatsapp:+14155238886"
  }
}
```

**Benefits:**
- ✅ Develop without API costs
- ✅ Test notification templates
- ✅ Debug message content and timing
- ✅ Demo system to stakeholders
- ✅ Easy switch to production (change one config setting)

**Migration Path:**
1. Develop with mock service
2. Obtain Twilio credentials
3. Test with Twilio sandbox
4. Switch to production by setting `UseWhatsAppSimulator: false`

---

## Verification Plan

### Automated Tests

**Unit Tests:**
- Service layer logic (TeamService, TaskService, NotificationService)
- Model validation
- Workflow automation logic
- Calendar calculations

**Integration Tests:**
- API endpoints
- Database operations
- SignalR communication
- Email notifications

### Manual Verification

**Admin Features:**
1. Create and manage teams
2. Assign team leads and members
3. View team performance metrics
4. Bulk task operations

**Team Lead Features:**
1. Assign tasks to team members
2. Monitor team workload
3. Update task priorities
4. View team calendar

**Team Member Features:**
1. View assigned tasks
2. Update task status
3. Add comments and attachments
4. Receive notifications
5. View personal calendar

**Workflow Testing:**
1. Create case and verify automatic task creation
2. Update case status and verify workflow triggers
3. Test notification delivery
4. Verify deadline calculations

**Calendar Integration:**
1. Task due dates appear on calendar
2. Court dates from cases sync correctly
3. iCal export functionality
4. Reminder notifications

---

## Implementation Phases

### Phase 1: Database and Models (Week 1)
- Create all model classes
- Generate migrations
- Update existing models
- Seed initial data

### Phase 2: Core Services (Week 2)
- Implement TeamService
- Implement TaskService
- Implement NotificationService
- Add unit tests

### Phase 3: API Layer (Week 3)
- Create API controllers
- Implement authentication/authorization
- Add API documentation
- Integration tests

### Phase 4: Frontend - Teams (Week 4)
- Team management pages
- Team member management
- Team assignment to cases

### Phase 5: Frontend - Tasks (Week 5)
- Task board/kanban view
- Task creation and editing
- Task assignment interface
- Comments and attachments

### Phase 6: Frontend - Notifications (Week 6)
- Notification center
- Real-time SignalR integration
- Email notification templates
- WhatsApp notification setup
- Phone number verification UI
- Notification preferences (Email/WhatsApp/In-App)

### Phase 7: Calendar and Workflow (Week 7)
- Calendar views
- Workflow automation
- Task templates
- Deadline management

### Phase 8: Testing and Refinement (Week 8)
- User acceptance testing
- Bug fixes
- Performance optimization
- Documentation

---

## Technical Considerations

**Performance:**
- Index frequently queried columns (TeamId, AssignedToUserId, Status, DueDate)
- Implement caching for team and user lookups
- Use pagination for task lists
- Optimize SignalR connection management

**Security:**
- Role-based access control for team management
- Task visibility based on team membership
- Audit logging for sensitive operations
- Secure notification delivery

**Scalability:**
- Support for thousands of tasks
- Efficient notification queries
- Background job processing for bulk operations
- Database partitioning for historical data

**Integration:**
- Extend existing RAF case workflow
- Maintain backward compatibility
- Support existing user roles
- Integrate with current reporting system