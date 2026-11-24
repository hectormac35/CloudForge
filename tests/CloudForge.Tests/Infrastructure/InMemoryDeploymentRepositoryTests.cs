using CloudForge.Domain.Deployments;
using CloudForge.Infrastructure.Persistence;

namespace CloudForge.Tests.Infrastructure;

public sealed class InMemoryDeploymentRepositoryTests
{
    [Fact]
    public async Task AddYGetById_DevuelveElMismoDespliegue()
    {
        var repository = new InMemoryDeploymentRepository();
        var deployment = new Deployment("CloudForge.Api", "1.0.0", DeploymentEnvironment.Development);

        await repository.AddAsync(deployment);

        Assert.Same(deployment, await repository.GetByIdAsync(deployment.Id));
        Assert.Null(await repository.GetByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task Add_Duplicado_Lanza()
    {
        var repository = new InMemoryDeploymentRepository();
        var deployment = new Deployment("CloudForge.Api", "1.0.0", DeploymentEnvironment.Development);
        await repository.AddAsync(deployment);

        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.AddAsync(deployment));
    }

    [Fact]
    public async Task Update_DeInexistente_Lanza()
    {
        var repository = new InMemoryDeploymentRepository();
        var deployment = new Deployment("CloudForge.Api", "1.0.0", DeploymentEnvironment.Development);

        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.UpdateAsync(deployment));
    }

    [Fact]
    public async Task Add_Concurrente_GuardaTodos()
    {
        var repository = new InMemoryDeploymentRepository();

        await Task.WhenAll(Enumerable.Range(0, 200).Select(i =>
            repository.AddAsync(new Deployment("CloudForge.Api", $"1.0.{i}", DeploymentEnvironment.Development))));

        Assert.Equal(200, (await repository.GetAllAsync()).Count);
    }
}
