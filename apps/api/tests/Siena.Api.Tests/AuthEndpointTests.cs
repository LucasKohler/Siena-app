using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Siena.Api.Tests;

public sealed class AuthEndpointTests : IClassFixture<SienaWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthEndpointTests(SienaWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_WithAllowedPhone_ReturnsToken()
    {
        using var response = await _client.PostAsJsonAsync(
            "/api/auth/login",
            new { phoneNumber = "+5511999990001" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);

        var root = document.RootElement;
        Assert.False(string.IsNullOrWhiteSpace(root.GetProperty("token").GetString()));
        Assert.Equal("Admin DEV", root.GetProperty("displayName").GetString());
        Assert.Equal("Administrador", root.GetProperty("role").GetString());
    }

    [Fact]
    public async Task Login_WithUnknownPhone_ReturnsUnauthorized()
    {
        using var response = await _client.PostAsJsonAsync(
            "/api/auth/login",
            new { phoneNumber = "+5511999999999" });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetCurrentUser_WithoutToken_ReturnsUnauthorized()
    {
        using var response = await _client.GetAsync("/api/auth/me");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetCurrentUser_WithValidToken_ReturnsCurrentUser()
    {
        using var loginResponse = await _client.PostAsJsonAsync(
            "/api/auth/login",
            new { phoneNumber = "+5511999990003" });

        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

        await using var loginStream = await loginResponse.Content.ReadAsStreamAsync();
        using var loginDocument = await JsonDocument.ParseAsync(loginStream);
        var token = loginDocument.RootElement.GetProperty("token").GetString();
        Assert.False(string.IsNullOrWhiteSpace(token));

        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/auth/me");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);

        var user = document.RootElement;
        Assert.Equal("user-athlete-dev-1", user.GetProperty("id").GetString());
        Assert.Equal("Atleta DEV 1", user.GetProperty("displayName").GetString());
        Assert.Equal("Atleta", user.GetProperty("role").GetString());
    }
}
