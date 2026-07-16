using CloudForge.Domain.Deployments;

namespace CloudForge.Application.Deployments.RegisterDeployment;

public sealed record RegisterDeploymentResult(
    Guid Id,
    string ApplicationName,
    string Version,
    DeploymentEnvironment Environment,
    DeploymentStatus Status,
    string? CommitSha,
    string? Branch,
    DateTimeOffset StartedAt);
