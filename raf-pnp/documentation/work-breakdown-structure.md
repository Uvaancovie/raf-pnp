# Work Breakdown Structure - RAF PNP Case Management System

## Project Information

**Project Name:** RAF PNP - Road Accident Fund Case Management System
**Client:** Pather & Pather Attorneys
**Project Type:** Custom Web Application Development
**Technology Stack:** ASP.NET Core 8.0, Razor Pages, SQL Server, Entity Framework Core

---

## Cost Summary

### Initial Development Cost: **R6,000.00**
**Pricing Model:** R500.00 per page × 12 pages

### Monthly Retainer: **R3,000.00**

---

## 1. Initial Development Breakdown (R6,000.00)

### Pricing Justification: R500 per Page

The R500 per page rate is justified by the comprehensive development required for each page:

**What's Included Per Page:**
- **Backend Logic:** ASP.NET Core PageModel with business logic and data access
- **Database Integration:** Entity Framework Core queries, transactions, and relationships
- **Frontend UI:** Responsive HTML/CSS with Bootstrap 5 framework
- **Form Validation:** Client-side and server-side validation
- **Error Handling:** Try-catch blocks, user-friendly error messages, Sentry logging
- **Security:** CSRF protection, input sanitization, SQL injection prevention
- **Testing:** Unit tests for page models and integration testing
- **Documentation:** Code comments and technical documentation

**Industry Standards:**
- Junior developer rate: R200-300/hour
- Mid-level developer rate: R350-500/hour
- Average time per CRUD page: 2-3 hours
- R500/page = ~1.5 hours @ R350/hour (highly competitive rate)

---

## 1.1 Page-by-Page Breakdown (12 Pages @ R500)

### 1.1.1 Client Management Module (4 Pages = R2,000)
- **1.1.1.1** Clients Index/List Page
  - Display all clients in searchable table
  - Pagination and sorting functionality
  - Quick action buttons (View, Edit, Delete)
  - Client count statistics
  - **Cost:** R500.00

- **1.1.1.2** Client Create Page
  - Registration form with validation
  - South African ID number validation (13-digit format)
  - Real-time field validation
  - Success/error messaging
  - **Cost:** R500.00

- **1.1.1.3** Client Edit Page
  - Pre-populated form with existing data
  - Update functionality with validation
  - Concurrency handling
  - Change tracking
  - **Cost:** R500.00

- **1.1.1.4** Client Details Page
  - Comprehensive client information display
  - Associated cases listing
  - Document history
  - Action menu (Edit, Delete, Add Case)
  - **Cost:** R500.00

**Subtotal Client Module:** R2,000.00

### 1.1.2 Case Management Module (4 Pages = R2,000)
- **1.1.2.1** Cases Index/List Page
  - Display all cases with status indicators
  - Filtering by status, client, date range
  - Color-coded status badges
  - Search functionality
  - **Cost:** R500.00

- **1.1.2.2** Case Create Page
  - Comprehensive case intake form
  - Client selection dropdown
  - Accident details capture
  - Document upload integration
  - Initial status assignment
  - **Cost:** R500.00

- **1.1.2.3** Case Edit Page
  - Full case update functionality
  - Status progression through 13 RAF stages
  - Date field management (MMI, Compliance, etc.)
  - Settlement amount tracking
  - **Cost:** R500.00

- **1.1.2.4** Case Details Page
  - Complete case overview
  - Timeline visualization
  - Document management section
  - Expert appointments listing
  - Activity log display
  - Status progression controls
  - **Cost:** R500.00

**Subtotal Case Module:** R2,000.00

### 1.1.3 Dashboard & Analytics (2 Pages = R1,000)
- **1.1.3.1** Dashboard/Index Page
  - Case statistics by status (pie/bar charts)
  - Urgent cases requiring attention
  - RAF process flow visualization
  - Recent activity feed
  - Quick action buttons
  - Key performance indicators (KPIs)
  - **Cost:** R500.00

- **1.1.3.2** Reports Page
  - Case status distribution report
  - Deadline tracking report
  - Settlement value analysis
  - Date range filtering
  - Export functionality (PDF/Excel)
  - Interactive charts and graphs
  - **Cost:** R500.00

**Subtotal Dashboard & Analytics:** R1,000.00

### 1.1.4 Supporting Features (2 Pages = R1,000)
- **1.1.4.1** Document Management Interface
  - Document upload form
  - File type validation (PDF, DOC, images)
  - Document categorization (Accident Report, Medical, RAF Forms)
  - Document viewer/download
  - Integrated into Case Details
  - **Cost:** R500.00

- **1.1.4.2** Expert Appointments Page
  - Appointment scheduling form
  - Expert type selection (Neurologist, Actuary, etc.)
  - Calendar integration
  - Status tracking (Scheduled, Completed, Cancelled)
  - Fee management
  - **Cost:** R500.00

**Subtotal Supporting Features:** R1,000.00

---

**Total Page Development:** R6,000.00 (12 pages @ R500/page)

---

## 1.2 Detailed Technical Breakdown by Component

This section provides an alternative view of costs organized by technical components rather than pages. Both breakdowns total R6,000.

### 1.2.1 Database Layer (R800.00)
- **1.2.1.1** Entity design and modeling (Client, Case, Document, Expert, Activity)
  - Entity class creation with data annotations
  - Navigation properties configuration
  - Enum definitions (CaseStatus, DocumentType, ExpertType)
  - **Cost:** R300.00

- **1.2.1.2** Database schema and migrations
  - Initial migration creation
  - Database relationship configuration
  - Index optimization for queries
  - **Cost:** R200.00

- **1.2.1.3** Data integrity and constraints
  - Foreign key relationships
  - Cascade delete rules
  - Unique constraints and validation
  - **Cost:** R150.00

- **1.2.1.4** Data seeding
  - Sample client data (10 clients)
  - Sample cases (11 cases across all stages)
  - Expert appointments and documents
  - **Cost:** R150.00

**Subtotal Database Layer:** R800.00

### 1.2.2 Business Logic & Services (R1,200.00)
- **1.2.2.1** Client service layer
  - CRUD operations implementation
  - South African ID validation logic
  - Search and filtering logic
  - **Cost:** R300.00

- **1.2.2.2** Case management service
  - Case workflow management
  - Status progression logic
  - Date calculation (deadlines, expiry)
  - **Cost:** R400.00

- **1.2.2.3** Document handling service
  - File upload processing
  - File storage management
  - Document type validation
  - **Cost:** R250.00

- **1.2.2.4** Activity logging service
  - Audit trail creation
  - Activity type categorization
  - Timeline generation
  - **Cost:** R250.00

**Subtotal Business Logic:** R1,200.00

### 1.2.3 User Interface & Design (R800.00)
- **1.2.3.1** Layout and navigation
  - Master layout template
  - Navigation menu structure
  - Footer and header components
  - **Cost:** R150.00

- **1.2.3.2** Bootstrap customization
  - Theme color customization (#006a4e)
  - Card and table styling
  - Button and form styling
  - **Cost:** R150.00

- **1.2.3.3** Charts and visualizations
  - Status distribution charts
  - Timeline visualization
  - Progress indicators
  - **Cost:** R200.00

- **1.2.3.4** Responsive design
  - Mobile-first layout
  - Tablet optimization
  - Desktop view enhancements
  - **Cost:** R150.00

- **1.2.3.5** Pather & Pather branding
  - Logo integration
  - Color scheme application
  - Brand consistency across pages
  - **Cost:** R150.00

**Subtotal UI & Design:** R800.00

### 1.2.4 Reports & Analytics (R400.00)
- **1.2.4.1** Dashboard statistics
  - Case count by status
  - KPI calculations
  - Recent activity queries
  - **Cost:** R150.00

- **1.2.4.2** Report generation
  - Status distribution report
  - Deadline tracking report
  - Settlement analysis
  - **Cost:** R200.00

- **1.2.4.3** Export functionality
  - PDF generation
  - Excel export
  - Data formatting
  - **Cost:** R50.00

**Subtotal Reports:** R400.00

### 1.2.5 Infrastructure & DevOps (R800.00)
- **1.2.5.1** Project setup
  - Development environment configuration
  - Solution structure
  - NuGet package management
  - **Cost:** R200.00

- **1.2.5.2** Error monitoring
  - Sentry integration
  - Error logging configuration
  - Exception handling middleware
  - **Cost:** R150.00

- **1.2.5.3** Testing framework
  - Unit test setup
  - Integration test configuration
  - Test data preparation
  - **Cost:** R200.00

- **1.2.5.4** Deployment
  - Deployment configuration
  - Environment settings
  - Database migration scripts
  - **Cost:** R150.00

- **1.2.5.5** Documentation
  - Technical documentation
  - User guides
  - Code comments
  - **Cost:** R100.00

**Subtotal Infrastructure:** R800.00

### 1.2.6 Security & Validation (R500.00)
- **1.2.6.1** Input validation
  - Client-side validation
  - Server-side validation
  - Custom validation attributes
  - **Cost:** R200.00

- **1.2.6.2** Security measures
  - CSRF protection (anti-forgery tokens)
  - SQL injection prevention (parameterized queries)
  - XSS protection (Razor encoding)
  - **Cost:** R200.00

- **1.2.6.3** Error handling
  - Try-catch blocks
  - User-friendly error messages
  - Error page customization
  - **Cost:** R100.00

**Subtotal Security:** R500.00

### 1.2.7 Quality Assurance & Testing (R500.00)
- **1.2.7.1** Unit testing
  - PageModel tests
  - Service layer tests
  - Validation tests
  - **Cost:** R200.00

- **1.2.7.2** Integration testing
  - Database integration tests
  - End-to-end page tests
  - API endpoint tests
  - **Cost:** R150.00

- **1.2.7.3** User acceptance testing
  - Test case preparation
  - Bug fixing during UAT
  - User feedback incorporation
  - **Cost:** R150.00

**Subtotal QA & Testing:** R500.00

---

## 1.3 Cost Breakdown Summary

### Page-Based View (12 Pages @ R500 each)

| Component | Cost | Percentage |
|-----------|------|------------|
| **Client Module** (4 pages) | R2,000.00 | 33.3% |
| **Case Module** (4 pages) | R2,000.00 | 33.3% |
| **Dashboard & Analytics** (2 pages) | R1,000.00 | 16.7% |
| **Supporting Features** (2 pages) | R1,000.00 | 16.7% |
| **Total** | **R6,000.00** | **100%** |

---

### Component-Based View (Technical Breakdown)

| Component | Cost | Percentage |
|-----------|------|------------|
| Database Layer | R800.00 | 13.3% |
| Business Logic & Services | R1,200.00 | 20.0% |
| User Interface & Design | R800.00 | 13.3% |
| Reports & Analytics | R400.00 | 6.7% |
| Infrastructure & DevOps | R800.00 | 13.3% |
| Security & Validation | R500.00 | 8.3% |
| Quality Assurance & Testing | R500.00 | 8.3% |
| Page Integration & Assembly | R1,000.00 | 16.7% |
| **Total** | **R6,000.00** | **100%** |

> **Note:** Both breakdowns represent the same R6,000 total cost, viewed from different perspectives:
> - **Page-Based View:** Cost organized by functional modules and user-facing pages
> - **Component-Based View:** Cost organized by technical layers and infrastructure components

---

## 2. Monthly Retainer Services (R3,000.00/month)

### 2.1 Maintenance & Support (R1,200.00)
- Bug fixes and issue resolution
- Performance monitoring and optimization
- Security updates and patches
- Database maintenance and optimization
- Server monitoring and uptime management

### 2.2 Feature Enhancements (R800.00)
- Minor feature additions (up to 4 hours/month)
- UI/UX improvements
- Workflow optimizations
- Report customizations
- User experience enhancements

### 2.3 Technical Support (R600.00)
- Email support during business hours
- Remote assistance for technical issues
- User training and guidance
- System configuration changes
- Backup verification and management

### 2.4 Monitoring & Reporting (R400.00)
- Monthly system health reports
- Usage analytics and insights
- Performance metrics tracking
- Error log analysis via Sentry
- Proactive issue identification

---

## 3. Deliverables

### Phase 1: Initial Development (Included in R6,000)
- ✅ Fully functional RAF Case Management System
- ✅ Source code repository with version control
- ✅ SQL Server database with sample data
- ✅ User interface with Pather & Pather branding
- ✅ Dashboard with analytics and reports
- ✅ Complete CRUD operations for all entities
- ✅ Document upload and management system
- ✅ Expert appointment scheduling
- ✅ Activity tracking and audit trails
- ✅ Error monitoring integration (Sentry)
- ✅ User documentation
- ✅ Technical documentation

### Phase 2: Monthly Retainer (Ongoing)
- 📊 Monthly system health and usage reports
- 🛠️ Continuous maintenance and support
- 🔄 Regular updates and improvements
- 📞 Technical support availability
- 🔍 Proactive monitoring and issue resolution

---

## 4. Exclusions (Not Included in Base Cost)

The following items are excluded from the initial development and monthly retainer:

- ❌ Advanced authentication/authorization system (user roles, permissions)
- ❌ Email notification system
- ❌ SMS integration for reminders
- ❌ Advanced reporting with custom report builder
- ❌ Mobile application development
- ❌ Third-party API integrations (e.g., RAF official systems)
- ❌ Multi-tenant architecture
- ❌ Cloud hosting costs (Azure/AWS)
- ❌ SSL certificates
- ❌ Domain registration and DNS management
- ❌ Major feature requests exceeding 4 hours/month
- ❌ Data migration from legacy systems
- ❌ Custom workflow automation

---

## 5. Payment Terms

### Initial Development (R6,000.00)
- **50% Upfront:** R3,000.00 (Project commencement)
- **50% On Completion:** R3,000.00 (Upon delivery and acceptance)

### Monthly Retainer (R3,000.00)
- **Billing Cycle:** Monthly in advance
- **Payment Terms:** Due on the 1st of each month
- **Contract Period:** Minimum 3 months, then month-to-month
- **Cancellation Notice:** 30 days written notice

---

## 6. Timeline Estimates

### Initial Development
- **Total Duration:** 4-6 weeks
- **Week 1-2:** Database design, core setup, client/case modules
- **Week 3-4:** Documents, experts, activities, UI design
- **Week 5:** Reports, testing, bug fixes
- **Week 6:** Deployment, documentation, training

### Ongoing Support
- **Response Time:** Within 24 business hours
- **Critical Issues:** Within 4 business hours
- **Feature Requests:** Prioritized and scheduled monthly

---

## 7. Assumptions

This WBS is based on the following assumptions:

1. Client provides all necessary business requirements and RAF process documentation
2. SQL Server database server is available and accessible
3. Hosting environment is provided or selected by client
4. Sample data structure is suitable for production use
5. Pather & Pather provides branding assets (logo, color codes)
6. No integration with external RAF or government systems required
7. Standard business hours support (Monday-Friday, 8:00 AM - 5:00 PM)
8. Client provides timely feedback during development
9. No custom security or compliance requirements beyond standard best practices
10. Scope is limited to features outlined in the project README

---

## 8. Change Management

### Change Request Process
1. Client submits change request in writing
2. Developer assesses impact, effort, and cost
3. Quote provided within 48 hours
4. Client approval required before proceeding
5. Changes implemented upon approval

### Hourly Rate (for out-of-scope work)
- **Development Work:** R350/hour
- **Consultation:** R300/hour
- **Emergency Support (after hours):** R500/hour

---

## 9. Risk Management

### Identified Risks
1. **Scope Creep:** Mitigated by clear WBS and change management process
2. **Technical Dependencies:** SQL Server and .NET 8 compatibility verified upfront
3. **Data Security:** Implementing best practices for sensitive client information
4. **Performance:** Database optimization and query tuning included
5. **Browser Compatibility:** Testing on modern browsers (Chrome, Edge, Firefox)

### Mitigation Strategies
- Regular client communication and progress updates
- Version control for code safety and rollback capability
- Staging environment for testing before production deployment
- Automated backups and disaster recovery planning
- Comprehensive error monitoring via Sentry

---

## 10. Success Criteria

The project will be considered successful upon:

✅ All core modules functioning as specified
✅ Dashboard displaying accurate statistics and charts
✅ All 13 RAF case stages properly implemented
✅ Document upload and management working correctly
✅ Expert appointment scheduling operational
✅ Reports generating accurate data
✅ User interface is responsive and branded correctly
✅ No critical bugs or errors
✅ Application deployed to production environment
✅ Client training completed and documentation delivered
✅ Client sign-off obtained

---

## Contact Information

For questions about this Work Breakdown Structure or to discuss project scope:

**Developer:** [Your Name]
**Email:** [Your Email]
**Phone:** [Your Phone]
**Project Repository:** https://github.com/Uvaancovie/raf-pnp

---

## Document History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2026-02-10 | Development Team | Initial WBS creation |

---

**© 2026 Pather & Pather Attorneys - RAF PNP Case Management System**
