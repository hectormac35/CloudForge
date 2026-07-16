using CloudForge.Application.Abstractions.Persistence;
using CloudForge.Domain.Deployments;

namespace CloudForge.Tests.Fakes;

internal sealed class FakeDeploymentRepository : IDeploymentRepository
{
    private readonly List<Deployment> _deployments = [];

    public Task AddAsync(
        Deployment deployment,
        CancellationToken cancellationToken = default)
    {
        _deployments.Add(deployment);
        return Task.CompletedTask;
    }

    public Task<Deployment?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var deployment = _deployments.SingleOrDefault(
            item => item.Id == id);

        return Task.FromResult(deployment);
    }

    public Task<IReadOnlyCollection<Deployment>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<Deployment> result =
            _deployments.AsReadOnly();

        return Task.FromResult(result);
    }
}
