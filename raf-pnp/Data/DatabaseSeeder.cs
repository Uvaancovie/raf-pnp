using raf_pnp.Models;

namespace raf_pnp.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // Seed Users first (if not exists)
            if (!context.Users.Any())
            {
                var users = new List<User>
                {
                    new User
                    {
                        FullName = "Sanjay Pather",
                        Email = "sanjay.pather@patherlaw.co.za",
                        PhoneNumber = "+27825551234",
                        Role = "Senior Attorney",
                        WhatsAppNotificationsEnabled = true,
                        PhoneNumberVerified = true,
                        PreferredNotificationChannel = NotificationChannel.All,
                        IsActive = true
                    },
                    new User
                    {
                        FullName = "Vinesh Pather",
                        Email = "vinesh.pather@patherlaw.co.za",
                        PhoneNumber = "+27825552345",
                        Role = "Senior Attorney",
                        WhatsAppNotificationsEnabled = true,
                        PhoneNumberVerified = true,
                        PreferredNotificationChannel = NotificationChannel.All,
                        IsActive = true
                    },
                    new User
                    {
                        FullName = "Anjali Govender",
                        Email = "anjali.govender@patherlaw.co.za",
                        PhoneNumber = "+27825553456",
                        Role = "Candidate Attorney",
                        WhatsAppNotificationsEnabled = true,
                        PhoneNumberVerified = true,
                        PreferredNotificationChannel = NotificationChannel.WhatsApp,
                        IsActive = true
                    },
                    new User
                    {
                        FullName = "Rajesh Singh",
                        Email = "rajesh.singh@patherlaw.co.za",
                        PhoneNumber = "+27825554567",
                        Role = "Candidate Attorney",
                        WhatsAppNotificationsEnabled = false,
                        PreferredNotificationChannel = NotificationChannel.Email,
                        IsActive = true
                    },
                    new User
                    {
                        FullName = "Thandi Moodley",
                        Email = "thandi.moodley@patherlaw.co.za",
                        PhoneNumber = "+27825555678",
                        Role = "Candidate Attorney",
                        WhatsAppNotificationsEnabled = true,
                        PhoneNumberVerified = true,
                        PreferredNotificationChannel = NotificationChannel.All,
                        IsActive = true
                    },
                    new User
                    {
                        FullName = "Nokulunga Dube",
                        Email = "nokulunga.dube@patherlaw.co.za",
                        PhoneNumber = "+27825556789",
                        Role = "Paralegal",
                        WhatsAppNotificationsEnabled = true,
                        PhoneNumberVerified = true,
                        PreferredNotificationChannel = NotificationChannel.InApp,
                        IsActive = true
                    }
                };

                context.Users.AddRange(users);
                await context.SaveChangesAsync();

                // Create Teams
                var teams = new List<Team>
                {
                    new Team
                    {
                        Name = "RAF Litigation Team",
                        Description = "Handles all RAF litigation matters from pleading phase to trial",
                        LeadUserId = users[0].Id, // Sanjay Pather
                        IsActive = true
                    },
                    new Team
                    {
                        Name = "Compliance & Pre-Litigation",
                        Description = "Manages case intake, compliance lodgements, and pre-litigation preparation",
                        LeadUserId = users[1].Id, // Vinesh Pather
                        IsActive = true
                    },
                    new Team
                    {
                        Name = "Expert Coordination",
                        Description = "Coordinates expert appointments and report collection",
                        LeadUserId = users[2].Id, // Anjali Govender
                        IsActive = true
                    }
                };

                context.Teams.AddRange(teams);
                await context.SaveChangesAsync();

                // Create Team Members
                var teamMembers = new List<TeamMember>
                {
                    // RAF Litigation Team
                    new TeamMember { TeamId = teams[0].Id, UserId = users[0].Id, Role = TeamRole.Lead },
                    new TeamMember { TeamId = teams[0].Id, UserId = users[2].Id, Role = TeamRole.Member },
                    new TeamMember { TeamId = teams[0].Id, UserId = users[5].Id, Role = TeamRole.Member },

                    // Compliance & Pre-Litigation
                    new TeamMember { TeamId = teams[1].Id, UserId = users[1].Id, Role = TeamRole.Lead },
                    new TeamMember { TeamId = teams[1].Id, UserId = users[3].Id, Role = TeamRole.Member },
                    new TeamMember { TeamId = teams[1].Id, UserId = users[4].Id, Role = TeamRole.Member },

                    // Expert Coordination
                    new TeamMember { TeamId = teams[2].Id, UserId = users[2].Id, Role = TeamRole.Lead },
                    new TeamMember { TeamId = teams[2].Id, UserId = users[5].Id, Role = TeamRole.Member }
                };

                context.TeamMembers.AddRange(teamMembers);
                await context.SaveChangesAsync();
            }

            // Only seed if no data exists
            if (context.Clients.Any())
            {
                return;
            }

            // Create sample clients
            var clients = new List<Client>
            {
                new Client
                {
                    FirstName = "Thabo",
                    LastName = "Molefe",
                    IdNumber = "8506125185083",
                    PhoneNumber = "082 456 7890",
                    Email = "thabo.molefe@email.co.za",
                    Address = "45 Main Road, Durban, KwaZulu-Natal, 4001",
                    DateCreated = DateTime.Now.AddMonths(-18)
                },
                new Client
                {
                    FirstName = "Priya",
                    LastName = "Naidoo",
                    IdNumber = "9203145234089",
                    PhoneNumber = "083 234 5678",
                    Email = "priya.naidoo@gmail.com",
                    Address = "12 Smith Street, Pietermaritzburg, 3201",
                    DateCreated = DateTime.Now.AddMonths(-14)
                },
                new Client
                {
                    FirstName = "Sipho",
                    LastName = "Dlamini",
                    IdNumber = "7812095432087",
                    PhoneNumber = "072 345 6789",
                    Email = "sipho.dlamini@outlook.com",
                    Address = "78 Beach Road, Umhlanga, 4320",
                    DateCreated = DateTime.Now.AddMonths(-10)
                },
                new Client
                {
                    FirstName = "Fatima",
                    LastName = "Khan",
                    IdNumber = "9508234567082",
                    PhoneNumber = "084 567 8901",
                    Email = "fatima.khan@yahoo.com",
                    Address = "23 Victoria Embankment, Durban, 4001",
                    DateCreated = DateTime.Now.AddMonths(-8)
                },
                new Client
                {
                    FirstName = "John",
                    LastName = "van der Merwe",
                    IdNumber = "8001155189085",
                    PhoneNumber = "076 890 1234",
                    Email = "john.vdmerwe@email.co.za",
                    Address = "56 Musgrave Road, Berea, 4001",
                    DateCreated = DateTime.Now.AddMonths(-6)
                },
                new Client
                {
                    FirstName = "Nomvula",
                    LastName = "Zulu",
                    IdNumber = "9109234567081",
                    PhoneNumber = "082 111 2222",
                    Email = "nomvula.zulu@gmail.com",
                    Address = "34 Inanda Road, Phoenix, 4068",
                    DateCreated = DateTime.Now.AddMonths(-4)
                },
                new Client
                {
                    FirstName = "Rajesh",
                    LastName = "Pillay",
                    IdNumber = "8705125186080",
                    PhoneNumber = "083 333 4444",
                    Email = "rajesh.pillay@email.co.za",
                    Address = "89 Sparks Road, Overport, 4091",
                    DateCreated = DateTime.Now.AddMonths(-3)
                },
                new Client
                {
                    FirstName = "Lindiwe",
                    LastName = "Mthembu",
                    IdNumber = "9402145678086",
                    PhoneNumber = "071 555 6666",
                    Email = "lindiwe.m@outlook.com",
                    Address = "67 Umbilo Road, Durban, 4001",
                    DateCreated = DateTime.Now.AddMonths(-2)
                },
                new Client
                {
                    FirstName = "Mohammed",
                    LastName = "Essop",
                    IdNumber = "8811235189082",
                    PhoneNumber = "084 777 8888",
                    Email = "m.essop@gmail.com",
                    Address = "12 Grey Street, Durban CBD, 4001",
                    DateCreated = DateTime.Now.AddMonths(-1)
                },
                new Client
                {
                    FirstName = "Sarah",
                    LastName = "Nkosi",
                    IdNumber = "9606145234088",
                    PhoneNumber = "072 999 0000",
                    Email = "sarah.nkosi@yahoo.com",
                    Address = "45 Argyle Road, Glenwood, 4001",
                    DateCreated = DateTime.Now.AddDays(-14)
                }
            };

            context.Clients.AddRange(clients);
            await context.SaveChangesAsync();

            // Create sample cases at various stages of the RAF process
            var cases = new List<RafCase>
            {
                // Case 1: FINALISED - Demonstrates completed case
                new RafCase
                {
                    CaseNumber = "RAF-2024-001",
                    ClientId = clients[0].Id,
                    AccidentDate = DateTime.Now.AddMonths(-30),
                    AccidentDescription = "Motor vehicle collision on N3 highway. Client was a passenger in a taxi that was rear-ended by a truck. Sustained serious spinal injuries.",
                    AccidentLocation = "N3 Highway near Pietermaritzburg toll plaza",
                    Status = CaseStatus.Finalised,
                    DateOpened = DateTime.Now.AddMonths(-29),
                    DateClosed = DateTime.Now.AddMonths(-2),
                    InitialLodgementDate = DateTime.Now.AddMonths(-28),
                    MmiDate = DateTime.Now.AddMonths(-18),
                    ComplianceLodgementDate = DateTime.Now.AddMonths(-16),
                    StatutoryExpiryDate = DateTime.Now.AddMonths(-12),
                    SummonsIssueDate = DateTime.Now.AddMonths(-11),
                    AssignedAttorney = "Adv. S. Pather",
                    CandidateAttorney = "Mr. R. Singh",
                    FeeAgreementSigned = true,
                    ContingencyFeeAgreement = true,
                    EstimatedClaimValue = 1500000,
                    SettlementAmount = 1250000,
                    Notes = "Matter successfully settled at mediation. Client satisfied with outcome."
                },

                // Case 2: TRIAL PHASE - Active litigation
                new RafCase
                {
                    CaseNumber = "RAF-2024-002",
                    ClientId = clients[1].Id,
                    AccidentDate = DateTime.Now.AddMonths(-24),
                    AccidentDescription = "Pedestrian knocked down by uninsured vehicle at zebra crossing. Multiple fractures and traumatic brain injury.",
                    AccidentLocation = "Corner of West and Field Street, Durban CBD",
                    Status = CaseStatus.TrialPhase,
                    DateOpened = DateTime.Now.AddMonths(-23),
                    InitialLodgementDate = DateTime.Now.AddMonths(-22),
                    MmiDate = DateTime.Now.AddMonths(-12),
                    ComplianceLodgementDate = DateTime.Now.AddMonths(-10),
                    StatutoryExpiryDate = DateTime.Now.AddMonths(-6),
                    SummonsIssueDate = DateTime.Now.AddMonths(-5),
                    AssignedAttorney = "Adv. S. Pather",
                    CandidateAttorney = "Ms. A. Govender",
                    FeeAgreementSigned = true,
                    ContingencyFeeAgreement = true,
                    EstimatedClaimValue = 2500000,
                    Notes = "Trial date set for next month. All expert witnesses confirmed. RAF disputing quantum."
                },

                // Case 3: PRE-TRIAL PHASE
                new RafCase
                {
                    CaseNumber = "RAF-2024-003",
                    ClientId = clients[2].Id,
                    AccidentDate = DateTime.Now.AddMonths(-20),
                    AccidentDescription = "Motorcycle accident on M4 South Coast Road. Client collided with vehicle that failed to yield. Leg amputation required.",
                    AccidentLocation = "M4 South Coast Road, Amanzimtoti",
                    Status = CaseStatus.PreTrialPhase,
                    DateOpened = DateTime.Now.AddMonths(-19),
                    InitialLodgementDate = DateTime.Now.AddMonths(-18),
                    MmiDate = DateTime.Now.AddMonths(-8),
                    ComplianceLodgementDate = DateTime.Now.AddMonths(-7),
                    StatutoryExpiryDate = DateTime.Now.AddMonths(-3),
                    SummonsIssueDate = DateTime.Now.AddMonths(-2),
                    AssignedAttorney = "Adv. V. Pather",
                    CandidateAttorney = "Mr. T. Moodley",
                    FeeAgreementSigned = true,
                    ContingencyFeeAgreement = true,
                    EstimatedClaimValue = 3500000,
                    Notes = "Pre-trial conference scheduled. Discovery completed. Awaiting joint expert minutes."
                },

                // Case 4: PLEADING PHASE
                new RafCase
                {
                    CaseNumber = "RAF-2024-004",
                    ClientId = clients[3].Id,
                    AccidentDate = DateTime.Now.AddMonths(-18),
                    AccidentDescription = "Rear-end collision at traffic light. Whiplash and disc herniation. Client was breadwinner for family of 5.",
                    AccidentLocation = "Umgeni Road and Argyle Road intersection, Durban",
                    Status = CaseStatus.PleadingPhase,
                    DateOpened = DateTime.Now.AddMonths(-17),
                    InitialLodgementDate = DateTime.Now.AddMonths(-16),
                    MmiDate = DateTime.Now.AddMonths(-6),
                    ComplianceLodgementDate = DateTime.Now.AddMonths(-5),
                    StatutoryExpiryDate = DateTime.Now.AddMonths(-1),
                    SummonsIssueDate = DateTime.Now.AddDays(-20),
                    AssignedAttorney = "Adv. S. Pather",
                    CandidateAttorney = "Ms. N. Dube",
                    FeeAgreementSigned = true,
                    ContingencyFeeAgreement = true,
                    EstimatedClaimValue = 850000,
                    Notes = "Summons served. Notice of Intention to Defend received. Awaiting RAF plea within 20 court days."
                },

                // Case 5: SUMMONS ISSUED + PAJA APPLICATION
                new RafCase
                {
                    CaseNumber = "RAF-2025-001",
                    ClientId = clients[4].Id,
                    AccidentDate = DateTime.Now.AddMonths(-16),
                    AccidentDescription = "Bus accident on N2 North. Multiple passengers injured. Client sustained rib fractures and internal injuries.",
                    AccidentLocation = "N2 Highway near King Shaka International Airport",
                    Status = CaseStatus.PajaApplication,
                    DateOpened = DateTime.Now.AddMonths(-15),
                    InitialLodgementDate = DateTime.Now.AddMonths(-14),
                    MmiDate = DateTime.Now.AddMonths(-4),
                    ComplianceLodgementDate = DateTime.Now.AddMonths(-3),
                    StatutoryExpiryDate = DateTime.Now.AddDays(15),
                    SummonsIssueDate = DateTime.Now.AddDays(-10),
                    AssignedAttorney = "Adv. V. Pather",
                    CandidateAttorney = "Mr. R. Singh",
                    FeeAgreementSigned = true,
                    ContingencyFeeAgreement = true,
                    EstimatedClaimValue = 650000,
                    Notes = "PAJA application filed to compel RAF decision on serious injury assessment. Summons issued in parallel."
                },

                // Case 6: 120-DAY STATUTORY WAITING PERIOD
                new RafCase
                {
                    CaseNumber = "RAF-2025-002",
                    ClientId = clients[5].Id,
                    AccidentDate = DateTime.Now.AddMonths(-14),
                    AccidentDescription = "Hit and run accident. Client was cycling on road shoulder. Driver fled scene. Fractured pelvis and facial injuries.",
                    AccidentLocation = "Old Main Road, Hillcrest",
                    Status = CaseStatus.StatutoryWaitingPeriod,
                    DateOpened = DateTime.Now.AddMonths(-13),
                    InitialLodgementDate = DateTime.Now.AddMonths(-12),
                    MmiDate = DateTime.Now.AddMonths(-2),
                    ComplianceLodgementDate = DateTime.Now.AddDays(-60),
                    StatutoryExpiryDate = DateTime.Now.AddDays(60),
                    AssignedAttorney = "Adv. S. Pather",
                    CandidateAttorney = "Ms. A. Govender",
                    FeeAgreementSigned = true,
                    ContingencyFeeAgreement = true,
                    EstimatedClaimValue = 920000,
                    Notes = "Compliance lodgement completed. In 120-day statutory waiting period. Diary set for expiry date to issue summons if no response."
                },

                // Case 7: COMPLIANCE LODGEMENT
                new RafCase
                {
                    CaseNumber = "RAF-2025-003",
                    ClientId = clients[6].Id,
                    AccidentDate = DateTime.Now.AddMonths(-13),
                    AccidentDescription = "Collision with stationary vehicle. Client's vehicle was rear-ended while parked. Neck and back injuries.",
                    AccidentLocation = "Sandile Thusi Road, Greyville",
                    Status = CaseStatus.ComplianceLodgement,
                    DateOpened = DateTime.Now.AddMonths(-12),
                    InitialLodgementDate = DateTime.Now.AddMonths(-11),
                    MmiDate = DateTime.Now.AddMonths(-1),
                    ComplianceLodgementDate = DateTime.Now.AddDays(-5),
                    AssignedAttorney = "Adv. V. Pather",
                    CandidateAttorney = "Mr. T. Moodley",
                    FeeAgreementSigned = true,
                    ContingencyFeeAgreement = true,
                    EstimatedClaimValue = 450000,
                    Notes = "Full compliance lodgement submitted. All RAF 1, RAF 4 forms and expert reports included. Awaiting RAF acknowledgement."
                },

                // Case 8: EXPERT APPOINTMENTS PHASE
                new RafCase
                {
                    CaseNumber = "RAF-2025-004",
                    ClientId = clients[7].Id,
                    AccidentDate = DateTime.Now.AddMonths(-11),
                    AccidentDescription = "Side impact collision at intersection. Client was driver. Head injury and shoulder dislocation.",
                    AccidentLocation = "Intersection of Jan Smuts Highway and Stella Road, Pinetown",
                    Status = CaseStatus.ExpertAppointments,
                    DateOpened = DateTime.Now.AddMonths(-10),
                    InitialLodgementDate = DateTime.Now.AddMonths(-9),
                    MmiDate = DateTime.Now.AddMonths(1),
                    AssignedAttorney = "Adv. S. Pather",
                    CandidateAttorney = "Ms. N. Dube",
                    FeeAgreementSigned = true,
                    ContingencyFeeAgreement = true,
                    EstimatedClaimValue = 780000,
                    Notes = "MMI reached. Neurosurgeon and Clinical Psychologist appointments completed. Awaiting Industrial Psychologist and Actuary reports."
                },

                // Case 9: MMI WAITING PERIOD (approaching MMI)
                new RafCase
                {
                    CaseNumber = "RAF-2025-005",
                    ClientId = clients[8].Id,
                    AccidentDate = DateTime.Now.AddMonths(-10),
                    AccidentDescription = "Taxi collision. Client was commuter. Multiple soft tissue injuries and psychological trauma.",
                    AccidentLocation = "Warwick Avenue, Durban CBD",
                    Status = CaseStatus.MmiWaitingPeriod,
                    DateOpened = DateTime.Now.AddMonths(-9),
                    InitialLodgementDate = DateTime.Now.AddMonths(-8),
                    MmiDate = DateTime.Now.AddMonths(2),
                    AssignedAttorney = "Adv. V. Pather",
                    CandidateAttorney = "Mr. R. Singh",
                    FeeAgreementSigned = true,
                    ContingencyFeeAgreement = false,
                    EstimatedClaimValue = 380000,
                    Notes = "Monitoring client's recovery. GP follow-up reports obtained. Planning expert appointments for post-MMI."
                },

                // Case 10: CLIENT INTAKE (new case)
                new RafCase
                {
                    CaseNumber = "RAF-2026-001",
                    ClientId = clients[9].Id,
                    AccidentDate = DateTime.Now.AddDays(-21),
                    AccidentDescription = "Head-on collision on R102. Client was passenger. Fractured arm and lacerations. Currently hospitalized.",
                    AccidentLocation = "R102 near Tongaat",
                    Status = CaseStatus.ClientIntake,
                    DateOpened = DateTime.Now.AddDays(-14),
                    MmiDate = DateTime.Now.AddMonths(12).AddDays(-21),
                    AssignedAttorney = "Adv. S. Pather",
                    CandidateAttorney = "Ms. A. Govender",
                    FeeAgreementSigned = true,
                    ContingencyFeeAgreement = true,
                    Notes = "New intake. Accident report obtained. Collecting hospital records. Fee agreement signed. Preparing initial lodgement."
                },

                // Case 11: INITIAL LODGEMENT
                new RafCase
                {
                    CaseNumber = "RAF-2025-006",
                    ClientId = clients[0].Id, // Returning client with second case
                    AccidentDate = DateTime.Now.AddMonths(-3),
                    AccidentDescription = "Second accident for same client. Rear-ended at stop street. Aggravation of previous spinal injuries.",
                    AccidentLocation = "Florida Road, Morningside",
                    Status = CaseStatus.InitialLodgement,
                    DateOpened = DateTime.Now.AddMonths(-2),
                    InitialLodgementDate = DateTime.Now.AddDays(-30),
                    MmiDate = DateTime.Now.AddMonths(9),
                    AssignedAttorney = "Adv. S. Pather",
                    CandidateAttorney = "Mr. T. Moodley",
                    FeeAgreementSigned = true,
                    ContingencyFeeAgreement = true,
                    Notes = "Non-compliance lodgement sent via registered post. Preservation letter sent to RAF. Awaiting MMI period."
                }
            };

            context.Cases.AddRange(cases);
            await context.SaveChangesAsync();

            // Add expert appointments for relevant cases
            var expertAppointments = new List<ExpertAppointment>
            {
                // Finalised case - all experts completed
                new ExpertAppointment
                {
                    CaseId = cases[0].Id,
                    ExpertType = ExpertType.Neurosurgeon,
                    ExpertName = "Dr. A. Maharaj",
                    PracticeName = "Durban Neurosurgery",
                    ContactNumber = "031 201 5555",
                    Email = "dr.maharaj@neurosurg.co.za",
                    AppointmentDate = DateTime.Now.AddMonths(-20),
                    Status = AppointmentStatus.ReportReceived,
                    ReportReceivedDate = DateTime.Now.AddMonths(-19),
                    ExpertFee = 15000,
                    FeePaid = true
                },
                new ExpertAppointment
                {
                    CaseId = cases[0].Id,
                    ExpertType = ExpertType.ClinicalPsychologist,
                    ExpertName = "Dr. P. Reddy",
                    PracticeName = "Mind Matters Psychology",
                    ContactNumber = "031 303 4444",
                    Email = "preddy@mindmatters.co.za",
                    AppointmentDate = DateTime.Now.AddMonths(-19),
                    Status = AppointmentStatus.ReportReceived,
                    ReportReceivedDate = DateTime.Now.AddMonths(-18),
                    ExpertFee = 8500,
                    FeePaid = true
                },
                new ExpertAppointment
                {
                    CaseId = cases[0].Id,
                    ExpertType = ExpertType.Actuary,
                    ExpertName = "Mr. J. Smith",
                    PracticeName = "Actuarial Solutions",
                    ContactNumber = "011 555 6666",
                    Email = "jsmith@actuary.co.za",
                    AppointmentDate = DateTime.Now.AddMonths(-17),
                    Status = AppointmentStatus.ReportReceived,
                    ReportReceivedDate = DateTime.Now.AddMonths(-16),
                    ExpertFee = 12000,
                    FeePaid = true
                },

                // Trial case experts
                new ExpertAppointment
                {
                    CaseId = cases[1].Id,
                    ExpertType = ExpertType.Neurosurgeon,
                    ExpertName = "Dr. S. Moodley",
                    PracticeName = "KZN Neurology Centre",
                    ContactNumber = "031 205 7777",
                    AppointmentDate = DateTime.Now.AddMonths(-14),
                    Status = AppointmentStatus.ReportReceived,
                    ReportReceivedDate = DateTime.Now.AddMonths(-13),
                    ExpertFee = 18000,
                    FeePaid = true
                },
                new ExpertAppointment
                {
                    CaseId = cases[1].Id,
                    ExpertType = ExpertType.IndustrialPsychologist,
                    ExpertName = "Dr. N. Pillay",
                    PracticeName = "Industrial Assessment Centre",
                    ContactNumber = "031 301 8888",
                    AppointmentDate = DateTime.Now.AddMonths(-12),
                    Status = AppointmentStatus.ReportReceived,
                    ReportReceivedDate = DateTime.Now.AddMonths(-11),
                    ExpertFee = 9500,
                    FeePaid = true
                },

                // Expert appointments case - some pending
                new ExpertAppointment
                {
                    CaseId = cases[7].Id,
                    ExpertType = ExpertType.Neurosurgeon,
                    ExpertName = "Dr. A. Maharaj",
                    PracticeName = "Durban Neurosurgery",
                    ContactNumber = "031 201 5555",
                    AppointmentDate = DateTime.Now.AddDays(-30),
                    Status = AppointmentStatus.ReportReceived,
                    ReportReceivedDate = DateTime.Now.AddDays(-14),
                    ExpertFee = 15000,
                    FeePaid = true
                },
                new ExpertAppointment
                {
                    CaseId = cases[7].Id,
                    ExpertType = ExpertType.ClinicalPsychologist,
                    ExpertName = "Dr. T. Govender",
                    PracticeName = "Psychological Services Durban",
                    ContactNumber = "031 309 2222",
                    AppointmentDate = DateTime.Now.AddDays(-21),
                    Status = AppointmentStatus.ReportReceived,
                    ReportReceivedDate = DateTime.Now.AddDays(-7),
                    ExpertFee = 7500,
                    FeePaid = false,
                    Notes = "Report received. Invoice pending payment."
                },
                new ExpertAppointment
                {
                    CaseId = cases[7].Id,
                    ExpertType = ExpertType.IndustrialPsychologist,
                    ExpertName = "Dr. R. Naicker",
                    PracticeName = "Vocational Assessment Centre",
                    ContactNumber = "031 312 3333",
                    AppointmentDate = DateTime.Now.AddDays(14),
                    Status = AppointmentStatus.Scheduled,
                    ExpertFee = 9000,
                    FeePaid = false,
                    Notes = "Appointment confirmed for 2 weeks time."
                },
                new ExpertAppointment
                {
                    CaseId = cases[7].Id,
                    ExpertType = ExpertType.Actuary,
                    ExpertName = "Ms. L. van Wyk",
                    PracticeName = "Cape Actuarial Services",
                    ContactNumber = "021 444 5555",
                    Status = AppointmentStatus.Pending,
                    Notes = "To be appointed after Industrial Psychologist report received."
                }
            };

            context.ExpertAppointments.AddRange(expertAppointments);
            await context.SaveChangesAsync();

            // Add documents for various cases
            var documents = new List<CaseDocument>
            {
                // Finalised case documents
                new CaseDocument { CaseId = cases[0].Id, DocumentName = "Accident Report - SAPS", DocumentType = DocumentType.AccidentReport, DateUploaded = DateTime.Now.AddMonths(-28), DateReceived = DateTime.Now.AddMonths(-29) },
                new CaseDocument { CaseId = cases[0].Id, DocumentName = "Hospital Records - Addington", DocumentType = DocumentType.HospitalRecords, DateUploaded = DateTime.Now.AddMonths(-27), DateReceived = DateTime.Now.AddMonths(-28) },
                new CaseDocument { CaseId = cases[0].Id, DocumentName = "Fee Agreement", DocumentType = DocumentType.FeeAgreement, DateUploaded = DateTime.Now.AddMonths(-29) },
                new CaseDocument { CaseId = cases[0].Id, DocumentName = "RAF 1 Form", DocumentType = DocumentType.Raf1Form, DateUploaded = DateTime.Now.AddMonths(-16) },
                new CaseDocument { CaseId = cases[0].Id, DocumentName = "RAF 4 Serious Injury Assessment", DocumentType = DocumentType.Raf4Form, DateUploaded = DateTime.Now.AddMonths(-16) },
                new CaseDocument { CaseId = cases[0].Id, DocumentName = "Neurosurgeon Report - Dr Maharaj", DocumentType = DocumentType.ExpertReport, DateUploaded = DateTime.Now.AddMonths(-19) },
                new CaseDocument { CaseId = cases[0].Id, DocumentName = "Settlement Agreement", DocumentType = DocumentType.CourtOrder, DateUploaded = DateTime.Now.AddMonths(-2) },

                // Trial case documents
                new CaseDocument { CaseId = cases[1].Id, DocumentName = "Police Accident Report", DocumentType = DocumentType.AccidentReport, DateUploaded = DateTime.Now.AddMonths(-22) },
                new CaseDocument { CaseId = cases[1].Id, DocumentName = "St Augustine Hospital Records", DocumentType = DocumentType.HospitalRecords, DateUploaded = DateTime.Now.AddMonths(-21) },
                new CaseDocument { CaseId = cases[1].Id, DocumentName = "Summons", DocumentType = DocumentType.Summons, DateUploaded = DateTime.Now.AddMonths(-5) },
                new CaseDocument { CaseId = cases[1].Id, DocumentName = "RAF Plea", DocumentType = DocumentType.Plea, DateUploaded = DateTime.Now.AddMonths(-4) },

                // New intake case
                new CaseDocument { CaseId = cases[9].Id, DocumentName = "Accident Report - SAPS Tongaat", DocumentType = DocumentType.AccidentReport, DateUploaded = DateTime.Now.AddDays(-10) },
                new CaseDocument { CaseId = cases[9].Id, DocumentName = "Contingency Fee Agreement", DocumentType = DocumentType.ContingencyFeeAgreement, DateUploaded = DateTime.Now.AddDays(-14) },

                // Initial lodgement case
                new CaseDocument { CaseId = cases[10].Id, DocumentName = "Accident Report", DocumentType = DocumentType.AccidentReport, DateUploaded = DateTime.Now.AddMonths(-2) },
                new CaseDocument { CaseId = cases[10].Id, DocumentName = "Previous RAF 4 Form (First Accident)", DocumentType = DocumentType.Raf4Form, DateUploaded = DateTime.Now.AddMonths(-2) },
                new CaseDocument { CaseId = cases[10].Id, DocumentName = "Registered Post Receipt", DocumentType = DocumentType.Correspondence, DateUploaded = DateTime.Now.AddDays(-28) }
            };

            context.CaseDocuments.AddRange(documents);
            await context.SaveChangesAsync();

            // Add activities for various cases
            var activities = new List<CaseActivity>
            {
                // New case activities
                new CaseActivity { CaseId = cases[9].Id, ActivityType = ActivityType.StatusChanged, Title = "Case Created", Description = "New RAF case opened following motor vehicle accident.", ActivityDate = DateTime.Now.AddDays(-14), CreatedBy = "Ms. A. Govender" },
                new CaseActivity { CaseId = cases[9].Id, ActivityType = ActivityType.DocumentUploaded, Title = "Fee Agreement Signed", Description = "Contingency fee agreement signed by client.", ActivityDate = DateTime.Now.AddDays(-14), CreatedBy = "Ms. A. Govender" },
                new CaseActivity { CaseId = cases[9].Id, ActivityType = ActivityType.DocumentUploaded, Title = "Accident Report Obtained", Description = "SAPS accident report collected from Tongaat police station.", ActivityDate = DateTime.Now.AddDays(-10), CreatedBy = "Ms. A. Govender" },
                new CaseActivity { CaseId = cases[9].Id, ActivityType = ActivityType.ClientContact, Title = "Hospital Visit", Description = "Visited client at hospital. Collected initial statement and signed mandate.", ActivityDate = DateTime.Now.AddDays(-12), CreatedBy = "Ms. A. Govender" },
                new CaseActivity { CaseId = cases[9].Id, ActivityType = ActivityType.DiaryEntry, Title = "Reminder: Hospital Records", Description = "Follow up on hospital records request.", ActivityDate = DateTime.Now, CreatedBy = "Ms. A. Govender", IsReminder = true, ReminderDate = DateTime.Now.AddDays(7) },

                // Trial case activities
                new CaseActivity { CaseId = cases[1].Id, ActivityType = ActivityType.CourtFiling, Title = "Trial Date Confirmed", Description = "Trial date confirmed for next month at Durban High Court.", ActivityDate = DateTime.Now.AddDays(-7), CreatedBy = "Adv. S. Pather" },
                new CaseActivity { CaseId = cases[1].Id, ActivityType = ActivityType.ExpertAppointment, Title = "Expert Witnesses Confirmed", Description = "All expert witnesses confirmed availability for trial date.", ActivityDate = DateTime.Now.AddDays(-5), CreatedBy = "Ms. A. Govender" },

                // 120-day waiting case
                new CaseActivity { CaseId = cases[5].Id, ActivityType = ActivityType.StatusChanged, Title = "Compliance Lodgement Submitted", Description = "Full RAF compliant claim lodged. 120-day period commenced.", ActivityDate = DateTime.Now.AddDays(-60), CreatedBy = "Mr. T. Moodley" },
                new CaseActivity { CaseId = cases[5].Id, ActivityType = ActivityType.DiaryEntry, Title = "Diary: 120-Day Expiry", Description = "Diarise for summons if no RAF response.", ActivityDate = DateTime.Now.AddDays(-58), CreatedBy = "Mr. T. Moodley", IsReminder = true, ReminderDate = DateTime.Now.AddDays(60) },

                // Finalised case final activity
                new CaseActivity { CaseId = cases[0].Id, ActivityType = ActivityType.StatusChanged, Title = "Matter Finalised", Description = "Settlement of R1,250,000 reached at mediation. Client funds disbursed.", ActivityDate = DateTime.Now.AddMonths(-2), CreatedBy = "Adv. S. Pather" }
            };

            context.CaseActivities.AddRange(activities);
            await context.SaveChangesAsync();
        }
    }
}
