using CloudForge.Application.Deployments.ChangeDeploymentStatus;
using CloudForge.Application.Deployments.Common;
using CloudForge.Application.Deployments.GetDeploymentById;
using CloudForge.Application.Deployments.GetDeployments;
using CloudForge.Application.Deployments.RegisterDeployment;
using CloudForge.Domain.Deployments;

namespace CloudForge.Api.Deployments;

public sealed record RegisterDeploymentRequest(
    string ApplicationName,
    string Version,
    DeploymentEnvironment Environment,
    string? CommitSha,
    string? Branch);

public static class DeploymentEndpoints
{
    public static IEndpointRouteBuilder MapDeploymentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/deployments").WithTags("Deployments");

        group.MapGet("/", async (
                DeploymentEnvironment? environment,
                DeploymentStatus? status,
                GetDeploymentsHandler handler,
                CancellationToken ct) =>
            TypedResults.Ok(await handler.HandleAsync(new GetDeploymentsQuery(environment, status), ct)))
            .WithName("GetDeployments")
            .WithSummary("Lista los despliegues, del más reciente al más antiguo")
            .WithDescription("Filtros opcionales: ?environment=Production&status=Running");

        group.MapGet("/{id:guid}", async (Guid id, GetDeploymentByIdHandler handler, CancellationToken ct) =>
                await handler.HandleAsync(id, ct) is { } deployment
                    ? Results.Ok(deployment)
                    : NotFound(id))
            .WithName("GetDeploymentById")
            .WithSummary("Obtiene un despliegue")
            .Produces<DeploymentDto>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapPost("/", async (RegisterDeploymentRequest request, RegisterDeploymentHandler handler, GetDeploymentByIdHandler query, CancellationToken ct) =>
            {
                var result = await handler.HandleAsync(new RegisterDeploymentCommand(
                    request.ApplicationName,
                    request.Version,
                    request.Environment,
                    request.CommitSha,
                    request.Branch), ct);

                var created = await query.HandleAsync(result.Id, ct);
                return Results.Created($"/api/deployments/{result.Id}", created);
            })
            .WithName("RegisterDeployment")
            .WithSummary("Registra un despliegue nuevo (estado Pending)")
            .Produces<DeploymentDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        MapAction(group, "start", DeploymentAction.Start, "Inicia un despliegue pendiente (Pending → Running)");
        MapAction(group, "succeed", DeploymentAction.Succeed, "Marca como correcto un despliegue en ejecución (Running → Succeeded)");
        MapAction(group, "fail", DeploymentAction.Fail, "Marca como fallido un despliegue en ejecución (Running → Failed)");
        MapAction(group, "cancel", DeploymentAction.Cancel, "Cancela un despliegue que aún no ha terminado");

        return app;
    }

    private static void MapAction(RouteGroupBuilder group, string route, DeploymentAction action, string summary)
    {
        group.MapPost($"/{{id:guid}}/{route}", async (Guid id, ChangeDeploymentStatusHandler handler, CancellationToken ct) =>
                await handler.HandleAsync(id, action, ct) is { } deployment
                    ? Results.Ok(deployment)
                    : NotFound(id))
            .WithName($"{action}Deployment")
            .WithSummary(summary)
            .Produces<DeploymentDto>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);
    }

    private static IResult NotFound(Guid id) =>
        Results.Problem(
            statusCode: StatusCodes.Status404NotFound,
            title: "Despliegue no encontrado.",
            detail: $"No existe ningún despliegue con el id {id}.");
}
