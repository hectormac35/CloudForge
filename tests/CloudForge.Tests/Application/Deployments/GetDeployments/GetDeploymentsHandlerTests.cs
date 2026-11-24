using CloudForge.Application.Deployments.GetDeployments;
using CloudForge.Domain.Deployments;
using CloudForge.Tests.Fakes;

namespace CloudForge.Tests.Application.Deployments.GetDeployments;

public sealed class GetDeploymentsHandlerTests
{
    [Fact]
    public async Task HandleAsync_FiltraPorEntornoYEstado()
    {
        var repository = new FakeDeploymentRepository();
        var produccion = new Deployment("CloudForge.Api", "2.0.0", DeploymentEnvironment.Production);
        produccion.Start();
        await repository.AddAsync(produccion);
        await repository.AddAsync(new Deployment("CloudForge.Api", "2.0.0", DeploymentEnvironment.Staging));
        await repository.AddAsync(new Deployment("CloudForge.Api", "1.9.0", DeploymentEnvironment.Production));

        var handler = new GetDeploymentsHandler(repository);

        var todos = await handler.HandleAsync(new GetDeploymentsQuery());
        var enProduccion = await handler.HandleAsync(new GetDeploymentsQuery(DeploymentEnvironment.Production));
        var enCurso = await handler.HandleAsync(new GetDeploymentsQuery(DeploymentEnvironment.Production, DeploymentStatus.Running));

        Assert.Equal(3, todos.Count);
        Assert.Equal(2, enProduccion.Count);
        Assert.Equal(produccion.Id, Assert.Single(enCurso).Id);
    }

    [Fact]
    public async Task HandleAsync_OrdenaDelMasRecienteAlMasAntiguo()
    {
        var repository = new FakeDeploymentRepository();
        var primero = new Deployment("CloudForge.Api", "1.0.0", DeploymentEnvironment.Development);
        await Task.Delay(5);
        var segundo = new Deployment("CloudForge.Api", "1.1.0", DeploymentEnvironment.Development);
        await repository.AddAsync(primero);
        await repository.AddAsync(segundo);

        var resultado = await new GetDeploymentsHandler(repository).HandleAsync(new GetDeploymentsQuery());

        Assert.Equal([segundo.Id, primero.Id], resultado.Select(d => d.Id));
    }
}
