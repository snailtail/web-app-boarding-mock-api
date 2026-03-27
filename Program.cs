using System.Text.Json;
using System.Text.Json.Serialization;
using MockServer.Endpoints;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((doc, _, _) =>
    {
        doc.Info.Title = "Boarding Mock API";
        doc.Info.Description = "Mock-server för test av onboarding-system";
        doc.Info.Version = "v1";
        return Task.CompletedTask;
    });
});

// JSON: camelCase + enums som strängar (UPPERCASE) + skippa null-värden
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
});

// CORS: tillåt alla origins
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod()));

var app = builder.Build();
app.UseCors();

app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.Title = "Boarding Mock API";
    options.Theme = ScalarTheme.DeepSpace;
});

app.MapTokenEndpoints();
app.MapEmployeeEndpoints();
app.MapChecklistEndpoints();
app.MapCompanyEndpoints();

app.Run();
