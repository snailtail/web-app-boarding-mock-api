namespace MockServer.Models.Checklist;

public record ChecklistTask(
    string? Id,
    string? Heading,
    string? HeadingReference,
    string? Text,
    int? SortOrder,
    RoleType? RoleType,
    QuestionType? QuestionType,
    Permission? Permission,
    DateTime? Created,
    DateTime? Updated,
    string? LastSavedBy,
    bool? Optional
);
