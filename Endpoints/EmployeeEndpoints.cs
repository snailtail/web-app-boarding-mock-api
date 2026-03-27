using MockServer.Data;

namespace MockServer.Endpoints;

public static class EmployeeEndpoints
{
    private const string Base = "/employee/2.0/2281/portalpersondata";

    public static void MapEmployeeEndpoints(this WebApplication app)
    {
        // OBS: PERSONAL/{username} måste registreras FÖRE /{email}
        app.MapGet($"{Base}/PERSONAL/{{username}}", (string username) =>
        {
            var p = MockStore.Employees.FirstOrDefault(e => e.LoginName == username);
            return p is not null ? Results.Ok(p) : Results.NotFound();
        });

        app.MapGet($"{Base}/{{email}}", (string email) =>
        {
            var decoded = Uri.UnescapeDataString(email);
            var p = MockStore.Employees.FirstOrDefault(e => e.Email == decoded);
            return p is not null ? Results.Ok(p) : Results.NotFound();
        });
    }
}
