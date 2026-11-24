using System.Collections.Concurrent;
using CloudForge.Application.Abstractions.Persistence;
using CloudForge.Domain.Deployments;

namespace CloudForge.Infrastructure.Persistence;

/// <summary>
/// Repositorio en memoria, seguro para peticiones concurrentes.
/// Implementa la misma interfaz que usaría uno con base de datos, así que se puede sustituir
/// (por ejemplo por Azure Table Storage o EF Core) sin tocar el dominio ni los casos de uso.
/// </summary>
public sealed class InMemoryDeploymentRepository : IDeploymentRepository
{
    private readonly ConcurrentDictionary<Guid, Deployment> _deployments = new();

    public Task AddAsync(Deployment deployment, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(deployment);

        if (!_deployments.TryAdd(deployment.Id, deployment))
        {
            throw new InvalidOperationException($"Ya existe un despliegue con el id {deployment.Id}.");
        }

        return Task.CompletedTask;
    }

    public Task<Deployment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _deployments.TryGetValue(id, out var deployment);
        return Task.FromResult(deployment);
    }

    public Task<IReadOnlyCollection<Deployment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<Deployment> result = _deployments.Values.ToList();
        return Task.FromResult(result);
    }

    public Task UpdateAsync(Deployment deployment, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(deployment);

        if (!_deployments.ContainsKey(deployment.Id))
        {
            throw new InvalidOperationException($"No existe ningún despliegue con el id {deployment.Id}.");
        }

        _deployments[deployment.Id] = deployment;
        return Task.CompletedTask;
    }
}
