namespace MockServer.Models.Checklist;

public record ChecklistTemplate(
    string? Id,
    string? Name,
    string? DisplayName,
    int? Version,
    LifeCycle? LifeCycle,
    DateTime? Created,
    DateTime? Updated,
    string? LastSavedBy,
    List<Phase>? Phases
);
