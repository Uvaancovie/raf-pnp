# RAF PNP - Road Accident Fund Case Management System

## 📋 Project Overview

A comprehensive ASP.NET Core Razor Pages application for managing Road Accident Fund (RAF) cases at Pather & Pather Attorneys. This system streamlines the entire RAF claims process from client intake through final settlement, providing attorneys with a complete case management solution.

## 🎯 Business Context

### Road Accident Fund (RAF) Process
The RAF process in South Africa involves 13 distinct stages for personal injury claims:

1. **Client Intake** - Initial client consultation and case registration
2. **Initial Lodgement** - Submission of claim to RAF
3. **MMI Waiting** - Waiting for Maximum Medical Improvement assessment
4. **Expert Appointments** - Medical and actuarial expert consultations
5. **Compliance** - RAF compliance requirements and documentation
6. **120-Day Wait** - Mandatory waiting period after compliance
7. **PAJA Consideration** - Promotion of Administrative Justice Act reviews
8. **Pleading** - Legal pleading preparation
9. **Pre-Trial** - Pre-trial preparations and negotiations
10. **Trial** - Court proceedings
11. **Finalised** - Case completion and settlement

### Pather & Pather Attorneys
- South African law firm specializing in RAF claims
- Brand colors: Primary #006a4e (green), Secondary white
- Focus on personal injury and motor vehicle accident claims

## 🛠 Technical Stack

- **Framework**: .NET 8.0
- **UI Framework**: ASP.NET Core Razor Pages
- **Database**: SQL Server with Entity Framework Core 8.0.12
- **Frontend**: Bootstrap 5.3.3, Bootstrap Icons, Custom CSS
- **Error Monitoring**: Sentry.AspNetCore 4.12.0
- **Architecture**: MVC with Repository pattern

## 🏗 Architecture

### Application Structure
```
raf-pnp/
├── Pages/                 # Razor Pages (MVC Views + Controllers)
│   ├── Index.cshtml/cs    # Dashboard with statistics
│   ├── Cases/             # Case management CRUD
│   ├── Clients/           # Client management CRUD
│   ├── Reports/           # Analytics and reporting
│   └── Shared/            # Layouts and partials
├── Data/                  # Data access layer
│   ├── ApplicationDbContext.cs
│   └── DatabaseSeeder.cs
├── Models/                # Entity models
├── wwwroot/               # Static assets
│   ├── css/site.css       # Custom styling
│   ├── js/                # JavaScript files
│   └── lib/               # Third-party libraries
├── Program.cs             # Application entry point
├── appsettings.json       # Configuration
└── raf-pnp.csproj        # Project file
```

### Database Schema

#### Core Entities

**Client**
- Personal information (Name, ID Number, Contact details)
- South African ID validation (13-digit format)
- One-to-many relationship with Cases

**RafCase**
- Case metadata (Case Number, Accident details, Status)
- Financial data (Estimated Claim Value, Settlement Amount)
- Key dates (Accident Date, MMI Date, Compliance Date, etc.)
- Status tracking through 13 RAF process stages
- Foreign key to Client

**CaseDocument**
- Document management (Name, Type, File Path)
- Upload tracking (Date, User)
- Linked to specific cases

**ExpertAppointment**
- Medical expert scheduling (Neurologists, Psychologists, Actuaries)
- Appointment tracking (Date, Status, Fees)
- Report management

**CaseActivity**
- Audit trail for case activities
- Activity types and descriptions
- Reminder system integration

#### Enums

**CaseStatus** (13 stages)
- ClientIntake, InitialLodgement, MmiWaiting, ExpertAppointments, Compliance, OneTwentyDayWait, PajaConsideration, Pleading, PreTrial, Trial, Finalised

**DocumentType**
- AccidentReport, PoliceStatement, MedicalReport, ExpertReport, RafForm, SettlementAgreement

**ExpertType**
- Neurosurgeon, Neurologist, Psychologist, Psychiatrist, OrthopaedicSurgeon, Actuary, OccupationalTherapist

**AppointmentStatus**
- Scheduled, Completed, Cancelled, NoShow

**ActivityType**
- PhoneCall, Email, Meeting, DocumentUpload, StatusChange, DeadlineReminder

## 🎨 Design & Branding

### Color Scheme
- **Primary**: #006a4e (Pather Green)
- **Secondary**: #ffffff (White)
- **Accent**: Bootstrap default colors for status indicators

### UI Components
- Bootstrap 5 cards, tables, and forms
- Custom status badges with color coding
- Progress bars for RAF process visualization
- Responsive design for desktop and mobile

## 📊 Key Features

### Dashboard (Index)
- Case statistics by status
- Urgent cases requiring attention
- RAF process flow visualization
- Recent activity feed

### Case Management
- **Create**: New case intake with client selection
- **Read**: Detailed case view with all related data
- **Update**: Status progression through RAF workflow
- **Delete**: Case removal (with cascade considerations)

### Client Management
- Full CRUD operations for client records
- South African ID number validation
- Case history viewing

### Reports & Analytics
- Cases by status distribution
- Deadline tracking (MMI dates, statutory expiry)
- Settlement value analysis
- Monthly/yearly statistics

### Data Seeding
- 10 realistic South African clients
- 11 sample cases across all RAF stages
- Expert appointments with local medical professionals
- Document and activity history
- Configurable via `DatabaseSeeder.cs`

## 🔧 Configuration

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=LAPTOP-A5DOSJNT;Initial Catalog=raf-pnp;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### Program.cs Setup
```csharp
// Services registration
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Sentry error monitoring
builder.WebHost.UseSentry(o =>
{
    o.Dsn = "https://b2b496a68fb691efedf3ddaae5d7aea5@o4510794040672256.ingest.us.sentry.io/4510794042114048";
    o.Debug = true; // Set to false in production
});

// Database seeding
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await DatabaseSeeder.SeedAsync(context);
}
```

## 🚀 Development Setup

### Prerequisites
- .NET 8.0 SDK
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code

### Installation Steps
1. Clone the repository
2. Update connection string in `appsettings.json`
3. Run database migrations:
   ```bash
   dotnet ef database update
   ```
4. Build and run:
   ```bash
   dotnet build
   dotnet run
   ```
5. Access at `http://localhost:5062`

### Database Setup
- Entity Framework migrations included
- Automatic seeding on application startup
- Sample data for demonstration purposes

## 📈 Sample Data Structure

### Clients (10 records)
- Realistic South African names and ID numbers
- Diverse contact information
- Geographic distribution across major cities

### Cases (11 records)
- **Finalised**: 1 case (R2.1M settlement)
- **Trial**: 1 case (active litigation)
- **Pre-Trial**: 1 case (preparation phase)
- **Pleading**: 1 case (legal documentation)
- **PAJA**: 1 case (administrative review)
- **120-Day Wait**: 1 case (waiting period)
- **Compliance**: 1 case (RAF requirements)
- **Expert Appointments**: 1 case (medical assessments)
- **MMI Waiting**: 1 case (medical improvement)
- **Initial Lodgement**: 1 case (recent submission)
- **Client Intake**: 1 case (new client)

### Expert Appointments (9 records)
- Medical specialists across major South African cities
- Realistic fee structures
- Various appointment statuses

### Documents (16 records)
- Accident reports, police statements, medical records
- RAF forms and settlement agreements
- Upload tracking with timestamps

## 🔍 Error Monitoring

### Sentry Integration
- Automatic error capture and reporting
- Performance monitoring
- Release tracking
- User feedback collection

### Configuration
- DSN configured in Program.cs
- Debug mode for development
- Production-ready error aggregation

## 📝 Development Notes

### Code Organization
- Clean separation of concerns (Models, Data, Pages)
- Repository pattern for data access
- Bootstrap for consistent UI
- Custom CSS for branding

### Database Design
- Normalized schema with proper relationships
- Cascade delete restrictions for data integrity
- Computed properties for business logic
- Enum usage for type safety

### Security Considerations
- Input validation on all forms
- SQL injection prevention via EF Core
- XSS protection via Razor encoding
- HTTPS enforcement in production

### Performance Optimizations
- EF Core query optimization
- Lazy loading configuration
- Static file caching
- Database indexing on key fields

## 🤝 Contributing

This project serves as a demonstration of RAF case management for Pather & Pather Attorneys. The codebase is structured for maintainability and follows .NET Core best practices.

## 📄 License

MIT License - see [LICENSE](LICENSE) file for details.

Copyright (c) 2026 Pather & Pather Attorneys

## 🔗 Links

- **GitHub Repository**: [https://github.com/Uvaancovie/raf-pnp](https://github.com/Uvaancovie/raf-pnp)
- **Enterprise Development Plan**: [documentation/enterprise-planning/enterprise-protocol.md](raf-pnp/documentation/enterprise-planning/enterprise-protocol.md)</content>
<filePath>README.md