namespace MockServer.Models.Checklist;

public record EmployeeChecklistPhase(
    string? Id,
    string? Name,
    string? BodyText,
    string? TimeToComplete,
    int? SortOrder,
    List<EmployeeChecklistTask>? Tasks
);
