namespace MockServer.Models.Checklist;

public record TaskItem(string? Id, int? Position);
public record PhaseItem(string? Id, int? Position, List<TaskItem>? Tasks);
public record SortorderRequest(List<PhaseItem>? Phases);
