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
}
