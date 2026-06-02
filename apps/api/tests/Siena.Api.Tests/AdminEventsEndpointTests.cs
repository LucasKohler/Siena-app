using System.Net;
using System.Net.Http.Json;

namespace Siena.Api.Tests;

public sealed class AdminEventsEndpointTests : IClassFixture<SienaWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AdminEventsEndpointTests(SienaWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateEvent_AsAdmin_ReturnsCreated()
    {
        var token = await TestAuth.LoginAsync(_client, "+5511999990001");

        using var request = TestAuth.WithBearer(HttpMethod.Post, "/api/admin/events", token);
        request.Content = JsonContent.Create(new
        {
            id = "treino-fisico-2026-07-01",
            title = "Treino Extra",
            type = "Treino Físico",
            category = "Masculino",
            startsAt = "2026-07-01T08:00:00-03:00",
            location = "Ginásio",
            opponent = (string?)null,
            description = "Treino administrativo"
        });

        using var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task CreateEvent_AsAthlete_ReturnsForbidden()
    {
        var token = await TestAuth.LoginAsync(_client, "+5511999990003");

        using var request = TestAuth.WithBearer(HttpMethod.Post, "/api/admin/events", token);
        request.Content = JsonContent.Create(new
        {
            id = "event-athlete-forbidden",
            title = "Forbidden",
            type = "Amistoso",
            category = "Feminino",
            startsAt = "2026-08-01T20:00:00-03:00",
            location = "Fora",
            opponent = "Rival",
            description = (string?)null
        });

        using var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task DeleteEvent_WithAttendances_ReturnsConflict()
    {
        var athleteToken = await TestAuth.LoginAsync(_client, "+5511999990003");

        using var attendanceRequest = TestAuth.WithBearer(
            HttpMethod.Post,
            "/api/trainings/treino-fisico-2026-06-15/attendance",
            athleteToken);
        attendanceRequest.Content = JsonContent.Create(new { status = "Eu vou" });
        using var attendanceResponse = await _client.SendAsync(attendanceRequest);
        Assert.Equal(HttpStatusCode.NoContent, attendanceResponse.StatusCode);

        var adminToken = await TestAuth.LoginAsync(_client, "+5511999990001");

        using var deleteRequest = TestAuth.WithBearer(
            HttpMethod.Delete,
            "/api/admin/events/treino-fisico-2026-06-15",
            adminToken);

        using var deleteResponse = await _client.SendAsync(deleteRequest);

        Assert.Equal(HttpStatusCode.Conflict, deleteResponse.StatusCode);
    }
}
