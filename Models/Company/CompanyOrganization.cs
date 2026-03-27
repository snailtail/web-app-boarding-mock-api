namespace MockServer.Models.Company;

public record CompanyOrganization(
    int OrgId,
    string? OrgName,
    int? ParentId,
    bool IsLeafLevel,
    int TreeLevel,
    string? ResponsibilityCode,
    int CompanyId,
    string? MunicipalityId
);
