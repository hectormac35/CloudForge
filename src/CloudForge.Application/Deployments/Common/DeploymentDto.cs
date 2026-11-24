using CloudForge.Domain.Deployments;

namespace CloudForge.Application.Deployments.Common;

/// <summary>
/// Vista de un despliegue para las capas exteriores (API). Evita exponer la entidad del dominio.
/// </summary>
public sealed record DeploymentDto(
    Guid Id,
    string ApplicationName,
    string Version,
    DeploymentEnvironment Environment,
    DeploymentStatus Status,
    string? CommitSha,
    string? Branch,
    DateTimeOffset StartedAt,
    DateTimeOffset? FinishedAt,
    double? DurationSeconds)
{
    public static DeploymentDto FromDomain(Deployment deployment)
    {
        ArgumentNullException.ThrowIfNull(deployment);

        return new DeploymentDto(
            deployment.Id,
            deployment.ApplicationName,
            deployment.Version,
            deployment.Environment,
            deployment.Status,
            deployment.CommitSha,
            deployment.Branch,
            deployment.StartedAt,
            deployment.FinishedAt,
            deployment.Duration is { } duration
                ? Math.Round(duration.TotalSeconds, 3)
                : null);
    }
}
