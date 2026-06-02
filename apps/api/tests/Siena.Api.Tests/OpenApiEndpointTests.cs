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
}
