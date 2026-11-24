using CloudForge.Application.Deployments.ChangeDeploymentStatus;
using CloudForge.Domain.Deployments;
using CloudForge.SharedKernel.Domain;
using CloudForge.Tests.Fakes;

namespace CloudForge.Tests.Application.Deployments.ChangeDeploymentStatus;

public sealed class ChangeDeploymentStatusHandlerTests
{
    private static async Task<(ChangeDeploymentStatusHandler Handler, Guid Id)> CrearDespliegueAsync()
    {
        var repository = new FakeDeploymentRepository();
        var deployment = new Deployment("CloudForge.Api", "1.2.0", DeploymentEnvironment.Staging);
        await repository.AddAsync(deployment);
        return (new ChangeDeploymentStatusHandler(repository), deployment.Id);
    }

    [Fact]
    public async Task HandleAsync_CicloCompleto_TerminaCorrectoConDuracion()
    {
        var (handler, id) = await CrearDespliegueAsync();

        var running = await handler.HandleAsync(id, DeploymentAction.Start);
        var succeeded = await handler.HandleAsync(id, DeploymentAction.Succeed);

        Assert.Equal(DeploymentStatus.Running, running!.Status);
        Assert.Equal(DeploymentStatus.Succeeded, succeeded!.Status);
        Assert.NotNull(succeeded.FinishedAt);
        Assert.NotNull(succeeded.DurationSeconds);
    }

    [Fact]
    public async Task HandleAsync_DespliegueInexistente_DevuelveNull()
    {
        var (handler, _) = await CrearDespliegueAsync();

        Assert.Null(await handler.HandleAsync(Guid.NewGuid(), DeploymentAction.Start));
    }

    [Fact]
    public async Task HandleAsync_TransicionNoPermitida_LanzaDomainException()
    {
        var (handler, id) = await CrearDespliegueAsync();

        // No se puede marcar como correcto un despliegue que no ha empezado
        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(id, DeploymentAction.Succeed));
    }

    [Fact]
    public async Task HandleAsync_CancelarPendiente_TerminaCancelado()
    {
        var (handler, id) = await CrearDespliegueAsync();

        var cancelled = await handler.HandleAsync(id, DeploymentAction.Cancel);

        Assert.Equal(DeploymentStatus.Cancelled, cancelled!.Status);
    }
}
