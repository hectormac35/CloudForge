using CloudForge.SharedKernel.Domain;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CloudForge.Api.Errors;

/// <summary>
/// Traduce los errores del dominio a respuestas HTTP con formato ProblemDetails (RFC 9457):
/// una regla de negocio incumplida es un 409 Conflict y un dato no válido es un 400 Bad Request.
/// El resto de errores siguen al manejador por defecto (500, sin detalles internos).
/// </summary>
public sealed class DomainExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetails;

    public DomainExceptionHandler(IProblemDetailsService problemDetails)
    {
        _problemDetails = problemDetails;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (status, title) = exception switch
        {
            DomainException => (StatusCodes.Status409Conflict, "La operación no está permitida en el estado actual."),
            ArgumentException => (StatusCodes.Status400BadRequest, "Los datos enviados no son válidos."),
            _ => (0, string.Empty)
        };

        if (status == 0)
        {
            return false;
        }

        httpContext.Response.StatusCode = status;

        return await _problemDetails.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = exception.Message
            }
        });
    }
}
