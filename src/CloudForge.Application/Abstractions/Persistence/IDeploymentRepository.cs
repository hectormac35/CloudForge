using CloudForge.Domain.Deployments;

namespace CloudForge.Application.Abstractions.Persistence;

public interface IDeploymentRepository
{
    Task AddAsync(
        Deployment deployment,
        CancellationToken cancellationToken = default);

    Task<Deployment?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Deployment>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        Deployment deployment,
        CancellationToken cancellationToken = default);
}
