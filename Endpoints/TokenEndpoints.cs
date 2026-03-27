namespace MockServer.Endpoints;

public static class TokenEndpoints
{
    public static void MapTokenEndpoints(this WebApplication app)
    {
        app.MapPost("/token", () =>
            Results.Ok(new
            {
                access_token = $"mock-token-{Guid.NewGuid():N}",
                expires_in = 3600
            }));
    }
}
