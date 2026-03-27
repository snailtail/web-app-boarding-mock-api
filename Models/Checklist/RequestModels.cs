namespace MockServer.Models.Checklist;

public record EmployeeChecklistTaskUpdateRequest(
    FulfilmentStatus? FulfilmentStatus,
    string? ResponseText,
    string? UpdatedBy
);

public record CustomTaskCreateRequest(
    string? Heading,
    string? HeadingReference,
    string? Text,
    int? SortOrder,
    RoleType? RoleType,
    QuestionType? QuestionType
);

public record CustomTaskUpdateRequest(
    string? Heading,
    string? HeadingReference,
    string? Text,
    int? SortOrder,
    RoleType? RoleType,
    QuestionType? QuestionType,
    string? UpdatedBy
);

public record ChecklistCreateRequest(
    string? Name,
    string? DisplayName,
    string? CreatedBy
);

public record TaskCreateRequest(
    string? Heading,
    string? HeadingReference,
    string? Text,
    int? SortOrder,
    RoleType? RoleType,
    QuestionType? QuestionType,
    Permission? Permission,
    bool? Optional,
    string? CreatedBy
);

public record TaskUpdateRequest(
    string? Heading,
    string? HeadingReference,
    string? Text,
    int? SortOrder,
    RoleType? RoleType,
    QuestionType? QuestionType,
    Permission? Permission,
    bool? Optional,
    string? UpdatedBy
);
