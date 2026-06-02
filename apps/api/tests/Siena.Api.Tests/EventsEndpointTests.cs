using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Siena.Api.Tests;

public sealed class EventsEndpointTests : IClassFixture<SienaWebApplicationFactory>
{
    private readonly HttpClient _client;

    public EventsEndpointTests(SienaWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetEvents_ReturnsInitialEventSummaries()
    {
        using var response = await _client.GetAsync("/api/events");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);

        var events = document.RootElement;
        Assert.Equal(JsonValueKind.Array, events.ValueKind);
        Assert.Equal(3, events.GetArrayLength());

        var ligaEvent = events.EnumerateArray()
            .Single(eventItem => eventItem.GetProperty("id").GetString() == "liga-nacional-minas-2026-03-15");

        Assert.Equal("Liga Nacional — Siena vs. Minas T.C.", ligaEvent.GetProperty("title").GetString());
        Assert.Equal("Liga Nacional", ligaEvent.GetProperty("type").GetString());
        Assert.Equal("Masculino", ligaEvent.GetProperty("category").GetString());
        Assert.Equal("Ginásio Principal", ligaEvent.GetProperty("location").GetString());
        Assert.Equal("Minas T.C.", ligaEvent.GetProperty("opponent").GetString());
        Assert.False(ligaEvent.TryGetProperty("description", out _));
    }

    [Fact]
    public async Task GetEventById_ReturnsEventDetail()
    {
        using var response = await _client.GetAsync("/api/events/liga-nacional-minas-2026-03-15");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);

        var eventItem = document.RootElement;
        Assert.Equal("liga-nacional-minas-2026-03-15", eventItem.GetProperty("id").GetString());
        Assert.Equal("Liga Nacional — Siena vs. Minas T.C.", eventItem.GetProperty("title").GetString());
        Assert.Equal("Liga Nacional", eventItem.GetProperty("type").GetString());
        Assert.Equal("Masculino", eventItem.GetProperty("category").GetString());
        Assert.True(eventItem.TryGetProperty("description", out var description));
        Assert.False(string.IsNullOrWhiteSpace(description.GetString()));
    }

    [Fact]
    public async Task GetEventById_WhenIdDoesNotExist_ReturnsNotFound()
    {
        using var response = await _client.GetAsync("/api/events/inexistente");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
