using CloudForge.Application.Abstractions.Persistence;
using CloudForge.Application.Deployments.Common;

namespace CloudForge.Application.Deployments.GetDeploymentById;

public sealed class GetDeploymentByIdHandler
{
    private readonly IDeploymentRepository _deploymentRepository;

    public GetDeploymentByIdHandler(IDeploymentRepository deploymentRepository)
    {
        _deploymentRepository = deploymentRepository;
    }

    /// <returns>El despliegue, o null si no existe.</returns>
    public async Task<DeploymentDto?> HandleAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var deployment = await _deploymentRepository.GetByIdAsync(id, cancellationToken);

        return deployment is null
            ? null
            : DeploymentDto.FromDomain(deployment);
    }
}
