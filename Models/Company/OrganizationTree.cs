namespace MockServer.Models.Company;

public record OrganizationTree(
    int OrgId,
    int TreeLevel,
    string? OrgName,
    int? ParentId,
    bool IsLeafLevel,
    int CompanyId,
    string? ResponsibilityCode,
    List<OrganizationTree>? Organizations
);
