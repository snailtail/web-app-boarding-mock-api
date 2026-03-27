namespace MockServer.Models.Employee;

public record PortalPersonData(
    Guid Personid,
    string? Givenname,
    string? Lastname,
    string? Fullname,
    string? Address,
    string? PostalCode,
    string? City,
    string? WorkPhone,
    string? MobilePhone,
    string? ExtraMobilePhone,
    string? AboutMe,
    string? Email,
    string? MailNickname,
    string? Company,
    int CompanyId,
    string? OrgTree,
    string? ReferenceNumber,
    bool IsManager,
    string? LoginName,
    string? FullOrgTree
);
