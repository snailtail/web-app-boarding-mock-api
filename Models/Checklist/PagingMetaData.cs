namespace MockServer.Models.Checklist;

public record PagingMetaData(
    int Page,
    int Limit,
    int Count,
    int TotalRecords,
    int TotalPages
);
