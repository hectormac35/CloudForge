using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using CloudForge.Application.Deployments.Common;
using CloudForge.Domain.Deployments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CloudForge.Tests.Api;

/// <summary>Tests de integración: arrancan la API completa en memoria y la llaman por HTTP.</summary>
public sealed class DeploymentsApiTests
{
    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private static HttpClient CrearCliente() => new WebApplicationFactory<Program>().CreateClient();

    private static async Task<DeploymentDto> RegistrarAsync(HttpClient client, string environment = "Staging")
    {
        var response = await client.PostAsJsonAsync("/api/deployments", new
        {
            applicationName = "CloudForge.Api",
            version = "1.4.0",
            environment,
            commitSha = "a1b2c3d",
            branch = "main"
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        return (await response.Content.ReadFromJsonAsync<DeploymentDto>(Json))!;
    }

    [Fact]
    public async Task Health_Responde200()
    {
        var response = await CrearCliente().GetAsync("/health");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Registrar_Devuelve201ConLocationYEstadoPending()
    {
        var client = CrearCliente();
        var response = await client.PostAsJsonAsync("/api/deployments", new
        {
            applicationName = "CloudForge.Api",
            version = "1.4.0",
            environment = "Production"
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("\"status\":\"Pending\"", body);          // enums como texto
        Assert.Contains("\"environment\":\"Production\"", body);
        Assert.StartsWith("/api/deployments/", response.Headers.Location!.ToString());
    }

    [Fact]
    public async Task CicloDeVidaCompleto_PorHttp()
    {
        var client = CrearCliente();
        var creado = await RegistrarAsync(client);

        var start = await client.PostAsync($"/api/deployments/{creado.Id}/start", null);
        var succeed = await client.PostAsync($"/api/deployments/{creado.Id}/succeed", null);
        var final = await client.GetFromJsonAsync<DeploymentDto>($"/api/deployments/{creado.Id}", Json);

        Assert.Equal(HttpStatusCode.OK, start.StatusCode);
        Assert.Equal(HttpStatusCode.OK, succeed.StatusCode);
        Assert.Equal(DeploymentStatus.Succeeded, final!.Status);
        Assert.NotNull(final.DurationSeconds);
    }

    [Fact]
    public async Task TransicionNoPermitida_Devuelve409ProblemDetails()
    {
        var client = CrearCliente();
        var creado = await RegistrarAsync(client);

        var response = await client.PostAsync($"/api/deployments/{creado.Id}/succeed", null);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.Equal(409, problem!.Status);
        Assert.Contains("en ejecución", problem.Detail);
    }

    [Fact]
    public async Task DatosNoValidos_Devuelve400()
    {
        var response = await CrearCliente().PostAsJsonAsync("/api/deployments", new
        {
            applicationName = "",
            version = "1.0.0",
            environment = "Development"
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task SinEntorno_Devuelve400()
    {
        var response = await CrearCliente().PostAsJsonAsync("/api/deployments", new
        {
            applicationName = "CloudForge.Api",
            version = "1.0.0"
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DespliegueInexistente_Devuelve404()
    {
        var client = CrearCliente();

        Assert.Equal(HttpStatusCode.NotFound, (await client.GetAsync($"/api/deployments/{Guid.NewGuid()}")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await client.PostAsync($"/api/deployments/{Guid.NewGuid()}/start", null)).StatusCode);
    }

    [Fact]
    public async Task Listar_FiltraPorEntorno()
    {
        var client = CrearCliente();
        await RegistrarAsync(client, "Production");
        await RegistrarAsync(client, "Staging");

        var produccion = await client.GetFromJsonAsync<List<DeploymentDto>>("/api/deployments?environment=Production", Json);

        Assert.All(produccion!, d => Assert.Equal(DeploymentEnvironment.Production, d.Environment));
        Assert.Single(produccion!);
    }
}
