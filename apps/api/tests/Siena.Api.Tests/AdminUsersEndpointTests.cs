using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Siena.Api.Tests;

public sealed class AdminUsersEndpointTests : IClassFixture<SienaWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AdminUsersEndpointTests(SienaWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateUser_AsCoach_ThenLoginSucceeds()
    {
        var staffToken = await TestAuth.LoginAsync(_client, "+5511999990002");

        using var createRequest = TestAuth.WithBearer(HttpMethod.Post, "/api/admin/users", staffToken);
        createRequest.Content = JsonContent.Create(new
        {
            id = "user-new-athlete",
            phoneNumber = "+5511988887777",
            displayName = "Novo Atleta",
            role = "Atleta",
            position = "Central"
        });

        using var createResponse = await _client.SendAsync(createRequest);
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        using var loginResponse = await _client.PostAsJsonAsync(
            "/api/auth/login",
            new { phoneNumber = "+5511988887777" });

        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

        await using var stream = await loginResponse.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);

        Assert.Equal("Novo Atleta", document.RootElement.GetProperty("displayName").GetString());
        Assert.Equal("Atleta", document.RootElement.GetProperty("role").GetString());
    }

    [Fact]
    public async Task CreateUser_AsAthlete_ReturnsForbidden()
    {
        var token = await TestAuth.LoginAsync(_client, "+5511999990003");

        using var request = TestAuth.WithBearer(HttpMethod.Post, "/api/admin/users", token);
        request.Content = JsonContent.Create(new
        {
            id = "user-forbidden",
            phoneNumber = "+5511988886666",
            displayName = "Forbidden",
            role = "Atleta",
            position = (string?)null
        });

        using var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task UpdateUser_AsStaff_ThenListReflectsChanges()
    {
        var staffToken = await TestAuth.LoginAsync(_client, "+5511999990001");

        using var updateRequest = TestAuth.WithBearer(
            HttpMethod.Put,
            "/api/admin/users/user-athlete-dev-1",
            staffToken);
        updateRequest.Content = JsonContent.Create(new
        {
            displayName = "Atleta DEV 1 Atualizado",
            role = "Atleta",
            position = "Líbero"
        });

        using var updateResponse = await _client.SendAsync(updateRequest);
        Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

        using var listRequest = TestAuth.WithBearer(
            HttpMethod.Get,
            "/api/admin/users?includeInactive=true",
            staffToken);

        using var listResponse = await _client.SendAsync(listRequest);
        Assert.Equal(HttpStatusCode.OK, listResponse.StatusCode);

        await using var stream = await listResponse.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);

        var user = document.RootElement.EnumerateArray()
            .First(item => item.GetProperty("id").GetString() == "user-athlete-dev-1");

        Assert.Equal("Atleta DEV 1 Atualizado", user.GetProperty("displayName").GetString());
        Assert.Equal("Líbero", user.GetProperty("position").GetString());
    }

    [Fact]
    public async Task SetUserActive_False_ThenLoginFails()
    {
        var staffToken = await TestAuth.LoginAsync(_client, "+5511999990001");

        using var deactivateRequest = TestAuth.WithBearer(
            HttpMethod.Patch,
            "/api/admin/users/user-athlete-dev-2/active",
            staffToken);
        deactivateRequest.Content = JsonContent.Create(new { isActive = false });

        using var deactivateResponse = await _client.SendAsync(deactivateRequest);
        Assert.Equal(HttpStatusCode.NoContent, deactivateResponse.StatusCode);

        using var loginResponse = await _client.PostAsJsonAsync(
            "/api/auth/login",
            new { phoneNumber = "+5511999990004" });

        Assert.Equal(HttpStatusCode.Unauthorized, loginResponse.StatusCode);
    }

    [Fact]
    public async Task UpdateUser_AsAthlete_ReturnsForbidden()
    {
        var token = await TestAuth.LoginAsync(_client, "+5511999990003");

        using var request = TestAuth.WithBearer(
            HttpMethod.Put,
            "/api/admin/users/user-athlete-dev-1",
            token);
        request.Content = JsonContent.Create(new
        {
            displayName = "Hack",
            role = "Atleta",
            position = "Central"
        });

        using var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}
