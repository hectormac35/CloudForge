using CloudForge.Application.Abstractions.Persistence;
using CloudForge.Domain.Deployments;

namespace CloudForge.Application.Deployments.RegisterDeployment;

public sealed class RegisterDeploymentHandler
{
    private readonly IDeploymentRepository _deploymentRepository;

    public RegisterDeploymentHandler(
        IDeploymentRepository deploymentRepository)
    {
        _deploymentRepository = deploymentRepository;
    }

    public async Task<RegisterDeploymentResult> HandleAsync(
        RegisterDeploymentCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var deployment = new Deployment(
            command.ApplicationName,
            command.Version,
            command.Environment,
            command.CommitSha,
            command.Branch);

        await _deploymentRepository.AddAsync(
            deployment,
            cancellationToken);

        return new RegisterDeploymentResult(
            deployment.Id,
            deployment.ApplicationName,
            deployment.Version,
            deployment.Environment,
            deployment.Status,
            deployment.CommitSha,
            deployment.Branch,
            deployment.StartedAt);
    }
}
