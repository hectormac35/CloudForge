var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

if (!app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.MapGet("/", () => Results.Ok(new
{
    service = "CloudForge.Api",
    status = "running",
    environment = app.Environment.EnvironmentName,
    timestamp = DateTimeOffset.UtcNow
}))
.WithName("GetServiceStatus");

app.MapHealthChecks("/health");

app.MapGet("/api/deployments", () => Results.Ok(new[]
{
    new
    {
        id = Guid.NewGuid(),
        application = "CloudForge.Api",
        environment = app.Environment.EnvironmentName,
        status = "Ready"
    }
}))
.WithName("GetDeployments");

app.Run();
