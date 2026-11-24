using CloudForge.Application.Abstractions.Persistence;
using CloudForge.Application.Deployments.Common;
using CloudForge.Domain.Deployments;

namespace CloudForge.Application.Deployments.GetDeployments;

public sealed record GetDeploymentsQuery(
    DeploymentEnvironment? Environment = null,
    DeploymentStatus? Status = null);

public sealed class GetDeploymentsHandler
{
    private readonly IDeploymentRepository _deploymentRepository;

    public GetDeploymentsHandler(IDeploymentRepository deploymentRepository)
    {
        _deploymentRepository = deploymentRepository;
    }

    /// <summary>
    /// Devuelve los despliegues, del más reciente al más antiguo, filtrados opcionalmente por entorno y estado.
    /// </summary>
    public async Task<IReadOnlyList<DeploymentDto>> HandleAsync(
        GetDeploymentsQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        var deployments = await _deploymentRepository.GetAllAsync(cancellationToken);

        return deployments
            .Where(d => query.Environment is null || d.Environment == query.Environment)
            .Where(d => query.Status is null || d.Status == query.Status)
            .OrderByDescending(d => d.StartedAt)
            .Select(DeploymentDto.FromDomain)
            .ToList();
    }
}
