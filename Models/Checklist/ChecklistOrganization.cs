namespace MockServer.Models.Checklist;

public record ChecklistOrganization(
    string? Id,
    string? OrganizationName,
    int? OrganizationNumber,
    List<string>? CommunicationChannels,
    DateTime? Created,
    DateTime? Updated
);
