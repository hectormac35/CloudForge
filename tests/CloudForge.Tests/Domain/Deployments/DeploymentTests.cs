using CloudForge.Domain.Deployments;
using CloudForge.SharedKernel.Domain;

namespace CloudForge.Tests.Domain.Deployments;

public sealed class DeploymentTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesPendingDeployment()
    {
        var deployment = new Deployment(
            "CloudForge.Api",
            "1.0.0",
            DeploymentEnvironment.Development,
            "abc123",
            "main");

        Assert.NotEqual(Guid.Empty, deployment.Id);
        Assert.Equal("CloudForge.Api", deployment.ApplicationName);
        Assert.Equal("1.0.0", deployment.Version);
        Assert.Equal(
            DeploymentEnvironment.Development,
            deployment.Environment);
        Assert.Equal(DeploymentStatus.Pending, deployment.Status);
        Assert.Equal("abc123", deployment.CommitSha);
        Assert.Equal("main", deployment.Branch);
        Assert.Null(deployment.FinishedAt);
        Assert.Null(deployment.Duration);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithoutApplicationName_Throws(string name)
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            new Deployment(
                name,
                "1.0.0",
                DeploymentEnvironment.Development));

        Assert.Equal("applicationName", exception.ParamName);
    }

    [Fact]
    public void Start_WhenPending_ChangesStatusToRunning()
    {
        var deployment = CreateDeployment();

        deployment.Start();

        Assert.Equal(DeploymentStatus.Running, deployment.Status);
    }

    [Fact]
    public void MarkAsSucceeded_WhenRunning_CompletesDeployment()
    {
        var deployment = CreateDeployment();
        deployment.Start();

        deployment.MarkAsSucceeded();

        Assert.Equal(DeploymentStatus.Succeeded, deployment.Status);
        Assert.NotNull(deployment.FinishedAt);
        Assert.NotNull(deployment.Duration);
    }

    [Fact]
    public void MarkAsFailed_WhenNotRunning_Throws()
    {
        var deployment = CreateDeployment();

        Assert.Throws<DomainException>(
            deployment.MarkAsFailed);
    }

    [Fact]
    public void Cancel_WhenPending_CompletesAsCancelled()
    {
        var deployment = CreateDeployment();

        deployment.Cancel();

        Assert.Equal(DeploymentStatus.Cancelled, deployment.Status);
        Assert.NotNull(deployment.FinishedAt);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithoutVersion_Throws(string version)
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            new Deployment(
                "CloudForge.Api",
                version,
                DeploymentEnvironment.Development));

        Assert.Equal("version", exception.ParamName);
    }

    [Fact]
    public void Start_WhenAlreadyRunning_Throws()
    {
        var deployment = CreateDeployment();
        deployment.Start();

        Assert.Throws<DomainException>(
            deployment.Start);
    }

    [Fact]
    public void Cancel_WhenAlreadyCompleted_Throws()
    {
        var deployment = CreateDeployment();
        deployment.Start();
        deployment.MarkAsSucceeded();

        Assert.Throws<DomainException>(
            deployment.Cancel);
    }
    
    private static Deployment CreateDeployment()
    {
        return new Deployment(
            "CloudForge.Api",
            "1.0.0",
            DeploymentEnvironment.Development);
    }
}
