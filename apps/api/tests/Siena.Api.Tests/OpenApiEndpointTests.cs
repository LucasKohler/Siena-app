using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Siena.Api.Tests;

public sealed class OpenApiEndpointTests : IClassFixture<SienaWebApplicationFactory>
{
    private readonly HttpClient _client;

    public OpenApiEndpointTests(SienaWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetOpenApiDocument_ReturnsApiDescription()
    {
        using var response = await _client.GetAsync("/openapi/v1.json");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("/api/health", content);
        Assert.Contains("/api/auth/login", content);
        Assert.Contains("/api/trainings/next", content);
        Assert.Contains("/api/events", content);
        Assert.Contains("/api/videos", content);
        Assert.Contains("/api/admin/events", content);
        Assert.Contains("/api/admin/users", content);
        Assert.Contains("/api/admin/trainings", content);
    }

    [Fact]
    public async Task GetOpenApiDocument_InProduction_ReturnsNotFound()
    {
        await using var factory = new SienaProductionWebApplicationFactory();
        using var client = factory.CreateClient();

        using var openApiResponse = await client.GetAsync("/openapi/v1.json");
        Assert.Equal(HttpStatusCode.NotFound, openApiResponse.StatusCode);

        using var scalarResponse = await client.GetAsync("/scalar/v1");
        Assert.Equal(HttpStatusCode.NotFound, scalarResponse.StatusCode);
    }
}
