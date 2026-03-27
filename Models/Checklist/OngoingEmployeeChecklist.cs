namespace MockServer.Models.Checklist;

public record OngoingEmployeeChecklist(
    string? EmployeeName,
    string? EmployeeUsername,
    string? ManagerName,
    string? DepartmentName,
    List<string>? DelegatedTo,
    string? EmploymentDate,
    string? PurgeDate
);
