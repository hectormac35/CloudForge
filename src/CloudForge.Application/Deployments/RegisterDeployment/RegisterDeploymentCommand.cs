using CloudForge.Domain.Deployments;

namespace CloudForge.Application.Deployments.RegisterDeployment;

public sealed record RegisterDeploymentCommand(
    string ApplicationName,
    string Version,
    DeploymentEnvironment Environment,
    string? CommitSha,
    string? Branch);
