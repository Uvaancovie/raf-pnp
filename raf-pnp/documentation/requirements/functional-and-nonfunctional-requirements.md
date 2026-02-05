# RAF PNP — Functional & Non-Functional Requirements

## Functional Requirements (what the system must **do**)

### FR1: Client Management
| ID | Requirement | Priority |
|----|-------------|----------|
| FR1.1 | Register new clients with personal details, SA ID (13-digit validated), contact info | Must |
| FR1.2 | Edit/update client records | Must |
| FR1.3 | Search and filter clients by name, ID number, phone | Must |
| FR1.4 | View all cases linked to a client | Must |
| FR1.5 | Soft-delete clients (retain for POPIA audit trail) | Must |

### FR2: Case Management
| ID | Requirement | Priority |
|----|-------------|----------|
| FR2.1 | Create RAF case linked to a client with case number, accident date, description | Must |
| FR2.2 | Track case through all 11 RAF stages (ClientIntake → Finalised) | Must |
| FR2.3 | Record financial data (estimated claim value, settlement amount) | Must |
| FR2.4 | Store key dates (accident, MMI, compliance, 120-day, trial) | Must |
| FR2.5 | Assign cases to attorneys / team members | Should |
| FR2.6 | Automatic deadline calculations (e.g. 120-day wait from compliance date) | Should |
| FR2.7 | Case timeline / activity history view | Should |
| FR2.8 | Bulk status update for multiple cases | Could |

### FR3: Document Management
| ID | Requirement | Priority |
|----|-------------|----------|
| FR3.1 | Upload documents to a case (AccidentReport, MedicalReport, RAF Forms, etc.) | Must |
| FR3.2 | Categorise documents by `DocumentType` enum | Must |
| FR3.3 | Download / preview uploaded documents | Must |
| FR3.4 | Track upload date and uploading user | Must |
| FR3.5 | Document versioning | Should |
| FR3.6 | Full-text search across documents | Could |

### FR4: Expert Appointments
| ID | Requirement | Priority |
|----|-------------|----------|
| FR4.1 | Schedule expert appointments (Neurologist, Actuary, OT, etc.) | Must |
| FR4.2 | Track appointment status (Scheduled, Completed, Cancelled, NoShow) | Must |
| FR4.3 | Record expert fees and link reports to case | Must |
| FR4.4 | Calendar view of upcoming appointments | Should |
| FR4.5 | Send appointment reminders (email/SMS) | Should |

### FR5: Activity & Audit Trail
| ID | Requirement | Priority |
|----|-------------|----------|
| FR5.1 | Log every case status change, document upload, and edit as a `CaseActivity` | Must |
| FR5.2 | Display activity timeline per case | Must |
| FR5.3 | Create manual activity notes / memos | Must |
| FR5.4 | Set reminders and follow-up dates | Should |

### FR6: Reporting & Dashboard
| ID | Requirement | Priority |
|----|-------------|----------|
| FR6.1 | Dashboard showing case counts by status, financial totals, upcoming deadlines | Must |
| FR6.2 | Filter reports by date range, attorney, status | Must |
| FR6.3 | Export reports to Excel / PDF | Should |
| FR6.4 | Predictive analytics on claim outcomes | Could |

### FR7: Authentication & Authorisation
| ID | Requirement | Priority |
|----|-------------|----------|
| FR7.1 | User login with email and password | Must |
| FR7.2 | Role-based access (Admin, Attorney, Paralegal, ReadOnly) | Must |
| FR7.3 | Password reset / recovery flow | Must |
| FR7.4 | Multi-factor authentication | Should |

---

## Non-Functional Requirements (how the system must **behave**)

### NFR1: Performance
| ID | Requirement | Target |
|----|-------------|--------|
| NFR1.1 | Page load time | < 2 seconds (p95) |
| NFR1.2 | API response time | < 200 ms (p95) |
| NFR1.3 | Database query time | < 100 ms (p95) |
| NFR1.4 | Concurrent users supported | 100+ simultaneous |
| NFR1.5 | Dashboard data refresh | < 3 seconds |

### NFR2: Security
| ID | Requirement | Target |
|----|-------------|--------|
| NFR2.1 | HTTPS enforced on all endpoints | 100% |
| NFR2.2 | OWASP Top 10 vulnerabilities | 0 high/critical |
| NFR2.3 | SQL injection protection | EF Core parameterised queries |
| NFR2.4 | XSS protection | Razor auto-encoding + CSP headers |
| NFR2.5 | Data encryption at rest | AES-256 for PII fields |
| NFR2.6 | Data encryption in transit | TLS 1.2+ |
| NFR2.7 | Session timeout | 30 minutes idle |
| NFR2.8 | Failed login lockout | 5 attempts → 15 min lockout |

### NFR3: Compliance (POPIA / Legal)
| ID | Requirement | Target |
|----|-------------|--------|
| NFR3.1 | Audit log retention | 7 years minimum |
| NFR3.2 | PII access logging | Every read/write logged |
| NFR3.3 | Data export on request (POPIA right of access) | Within 48 hours |
| NFR3.4 | Data deletion on request (right to be forgotten) | Soft-delete + anonymise within 30 days |
| NFR3.5 | Consent capture for data processing | Mandatory at client intake |
| NFR3.6 | Data breach notification capability | Within 72 hours |

### NFR4: Reliability & Availability
| ID | Requirement | Target |
|----|-------------|--------|
| NFR4.1 | Uptime SLA | 99.5% (excluding planned maintenance) |
| NFR4.2 | Recovery Point Objective (RPO) | < 1 hour |
| NFR4.3 | Recovery Time Objective (RTO) | < 4 hours |
| NFR4.4 | Automated database backups | Every 6 hours |
| NFR4.5 | Disaster recovery tested | Quarterly |

### NFR5: Maintainability
| ID | Requirement | Target |
|----|-------------|--------|
| NFR5.1 | Unit test code coverage | ≥ 80% |
| NFR5.2 | Cyclomatic complexity per method | < 10 |
| NFR5.3 | CI build time | < 5 minutes |
| NFR5.4 | Zero-downtime deployments | Required for production |
| NFR5.5 | Coding standards enforced | `.editorconfig` + analysers |

### NFR6: Scalability
| ID | Requirement | Target |
|----|-------------|--------|
| NFR6.1 | Horizontal scaling support | Stateless app tier |
| NFR6.2 | Database connection pooling | Configured in EF Core |
| NFR6.3 | Static asset delivery | CDN-backed |
| NFR6.4 | File storage | Azure Blob (not local disk) |

### NFR7: Usability
| ID | Requirement | Target |
|----|-------------|--------|
| NFR7.1 | Mobile responsive | All pages via Bootstrap 5 |
| NFR7.2 | Accessibility | WCAG 2.1 AA |
| NFR7.3 | Browser support | Chrome, Edge, Firefox (latest 2 versions) |
| NFR7.4 | Maximum clicks to reach any case | ≤ 3 |
| NFR7.5 | Onboarding time for new user | < 1 hour with training material |

---

## Traceability Matrix (Requirements → Epics)

| Requirement Group | Enterprise Protocol Epic |
|---|---|
| FR1–FR2 (Client & Case) | Epic 3.2 — Advanced Case Management |
| FR3 (Documents) | Epic 3.3 — Document Management Enhancement |
| FR4 (Experts) | Epic 3.2 — Advanced Case Management |
| FR5 (Audit Trail) | Epic 1.2 — Compliance Framework |
| FR6 (Reporting) | Epic 6.1 — Advanced Reporting & Analytics |
| FR7 (Auth) | Epic 2.3 — Security Hardening |
| NFR1 (Performance) | Epic 4.2 — Performance Testing |
| NFR2 (Security) | Epic 2.3 + Epic 4.3 — Security Hardening & Testing |
| NFR3 (POPIA) | Epic 1.2 + Epic 5.1 — Compliance |
| NFR4 (Reliability) | Epic 2.1 — DevOps Pipeline |
| NFR5 (Maintainability) | Epic 2.2 — Testing Infrastructure |
| NFR6 (Scalability) | Epic 6.3 — Multi-Tenancy & Scalability |
| NFR7 (Usability) | Epic 3.2 — Advanced Case Management |
