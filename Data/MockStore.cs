using MockServer.Models.Checklist;
using MockServer.Models.Company;
using MockServer.Models.Employee;

namespace MockServer.Data;

public static class MockStore
{
    public static readonly List<PortalPersonData> Employees = SeedEmployees();
    public static readonly List<EmployeeChecklist> EmployeeChecklists = SeedEmployeeChecklists();
    public static readonly List<ChecklistTemplate> ChecklistTemplates = SeedChecklistTemplates();
    public static readonly List<ChecklistOrganization> ChecklistOrganizations = SeedChecklistOrganizations();
    public static readonly List<CompanyOrganization> CompanyOrgs = SeedCompanyOrgs();
    public static readonly Dictionary<int, OrganizationTree> OrgTrees = SeedOrgTrees();

    private static List<PortalPersonData> SeedEmployees() =>
    [
        new PortalPersonData(
            Personid: Guid.Parse("aaaaaaaa-0000-0000-0000-000000000001"),
            Givenname: "Anna",
            Lastname: "Lindgren",
            Fullname: "Anna Lindgren",
            Address: "Storgatan 1",
            PostalCode: "85230",
            City: "Sundsvall",
            WorkPhone: "060-123456",
            MobilePhone: "070-111111",
            ExtraMobilePhone: null,
            AboutMe: null,
            Email: "anna.lindgren@sundsvall.se",
            MailNickname: "anna.lindgren",
            Company: "Sundsvalls Kommun",
            CompanyId: 1,
            OrgTree: "Sundsvalls Kommun\\IT-Avdelningen",
            ReferenceNumber: "1001",
            IsManager: true,
            LoginName: "anna.lindgren",
            FullOrgTree: "Sundsvalls Kommun\\IT-Avdelningen\\Systemförvaltning"
        ),
        new PortalPersonData(
            Personid: Guid.Parse("bbbbbbbb-0000-0000-0000-000000000002"),
            Givenname: "Erik",
            Lastname: "Johansson",
            Fullname: "Erik Johansson",
            Address: "Lillgatan 4",
            PostalCode: "85231",
            City: "Sundsvall",
            WorkPhone: "060-654321",
            MobilePhone: "070-222222",
            ExtraMobilePhone: null,
            AboutMe: null,
            Email: "erik.johansson@sundsvall.se",
            MailNickname: "erik.johansson",
            Company: "Sundsvalls Kommun",
            CompanyId: 1,
            OrgTree: "Sundsvalls Kommun\\IT-Avdelningen",
            ReferenceNumber: "1002",
            IsManager: false,
            LoginName: "erik.johansson",
            FullOrgTree: "Sundsvalls Kommun\\IT-Avdelningen\\Systemförvaltning"
        ),
        new PortalPersonData(
            Personid: Guid.Parse("cccccccc-0000-0000-0000-000000000003"),
            Givenname: "Sara",
            Lastname: "Nilsson",
            Fullname: "Sara Nilsson",
            Address: "Mellangatan 7",
            PostalCode: "85232",
            City: "Sundsvall",
            WorkPhone: "060-789012",
            MobilePhone: "070-333333",
            ExtraMobilePhone: null,
            AboutMe: null,
            Email: "sara.nilsson@sundsvall.se",
            MailNickname: "sara.nilsson",
            Company: "Sundsvalls Kommun",
            CompanyId: 1,
            OrgTree: "Sundsvalls Kommun\\IT-Avdelningen",
            ReferenceNumber: "1003",
            IsManager: false,
            LoginName: "sara.nilsson",
            FullOrgTree: "Sundsvalls Kommun\\IT-Avdelningen\\Systemförvaltning"
        )
    ];

    private static List<EmployeeChecklist> SeedEmployeeChecklists()
    {
        var annaStakeholder = new Stakeholder(
            "aaaaaaaa-0000-0000-0000-000000000001",
            "Anna", "Lindgren",
            "anna.lindgren@sundsvall.se",
            "anna.lindgren",
            "IT-chef"
        );
        var erikStakeholder = new Stakeholder(
            "bbbbbbbb-0000-0000-0000-000000000002",
            "Erik", "Johansson",
            "erik.johansson@sundsvall.se",
            "erik.johansson",
            "Systemutvecklare"
        );

        return
        [
            // Checklista 1: Erik under Anna (manager-vy)
            new EmployeeChecklist(
                Id: "checklist-0000-0001",
                Employee: erikStakeholder,
                Manager: annaStakeholder,
                Completed: false,
                Locked: false,
                Mentor: new Mentor("aaaaaaaa-0000-0000-0000-000000000001", "Anna Lindgren"),
                DelegatedTo: [],
                Phases:
                [
                    new EmployeeChecklistPhase(
                        Id: "phase-0001",
                        Name: "Introduktion",
                        BodyText: "Välkommen till organisationen",
                        TimeToComplete: "P7D",
                        SortOrder: 1,
                        Tasks:
                        [
                            new EmployeeChecklistTask(
                                "task-0001", "Läs välkomstbrev", null,
                                "Välkomstbrev finns på intranätet.", 1,
                                RoleType.NEW_EMPLOYEE, QuestionType.YES_OR_NO,
                                false, false, null, FulfilmentStatus.TRUE,
                                DateTime.UtcNow.AddDays(-2), "erik.johansson"
                            ),
                            new EmployeeChecklistTask(
                                "task-0002", "Träffa närmaste chef", null,
                                "Boka möte med din chef Anna.", 2,
                                RoleType.NEW_EMPLOYEE, QuestionType.YES_OR_NO,
                                false, false, null, FulfilmentStatus.EMPTY,
                                null, null
                            ),
                            new EmployeeChecklistTask(
                                "task-0003", "Skaffa passerkort", null,
                                "Kontakta receptionen.", 3,
                                RoleType.NEW_EMPLOYEE, QuestionType.YES_OR_NO,
                                false, true, null, FulfilmentStatus.NOT_RELEVANT,
                                DateTime.UtcNow.AddDays(-1), "anna.lindgren"
                            )
                        ]
                    ),
                    new EmployeeChecklistPhase(
                        Id: "phase-0002",
                        Name: "IT & Verktyg",
                        BodyText: "Se till att du har rätt behörigheter",
                        TimeToComplete: "P14D",
                        SortOrder: 2,
                        Tasks:
                        [
                            new EmployeeChecklistTask(
                                "task-0004", "Beställ datorutrustning", null,
                                "Via IT-portalen.", 1,
                                RoleType.MANAGER_FOR_NEW_EMPLOYEE, QuestionType.YES_OR_NO,
                                false, false, null, FulfilmentStatus.TRUE,
                                DateTime.UtcNow.AddDays(-5), "anna.lindgren"
                            ),
                            new EmployeeChecklistTask(
                                "task-0005", "Sätt upp e-post och Teams", null,
                                "Kontakta IT-support vid problem.", 2,
                                RoleType.NEW_EMPLOYEE, QuestionType.COMPLETED_OR_NOT_RELEVANT,
                                false, false, null, FulfilmentStatus.EMPTY,
                                null, null
                            )
                        ]
                    )
                ],
                Created: DateTime.UtcNow.AddDays(-10),
                Updated: DateTime.UtcNow.AddDays(-2),
                StartDate: DateOnly.FromDateTime(DateTime.Today.AddDays(-10)),
                EndDate: DateOnly.FromDateTime(DateTime.Today.AddDays(80)),
                ExpirationDate: DateOnly.FromDateTime(DateTime.Today.AddDays(180))
            ),

            // Checklista 2: Anna som medarbetare (employee-vy)
            new EmployeeChecklist(
                Id: "checklist-0000-0002",
                Employee: annaStakeholder,
                Manager: new Stakeholder(
                    "dddddddd-0000-0000-0000-000000000004",
                    "Bo", "Eriksson",
                    "bo.eriksson@sundsvall.se",
                    "bo.eriksson",
                    "Avdelningschef"
                ),
                Completed: false,
                Locked: false,
                Mentor: null,
                DelegatedTo: [],
                Phases:
                [
                    new EmployeeChecklistPhase(
                        Id: "phase-0010",
                        Name: "Chefsinformation",
                        BodyText: "För nyanställda chefer",
                        TimeToComplete: "P14D",
                        SortOrder: 1,
                        Tasks:
                        [
                            new EmployeeChecklistTask(
                                "task-0010", "Genomgång av personalansvar", null,
                                "Boka möte med HR.", 1,
                                RoleType.NEW_MANAGER, QuestionType.YES_OR_NO,
                                false, false, null, FulfilmentStatus.EMPTY,
                                null, null
                            ),
                            new EmployeeChecklistTask(
                                "task-0011", "Signera chefspolicy", null,
                                null, 2,
                                RoleType.NEW_MANAGER, QuestionType.YES_OR_NO,
                                false, false, null, FulfilmentStatus.EMPTY,
                                null, null
                            )
                        ]
                    )
                ],
                Created: DateTime.UtcNow.AddDays(-3),
                Updated: DateTime.UtcNow.AddDays(-3),
                StartDate: DateOnly.FromDateTime(DateTime.Today.AddDays(-3)),
                EndDate: DateOnly.FromDateTime(DateTime.Today.AddDays(87)),
                ExpirationDate: DateOnly.FromDateTime(DateTime.Today.AddDays(187))
            ),

            // Checklista 3: Delegerad till Sara
            new EmployeeChecklist(
                Id: "checklist-0000-0003",
                Employee: new Stakeholder(
                    "eeeeeeee-0000-0000-0000-000000000005",
                    "Lars", "Persson",
                    "lars.persson@sundsvall.se",
                    "lars.persson",
                    "Administratör"
                ),
                Manager: annaStakeholder,
                Completed: false,
                Locked: false,
                Mentor: null,
                DelegatedTo: ["sara.nilsson@sundsvall.se"],
                Phases:
                [
                    new EmployeeChecklistPhase(
                        Id: "phase-0020",
                        Name: "Introduktion",
                        BodyText: "Välkommen till organisationen",
                        TimeToComplete: "P7D",
                        SortOrder: 1,
                        Tasks:
                        [
                            new EmployeeChecklistTask(
                                "task-0020", "Läs välkomstbrev", null,
                                "Välkomstbrev finns på intranätet.", 1,
                                RoleType.NEW_EMPLOYEE, QuestionType.YES_OR_NO,
                                false, false, null, FulfilmentStatus.EMPTY,
                                null, null
                            ),
                            new EmployeeChecklistTask(
                                "task-0021", "Träffa närmaste chef", null,
                                "Boka möte med din chef.", 2,
                                RoleType.NEW_EMPLOYEE, QuestionType.YES_OR_NO,
                                false, false, null, FulfilmentStatus.EMPTY,
                                null, null
                            )
                        ]
                    )
                ],
                Created: DateTime.UtcNow.AddDays(-5),
                Updated: DateTime.UtcNow.AddDays(-1),
                StartDate: DateOnly.FromDateTime(DateTime.Today.AddDays(-5)),
                EndDate: DateOnly.FromDateTime(DateTime.Today.AddDays(85)),
                ExpirationDate: DateOnly.FromDateTime(DateTime.Today.AddDays(185))
            )
        ];
    }

    private static List<ChecklistTemplate> SeedChecklistTemplates() =>
    [
        new ChecklistTemplate(
            Id: "template-0000-0001",
            Name: "IT-Onboarding-v2",
            DisplayName: "IT Onboarding 2025",
            Version: 2,
            LifeCycle: LifeCycle.ACTIVE,
            Created: DateTime.UtcNow.AddDays(-90),
            Updated: DateTime.UtcNow.AddDays(-30),
            LastSavedBy: "admin",
            Phases:
            [
                new Phase(
                    Id: "tphase-0001",
                    Name: "Dag 1",
                    BodyText: "Aktiviteter första dagen",
                    TimeToComplete: "P1D",
                    Permission: Permission.ADMIN,
                    SortOrder: 1,
                    Tasks:
                    [
                        new ChecklistTask("ttask-0001", "Välkomstmöte", null,
                            "Möt teamet.", 1, RoleType.NEW_EMPLOYEE,
                            QuestionType.YES_OR_NO, Permission.ADMIN,
                            DateTime.UtcNow.AddDays(-90), DateTime.UtcNow.AddDays(-30),
                            "admin", false),
                        new ChecklistTask("ttask-0002", "Säkerhetsinstruktion", null,
                            "Gå igenom säkerhetsrutiner.", 2, RoleType.NEW_EMPLOYEE,
                            QuestionType.YES_OR_NO, Permission.ADMIN,
                            DateTime.UtcNow.AddDays(-90), DateTime.UtcNow.AddDays(-30),
                            "admin", false),
                        new ChecklistTask("ttask-0003", "Systemåtkomst", null,
                            "Kontrollera inloggning.", 3, RoleType.MANAGER_FOR_NEW_EMPLOYEE,
                            QuestionType.YES_OR_NO, Permission.ADMIN,
                            DateTime.UtcNow.AddDays(-90), DateTime.UtcNow.AddDays(-30),
                            "admin", true)
                    ],
                    Created: DateTime.UtcNow.AddDays(-90),
                    Updated: DateTime.UtcNow.AddDays(-30),
                    LastSavedBy: "admin"
                ),
                new Phase(
                    Id: "tphase-0002",
                    Name: "Vecka 1",
                    BodyText: "Aktiviteter under första veckan",
                    TimeToComplete: "P7D",
                    Permission: Permission.ADMIN,
                    SortOrder: 2,
                    Tasks:
                    [
                        new ChecklistTask("ttask-0004", "IT-utbildning", null,
                            "Obligatorisk IT-säkerhetsutbildning.", 1, RoleType.NEW_EMPLOYEE,
                            QuestionType.YES_OR_NO, Permission.ADMIN,
                            DateTime.UtcNow.AddDays(-90), DateTime.UtcNow.AddDays(-30),
                            "admin", false),
                        new ChecklistTask("ttask-0005", "Interna system", null,
                            "Genomgång av ärendehantering.", 2, RoleType.NEW_EMPLOYEE,
                            QuestionType.COMPLETED_OR_NOT_RELEVANT, Permission.ADMIN,
                            DateTime.UtcNow.AddDays(-90), DateTime.UtcNow.AddDays(-30),
                            "admin", false),
                        new ChecklistTask("ttask-0006", "Löneadministration", null,
                            "Registrera bankkonto i lönesystemet.", 3, RoleType.NEW_EMPLOYEE,
                            QuestionType.YES_OR_NO, Permission.ADMIN,
                            DateTime.UtcNow.AddDays(-90), DateTime.UtcNow.AddDays(-30),
                            "admin", false)
                    ],
                    Created: DateTime.UtcNow.AddDays(-90),
                    Updated: DateTime.UtcNow.AddDays(-30),
                    LastSavedBy: "admin"
                )
            ]
        )
    ];

    private static List<ChecklistOrganization> SeedChecklistOrganizations() =>
    [
        new ChecklistOrganization(
            Id: "org-checklist-0001",
            OrganizationName: "IT-Avdelningen",
            OrganizationNumber: 10,
            CommunicationChannels: ["EMAIL"],
            Created: DateTime.UtcNow.AddDays(-180),
            Updated: DateTime.UtcNow.AddDays(-30)
        )
    ];

    private static List<CompanyOrganization> SeedCompanyOrgs() =>
    [
        new CompanyOrganization(1,  "Sundsvalls Kommun",  null, false, 1, "SK",       1, "2281"),
        new CompanyOrganization(10, "IT-Avdelningen",     1,    false, 2, "SK.IT",    1, "2281"),
        new CompanyOrganization(100,"Systemförvaltning",  10,   true,  3, "SK.IT.SF", 1, "2281")
    ];

    private static Dictionary<int, OrganizationTree> SeedOrgTrees()
    {
        var leaf = new OrganizationTree(100, 3, "Systemförvaltning", 10, true,  1, "SK.IT.SF", []);
        var it   = new OrganizationTree(10,  2, "IT-Avdelningen",    1,  false, 1, "SK.IT",    [leaf]);
        var root = new OrganizationTree(1,   1, "Sundsvalls Kommun", 0,  false, 1, "SK",       [it]);

        return new Dictionary<int, OrganizationTree>
        {
            [1]   = root,
            [10]  = it,
            [100] = leaf
        };
    }
}
