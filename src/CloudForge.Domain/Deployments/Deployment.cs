using CloudForge.SharedKernel.Domain;

namespace CloudForge.Domain.Deployments;

public sealed class Deployment : Entity
{
    public string ApplicationName { get; private set; }

    public string Version { get; private set; }

    public DeploymentEnvironment Environment { get; private set; }

    public DeploymentStatus Status { get; private set; }

    public string? CommitSha { get; private set; }

    public string? Branch { get; private set; }

    public DateTimeOffset StartedAt { get; private set; }

    public DateTimeOffset? FinishedAt { get; private set; }

    public TimeSpan? Duration =>
        FinishedAt.HasValue
            ? FinishedAt.Value - StartedAt
            : null;

    private Deployment()
        : base(Guid.NewGuid())
    {
        ApplicationName = string.Empty;
        Version = string.Empty;
    }

    public Deployment(
        string applicationName,
        string version,
        DeploymentEnvironment environment,
        string? commitSha = null,
        string? branch = null)
        : base(Guid.NewGuid())
    {
        if (string.IsNullOrWhiteSpace(applicationName))
        {
            throw new ArgumentException(
                "El nombre de la aplicación es obligatorio.",
                nameof(applicationName));
        }

        if (string.IsNullOrWhiteSpace(version))
        {
            throw new ArgumentException(
                "La versión del despliegue es obligatoria.",
                nameof(version));
        }

        ApplicationName = applicationName.Trim();
        Version = version.Trim();
        Environment = environment;
        Status = DeploymentStatus.Pending;
        CommitSha = NormalizeOptionalValue(commitSha);
        Branch = NormalizeOptionalValue(branch);
        StartedAt = DateTimeOffset.UtcNow;
    }

    public void Start()
    {
        if (Status is not DeploymentStatus.Pending)
        {
            throw new DomainException(
                "Solo se puede iniciar un despliegue que esté pendiente.");
        }

        Status = DeploymentStatus.Running;
    }

    public void MarkAsSucceeded()
    {
        Complete(DeploymentStatus.Succeeded);
    }

    public void MarkAsFailed()
    {
        Complete(DeploymentStatus.Failed);
    }

    public void Cancel()
    {
        if (Status is DeploymentStatus.Succeeded
            or DeploymentStatus.Failed
            or DeploymentStatus.Cancelled)
        {
            throw new DomainException(
                "No se puede cancelar un despliegue que ya ha finalizado.");
        }

        Status = DeploymentStatus.Cancelled;
        FinishedAt = DateTimeOffset.UtcNow;
    }

    private void Complete(DeploymentStatus finalStatus)
    {
        if (Status is not DeploymentStatus.Running)
        {
            throw new DomainException(
                "Solo se puede completar un despliegue que esté en ejecución.");
        }

        Status = finalStatus;
        FinishedAt = DateTimeOffset.UtcNow;
    }

    private static string? NormalizeOptionalValue(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}