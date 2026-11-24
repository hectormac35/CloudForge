using System.Reflection;
using System.Text.Json.Serialization;
using CloudForge.Api.Deployments;
using CloudForge.Api.Errors;
using CloudForge.Application.Abstractions.Persistence;
using CloudForge.Application.Deployments.ChangeDeploymentStatus;
using CloudForge.Application.Deployments.GetDeploymentById;
using CloudForge.Application.Deployments.GetDeployments;
using CloudForge.Application.Deployments.RegisterDeployment;
using CloudForge.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

// Errores con formato ProblemDetails (RFC 9457) y traducción de las excepciones del dominio
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<DomainExceptionHandler>();

// Enums como texto en el JSON ("Production" en vez de 3)
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Infraestructura: repositorio en memoria (singleton: los datos viven mientras viva la app)
builder.Services.AddSingleton<IDeploymentRepository, InMemoryDeploymentRepository>();

// Casos de uso
builder.Services.AddScoped<RegisterDeploymentHandler>();
builder.Services.AddScoped<GetDeploymentsHandler>();
builder.Services.AddScoped<GetDeploymentByIdHandler>();
builder.Services.AddScoped<ChangeDeploymentStatusHandler>();

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

if (!app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

var version = Assembly.GetExecutingAssembly()
    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
    .Split('+')[0] ?? "1.0.0";

app.MapGet("/", () => Results.Ok(new
{
    service = "CloudForge.Api",
    version,
    status = "running",
    environment = app.Environment.EnvironmentName,
    timestamp = DateTimeOffset.UtcNow
}))
.WithName("GetServiceStatus")
.ExcludeFromDescription();

app.MapHealthChecks("/health");

app.MapDeploymentEndpoints();

app.Run();

// Permite a los tests de integración arrancar la API con WebApplicationFactory<Program>
public partial class Program { }
