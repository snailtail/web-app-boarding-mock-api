namespace MockServer.Models.Checklist;

public record Phase(
    string? Id,
    string? Name,
    string? BodyText,
    string? TimeToComplete,
    Permission? Permission,
    int? SortOrder,
    List<ChecklistTask>? Tasks,
    DateTime? Created,
    DateTime? Updated,
    string? LastSavedBy
);
