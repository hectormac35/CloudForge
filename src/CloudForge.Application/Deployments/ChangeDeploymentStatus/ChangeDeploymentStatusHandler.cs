using CloudForge.Application.Abstractions.Persistence;
using CloudForge.Application.Deployments.Common;
using CloudForge.Domain.Deployments;

namespace CloudForge.Application.Deployments.ChangeDeploymentStatus;

/// <summary>Acciones del ciclo de vida de un despliegue.</summary>
public enum DeploymentAction
{
    Start,
    Succeed,
    Fail,
    Cancel
}

public sealed class ChangeDeploymentStatusHandler
{
    private readonly IDeploymentRepository _deploymentRepository;

    public ChangeDeploymentStatusHandler(IDeploymentRepository deploymentRepository)
    {
        _deploymentRepository = deploymentRepository;
    }

    /// <summary>
    /// Aplica la acción sobre el despliegue. Las reglas de qué transición es válida viven en el dominio:
    /// si no se permite, el dominio lanza una DomainException.
    /// </summary>
    /// <returns>El despliegue actualizado, o null si no existe.</returns>
    public async Task<DeploymentDto?> HandleAsync(
        Guid id,
        DeploymentAction action,
        CancellationToken cancellationToken = default)
    {
        var deployment = await _deploymentRepository.GetByIdAsync(id, cancellationToken);
        if (deployment is null)
        {
            return null;
        }

        switch (action)
        {
            case DeploymentAction.Start:
                deployment.Start();
                break;
            case DeploymentAction.Succeed:
                deployment.MarkAsSucceeded();
                break;
            case DeploymentAction.Fail:
                deployment.MarkAsFailed();
                break;
            case DeploymentAction.Cancel:
                deployment.Cancel();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(action), action, "Acción no soportada.");
        }

        await _deploymentRepository.UpdateAsync(deployment, cancellationToken);

        return DeploymentDto.FromDomain(deployment);
    }
}
