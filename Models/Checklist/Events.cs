namespace MockServer.Models.Checklist;

public record MockEvent(
    string? LogKey,
    string? EventType,
    string? MunicipalityId,
    string? Message,
    string? Owner,
    string? HistoryReference,
    string? SourceType,
    DateTime? Created,
    DateTime? Expires
);

public record Events(
    int Page,
    int Limit,
    int Count,
    int TotalRecords,
    int TotalPages,
    List<MockEvent> EventList
);
