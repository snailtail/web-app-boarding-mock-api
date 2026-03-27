using MockServer.Data;

namespace MockServer.Endpoints;

public static class CompanyEndpoints
{
    private const string Base = "/company/1.0/2281";

    public static void MapCompanyEndpoints(this WebApplication app)
    {
        app.MapGet($"{Base}/{{orgId}}/orgtree", (int orgId) =>
        {
            return MockStore.OrgTrees.TryGetValue(orgId, out var tree)
                ? Results.Ok(tree)
                : Results.NotFound();
        });

        app.MapGet($"{Base}/{{companyId}}/orgnodes", (int companyId) =>
        {
            var nodes = MockStore.CompanyOrgs
                .Where(o => o.CompanyId == companyId)
                .ToList();
            return Results.Ok(nodes);
        });
    }
}
