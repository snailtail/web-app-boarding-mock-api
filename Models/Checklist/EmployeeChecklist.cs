namespace MockServer.Models.Checklist;

public record EmployeeChecklist(
    string? Id,
    Stakeholder? Employee,
    Stakeholder? Manager,
    bool? Completed,
    bool? Locked,
    Mentor? Mentor,
    List<string>? DelegatedTo,
    List<EmployeeChecklistPhase>? Phases,
    DateTime? Created,
    DateTime? Updated,
    DateOnly? StartDate,
    DateOnly? EndDate,
    DateOnly? ExpirationDate
);
