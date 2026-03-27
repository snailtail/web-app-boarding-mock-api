using MockServer.Data;
using MockServer.Models.Checklist;

namespace MockServer.Endpoints;

public static class ChecklistEndpoints
{
    private const string Base = "/checklist/1.3/2281";

    public static void MapChecklistEndpoints(this WebApplication app)
    {
        // --- Employee Checklists ---

        app.MapGet($"{Base}/employee-checklists/manager/{{username}}", (string username) =>
        {
            var checklists = MockStore.EmployeeChecklists
                .Where(c => c.Manager?.Username == username)
                .ToList();
            return checklists.Count > 0 ? Results.Ok(checklists) : Results.NotFound();
        });

        app.MapGet($"{Base}/employee-checklists/employee/{{username}}", (string username) =>
        {
            var checklist = MockStore.EmployeeChecklists
                .FirstOrDefault(c => c.Employee?.Username == username);
            return checklist is not null ? Results.Ok(checklist) : Results.NotFound();
        });

        app.MapGet($"{Base}/employee-checklists/delegated-to/{{username}}", (string username) =>
        {
            var person = MockStore.Employees.FirstOrDefault(e => e.LoginName == username);
            var email = person?.Email ?? string.Empty;
            var checklists = MockStore.EmployeeChecklists
                .Where(c => c.DelegatedTo != null && c.DelegatedTo.Contains(email))
                .ToList();
            return Results.Ok(new DelegatedEmployeeChecklistResponse(checklists));
        });

        app.MapGet($"{Base}/employee-checklists/ongoing", (
            int page = 1,
            int limit = 20,
            string? employeeName = null,
            string? sortBy = null,
            string? sortDirection = null) =>
        {
            var all = MockStore.EmployeeChecklists
                .Select(c => new OngoingEmployeeChecklist(
                    EmployeeName: c.Employee?.FirstName + " " + c.Employee?.LastName,
                    EmployeeUsername: c.Employee?.Username,
                    ManagerName: c.Manager?.FirstName + " " + c.Manager?.LastName,
                    DepartmentName: "IT-Avdelningen",
                    DelegatedTo: c.DelegatedTo,
                    EmploymentDate: c.StartDate?.ToString("yyyy-MM-dd"),
                    PurgeDate: c.ExpirationDate?.ToString("yyyy-MM-dd")
                ))
                .ToList();

            if (!string.IsNullOrEmpty(employeeName))
                all = all.Where(o => o.EmployeeName != null &&
                    o.EmployeeName.Contains(employeeName, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            var totalRecords = all.Count;
            var totalPages = (int)Math.Ceiling(totalRecords / (double)limit);
            var items = all.Skip((page - 1) * limit).Take(limit).ToList();

            return Results.Ok(new OngoingEmployeeChecklists(items)
            {
                Meta = new PagingMetaData(page, limit, items.Count, totalRecords, totalPages)
            });
        });

        app.MapPatch($"{Base}/employee-checklists/{{checklistId}}/tasks/{{taskId}}", (
            string checklistId,
            string taskId,
            EmployeeChecklistTaskUpdateRequest req) =>
        {
            var checklist = MockStore.EmployeeChecklists.FirstOrDefault(c => c.Id == checklistId);
            if (checklist is null) return Results.NotFound();

            foreach (var phase in checklist.Phases ?? [])
            {
                var tasks = phase.Tasks;
                if (tasks is null) continue;
                var idx = tasks.FindIndex(t => t.Id == taskId);
                if (idx < 0) continue;

                var updated = tasks[idx] with
                {
                    FulfilmentStatus = req.FulfilmentStatus ?? tasks[idx].FulfilmentStatus,
                    ResponseText = req.ResponseText ?? tasks[idx].ResponseText,
                    UpdatedBy = req.UpdatedBy ?? tasks[idx].UpdatedBy,
                    Updated = DateTime.UtcNow
                };
                tasks[idx] = updated;
                return Results.Ok(updated);
            }
            return Results.NotFound();
        });

        app.MapPost($"{Base}/employee-checklists/{{checklistId}}/phases/{{phaseId}}/customtasks", (
            string checklistId,
            string phaseId,
            CustomTaskCreateRequest req) =>
        {
            var checklist = MockStore.EmployeeChecklists.FirstOrDefault(c => c.Id == checklistId);
            if (checklist is null) return Results.NotFound();

            var phase = checklist.Phases?.FirstOrDefault(p => p.Id == phaseId);
            if (phase is null) return Results.NotFound();

            var newTask = new EmployeeChecklistTask(
                Id: Guid.NewGuid().ToString(),
                Heading: req.Heading,
                HeadingReference: req.HeadingReference,
                Text: req.Text,
                SortOrder: req.SortOrder,
                RoleType: req.RoleType,
                QuestionType: req.QuestionType,
                CustomTask: true,
                Optional: false,
                ResponseText: null,
                FulfilmentStatus: FulfilmentStatus.EMPTY,
                Updated: DateTime.UtcNow,
                UpdatedBy: null
            );
            phase.Tasks?.Add(newTask);
            return Results.Created($"{Base}/employee-checklists/{checklistId}/phases/{phaseId}/customtasks/{newTask.Id}", newTask);
        });

        app.MapPatch($"{Base}/employee-checklists/{{checklistId}}/customtasks/{{taskId}}", (
            string checklistId,
            string taskId,
            CustomTaskUpdateRequest req) =>
        {
            var checklist = MockStore.EmployeeChecklists.FirstOrDefault(c => c.Id == checklistId);
            if (checklist is null) return Results.NotFound();

            foreach (var phase in checklist.Phases ?? [])
            {
                var tasks = phase.Tasks;
                if (tasks is null) continue;
                var idx = tasks.FindIndex(t => t.Id == taskId && t.CustomTask == true);
                if (idx < 0) continue;

                var t = tasks[idx];
                var updated = t with
                {
                    Heading = req.Heading ?? t.Heading,
                    HeadingReference = req.HeadingReference ?? t.HeadingReference,
                    Text = req.Text ?? t.Text,
                    SortOrder = req.SortOrder ?? t.SortOrder,
                    RoleType = req.RoleType ?? t.RoleType,
                    QuestionType = req.QuestionType ?? t.QuestionType,
                    UpdatedBy = req.UpdatedBy ?? t.UpdatedBy,
                    Updated = DateTime.UtcNow
                };
                tasks[idx] = updated;
                return Results.Ok(new CustomTask(
                    updated.Id, updated.Heading, updated.HeadingReference, updated.Text,
                    updated.SortOrder, updated.RoleType, updated.QuestionType,
                    null, updated.Updated, updated.UpdatedBy
                ));
            }
            return Results.NotFound();
        });

        app.MapDelete($"{Base}/employee-checklists/{{checklistId}}/customtasks/{{taskId}}", (
            string checklistId,
            string taskId) =>
        {
            var checklist = MockStore.EmployeeChecklists.FirstOrDefault(c => c.Id == checklistId);
            if (checklist is null) return Results.NotFound();

            foreach (var phase in checklist.Phases ?? [])
            {
                var tasks = phase.Tasks;
                if (tasks is null) continue;
                var idx = tasks.FindIndex(t => t.Id == taskId && t.CustomTask == true);
                if (idx < 0) continue;
                tasks.RemoveAt(idx);
                return Results.NoContent();
            }
            return Results.NotFound();
        });

        app.MapPost($"{Base}/employee-checklists/{{checklistId}}/delegate-to/{{email}}", (
            string checklistId,
            string email) =>
        {
            var decoded = Uri.UnescapeDataString(email);
            var checklist = MockStore.EmployeeChecklists.FirstOrDefault(c => c.Id == checklistId);
            if (checklist is null) return Results.NotFound();

            if (checklist.DelegatedTo != null && !checklist.DelegatedTo.Contains(decoded))
                checklist.DelegatedTo.Add(decoded);

            return Results.NoContent();
        });

        // --- Checklist Templates ---

        app.MapGet($"{Base}/checklists", () =>
            Results.Ok(MockStore.ChecklistTemplates));

        app.MapGet($"{Base}/checklists/{{templateId}}", (string templateId) =>
        {
            var t = MockStore.ChecklistTemplates.FirstOrDefault(t => t.Id == templateId);
            return t is not null ? Results.Ok(t) : Results.NotFound();
        });

        app.MapPost($"{Base}/checklists", (ChecklistCreateRequest req) =>
        {
            var newTemplate = new ChecklistTemplate(
                Id: Guid.NewGuid().ToString(),
                Name: req.Name,
                DisplayName: req.DisplayName,
                Version: 1,
                LifeCycle: LifeCycle.CREATED,
                Created: DateTime.UtcNow,
                Updated: DateTime.UtcNow,
                LastSavedBy: req.CreatedBy,
                Phases: []
            );
            MockStore.ChecklistTemplates.Add(newTemplate);
            return Results.Created($"{Base}/checklists/{newTemplate.Id}", newTemplate);
        });

        app.MapMethods($"{Base}/checklists/{{templateId}}/activate", ["PATCH"], (string templateId) =>
        {
            var idx = MockStore.ChecklistTemplates.FindIndex(t => t.Id == templateId);
            if (idx < 0) return Results.NotFound();
            var updated = MockStore.ChecklistTemplates[idx] with
            {
                LifeCycle = LifeCycle.ACTIVE,
                Updated = DateTime.UtcNow
            };
            MockStore.ChecklistTemplates[idx] = updated;
            return Results.Ok(updated);
        });

        app.MapPost($"{Base}/checklists/{{templateId}}/version", (string templateId) =>
        {
            var original = MockStore.ChecklistTemplates.FirstOrDefault(t => t.Id == templateId);
            if (original is null) return Results.NotFound();
            var newVersion = original with
            {
                Id = Guid.NewGuid().ToString(),
                Version = (original.Version ?? 1) + 1,
                LifeCycle = LifeCycle.CREATED,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow
            };
            MockStore.ChecklistTemplates.Add(newVersion);
            return Results.Created($"{Base}/checklists/{newVersion.Id}", newVersion);
        });

        app.MapGet($"{Base}/checklists/{{templateId}}/events", (string templateId) =>
            Results.Ok(new Events(1, 25, 0, 0, 1, [])));

        // --- Template Task CRUD ---

        app.MapPost($"{Base}/checklists/{{templateId}}/phases/{{phaseId}}/tasks", (
            string templateId,
            string phaseId,
            TaskCreateRequest req) =>
        {
            var template = MockStore.ChecklistTemplates.FirstOrDefault(t => t.Id == templateId);
            if (template is null) return Results.NotFound();
            var phase = template.Phases?.FirstOrDefault(p => p.Id == phaseId);
            if (phase is null) return Results.NotFound();

            var newTask = new ChecklistTask(
                Id: Guid.NewGuid().ToString(),
                Heading: req.Heading,
                HeadingReference: req.HeadingReference,
                Text: req.Text,
                SortOrder: req.SortOrder,
                RoleType: req.RoleType,
                QuestionType: req.QuestionType,
                Permission: req.Permission,
                Created: DateTime.UtcNow,
                Updated: DateTime.UtcNow,
                LastSavedBy: req.CreatedBy,
                Optional: req.Optional
            );
            phase.Tasks?.Add(newTask);
            return Results.Created($"{Base}/checklists/{templateId}/phases/{phaseId}/tasks/{newTask.Id}", newTask);
        });

        app.MapMethods($"{Base}/checklists/{{templateId}}/phases/{{phaseId}}/tasks/{{taskId}}", ["PATCH"], (
            string templateId,
            string phaseId,
            string taskId,
            TaskUpdateRequest req) =>
        {
            var template = MockStore.ChecklistTemplates.FirstOrDefault(t => t.Id == templateId);
            if (template is null) return Results.NotFound();
            var phase = template.Phases?.FirstOrDefault(p => p.Id == phaseId);
            if (phase is null) return Results.NotFound();
            var tasks = phase.Tasks;
            if (tasks is null) return Results.NotFound();
            var idx = tasks.FindIndex(t => t.Id == taskId);
            if (idx < 0) return Results.NotFound();

            var t = tasks[idx];
            var updated = t with
            {
                Heading = req.Heading ?? t.Heading,
                HeadingReference = req.HeadingReference ?? t.HeadingReference,
                Text = req.Text ?? t.Text,
                SortOrder = req.SortOrder ?? t.SortOrder,
                RoleType = req.RoleType ?? t.RoleType,
                QuestionType = req.QuestionType ?? t.QuestionType,
                Permission = req.Permission ?? t.Permission,
                Optional = req.Optional ?? t.Optional,
                LastSavedBy = req.UpdatedBy ?? t.LastSavedBy,
                Updated = DateTime.UtcNow
            };
            tasks[idx] = updated;
            return Results.Ok(updated);
        });

        app.MapDelete($"{Base}/checklists/{{templateId}}/phases/{{phaseId}}/tasks/{{taskId}}", (
            string templateId,
            string phaseId,
            string taskId) =>
        {
            var template = MockStore.ChecklistTemplates.FirstOrDefault(t => t.Id == templateId);
            if (template is null) return Results.NotFound();
            var phase = template.Phases?.FirstOrDefault(p => p.Id == phaseId);
            if (phase is null) return Results.NotFound();
            var tasks = phase.Tasks;
            if (tasks is null) return Results.NotFound();
            var idx = tasks.FindIndex(t => t.Id == taskId);
            if (idx < 0) return Results.NotFound();
            tasks.RemoveAt(idx);
            return Results.NoContent();
        });

        // --- Sort Order ---

        app.MapPut($"{Base}/sortorder/{{orgId}}", (string orgId, SortorderRequest req) =>
            Results.Ok(req));

        // --- Organizations ---

        app.MapGet($"{Base}/organizations", (int? organizationFilter) =>
        {
            var orgs = MockStore.ChecklistOrganizations.AsEnumerable();
            if (organizationFilter.HasValue)
                orgs = orgs.Where(o => o.OrganizationNumber == organizationFilter.Value);
            return Results.Ok(orgs.ToList());
        });
    }
}
