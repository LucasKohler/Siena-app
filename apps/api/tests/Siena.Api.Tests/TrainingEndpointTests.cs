using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Siena.Api.Tests;

public sealed class TrainingEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TrainingEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetNextTraining_ReturnsTreinoFisicoWithShape()
    {
        using var request = await CreateAuthenticatedRequestAsync("+5511999990003");
        using var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);

        var training = document.RootElement;
        Assert.Equal("treino-fisico-2026-06-15", training.GetProperty("eventId").GetString());
        Assert.Equal("Treino Físico", training.GetProperty("title").GetString());
        Assert.Equal("Feminino", training.GetProperty("category").GetString());
        Assert.Equal("Centro de Treinamento", training.GetProperty("location").GetString());
        Assert.Equal(JsonValueKind.Array, training.GetProperty("confirmed").ValueKind);
    }

    [Fact]
    public async Task SetAttendance_AsAthlete_AppearsInConfirmedList()
    {
        var token = await LoginAsync("+5511999990003");

        using var postRequest = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/trainings/treino-fisico-2026-06-15/attendance");
        postRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        postRequest.Content = JsonContent.Create(new { status = "Eu vou" });

        using var postResponse = await _client.SendAsync(postRequest);
        Assert.Equal(HttpStatusCode.NoContent, postResponse.StatusCode);

        using var getRequest = new HttpRequestMessage(HttpMethod.Get, "/api/trainings/next");
        getRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var getResponse = await _client.SendAsync(getRequest);
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        await using var stream = await getResponse.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);

        var training = document.RootElement;
        Assert.Equal("Eu vou", training.GetProperty("myStatus").GetString());

        var confirmed = training.GetProperty("confirmed");
        Assert.Contains(
            confirmed.EnumerateArray(),
            attendee => attendee.GetProperty("displayName").GetString() == "Atleta DEV 1");
    }

    [Fact]
    public async Task SetAttendance_WithoutToken_ReturnsUnauthorized()
    {
        using var response = await _client.PostAsJsonAsync(
            "/api/trainings/treino-fisico-2026-06-15/attendance",
            new { status = "Eu vou" });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task SetAttendance_AsCoach_ReturnsForbidden()
    {
        var token = await LoginAsync("+5511999990002");

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/trainings/treino-fisico-2026-06-15/attendance");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(new { status = "Eu vou" });

        using var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task SetAttendance_OnNonTrainingEvent_ReturnsNotFound()
    {
        var token = await LoginAsync("+5511999990003");

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/trainings/liga-nacional-minas-2026-03-15/attendance");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(new { status = "Eu vou" });

        using var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private async Task<HttpRequestMessage> CreateAuthenticatedRequestAsync(string phoneNumber)
    {
        var token = await LoginAsync(phoneNumber);
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/trainings/next");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return request;
    }

    private async Task<string> LoginAsync(string phoneNumber)
    {
        using var response = await _client.PostAsJsonAsync(
            "/api/auth/login",
            new { phoneNumber });

        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);
        var token = document.RootElement.GetProperty("token").GetString();

        return token ?? throw new InvalidOperationException("Login did not return a token.");
    }
}
