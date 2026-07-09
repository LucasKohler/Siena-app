using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Siena.Api.Tests;

public sealed class TrainingEndpointTests : IClassFixture<SienaWebApplicationFactory>
{
    private readonly HttpClient _client;

    public TrainingEndpointTests(SienaWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetNextTraining_ReturnsTreinoFisicoWithShape()
    {
        var token = await TestAuth.LoginAsync(_client, "+5511999990003");
        using var request = TestAuth.WithBearer(HttpMethod.Get, "/api/trainings/next", token);
        using var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);

        var training = document.RootElement;
        Assert.Equal("treino-fisico-2026-09-15", training.GetProperty("eventId").GetString());
        Assert.Equal("Treino Físico", training.GetProperty("title").GetString());
        Assert.Equal("Feminino", training.GetProperty("category").GetString());
        Assert.Equal("Centro de Treinamento", training.GetProperty("location").GetString());
        Assert.Equal(JsonValueKind.Array, training.GetProperty("confirmed").ValueKind);
    }

    [Fact]
    public async Task SetAttendance_AsAthlete_IsPendingUntilApproved()
    {
        var token = await TestAuth.LoginAsync(_client, "+5511999990003");

        using var postRequest = TestAuth.WithBearer(
            HttpMethod.Post,
            "/api/trainings/treino-fisico-2026-09-15/attendance",
            token);
        postRequest.Content = JsonContent.Create(new { status = "Eu vou" });

        using var postResponse = await _client.SendAsync(postRequest);
        Assert.Equal(HttpStatusCode.NoContent, postResponse.StatusCode);

        using var getRequest = TestAuth.WithBearer(HttpMethod.Get, "/api/trainings/next", token);

        using var getResponse = await _client.SendAsync(getRequest);
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        await using var stream = await getResponse.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);

        var training = document.RootElement;
        Assert.Equal("Eu vou", training.GetProperty("myStatus").GetString());
        Assert.Equal("Pendente", training.GetProperty("myApprovalStatus").GetString());
        Assert.Empty(training.GetProperty("confirmed").EnumerateArray());
    }

    [Fact]
    public async Task SetAttendance_WithoutToken_ReturnsUnauthorized()
    {
        using var response = await _client.PostAsJsonAsync(
            "/api/trainings/treino-fisico-2026-09-15/attendance",
            new { status = "Eu vou" });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task SetAttendance_AsCoach_ReturnsForbidden()
    {
        var token = await TestAuth.LoginAsync(_client, "+5511999990002");

        using var request = TestAuth.WithBearer(
            HttpMethod.Post,
            "/api/trainings/treino-fisico-2026-09-15/attendance",
            token);
        request.Content = JsonContent.Create(new { status = "Eu vou" });

        using var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task SetAttendance_OnNonTrainingEvent_ReturnsNotFound()
    {
        var token = await TestAuth.LoginAsync(_client, "+5511999990003");

        using var request = TestAuth.WithBearer(
            HttpMethod.Post,
            "/api/trainings/liga-nacional-minas-2026-03-15/attendance",
            token);
        request.Content = JsonContent.Create(new { status = "Eu vou" });

        using var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

}
