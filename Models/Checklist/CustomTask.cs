namespace MockServer.Models.Checklist;

public record CustomTask(
    string? Id,
    string? Heading,
    string? HeadingReference,
    string? Text,
    int? SortOrder,
    RoleType? RoleType,
    QuestionType? QuestionType,
    DateTime? Created,
    DateTime? Updated,
    string? LastSavedBy
);
