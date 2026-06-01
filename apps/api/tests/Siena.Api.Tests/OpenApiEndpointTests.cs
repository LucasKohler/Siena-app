using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Siena.Api.Tests;

public sealed class OpenApiEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public OpenApiEndpointTests(WebApplicationFactory<Program> factory)
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
        Assert.Contains("/api/events", content);
        Assert.Contains("/api/videos", content);
    }
}
