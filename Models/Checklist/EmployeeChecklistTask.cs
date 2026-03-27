namespace MockServer.Models.Checklist;

public record EmployeeChecklistTask(
    string? Id,
    string? Heading,
    string? HeadingReference,
    string? Text,
    int? SortOrder,
    RoleType? RoleType,
    QuestionType? QuestionType,
    bool? CustomTask,
    bool? Optional,
    string? ResponseText,
    FulfilmentStatus? FulfilmentStatus,
    DateTime? Updated,
    string? UpdatedBy
);
