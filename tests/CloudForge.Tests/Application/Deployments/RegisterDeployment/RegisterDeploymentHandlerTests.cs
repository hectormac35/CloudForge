using CloudForge.Application.Deployments.RegisterDeployment;
using CloudForge.Domain.Deployments;
using CloudForge.Tests.Fakes;

namespace CloudForge.Tests.Application.Deployments.RegisterDeployment;

public sealed class RegisterDeploymentHandlerTests
{
    [Fact]
    public async Task HandleAsync_DatosValidos_RegistraElDespliegue()
    {
        var repository = new FakeDeploymentRepository();
        var handler = new RegisterDeploymentHandler(repository);

        var command = new RegisterDeploymentCommand(
            "CloudForge.Api",
            "1.0.0",
            DeploymentEnvironment.Development,
            "abc123",
            "main");

        var result = await handler.HandleAsync(command);

        var savedDeployment = await repository.GetByIdAsync(result.Id);

        Assert.NotNull(savedDeployment);
        Assert.Equal("CloudForge.Api", result.ApplicationName);
        Assert.Equal("1.0.0", result.Version);
        Assert.Equal(DeploymentStatus.Pending, result.Status);
        Assert.Equal("abc123", result.CommitSha);
        Assert.Equal("main", result.Branch);
    }

    [Fact]
    public async Task HandleAsync_NombreVacio_LanzaExcepcion()
    {
        var repository = new FakeDeploymentRepository();
        var handler = new RegisterDeploymentHandler(repository);

        var command = new RegisterDeploymentCommand(
            string.Empty,
            "1.0.0",
            DeploymentEnvironment.Development,
            null,
            null);

        await Assert.ThrowsAsync<ArgumentException>(
            () => handler.HandleAsync(command));
    }

    [Fact]
    public async Task HandleAsync_ComandoNulo_LanzaExcepcion()
    {
        var repository = new FakeDeploymentRepository();
        var handler = new RegisterDeploymentHandler(repository);

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => handler.HandleAsync(null!));
    }
}
