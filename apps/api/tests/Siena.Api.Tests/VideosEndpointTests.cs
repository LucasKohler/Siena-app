using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Siena.Api.Tests;

public sealed class VideosEndpointTests : IClassFixture<SienaWebApplicationFactory>
{
    private readonly HttpClient _client;

    public VideosEndpointTests(SienaWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetVideos_ReturnsInitialVideoSummaries()
    {
        using var response = await _client.GetAsync("/api/videos");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);

        var videos = document.RootElement;
        Assert.Equal(JsonValueKind.Array, videos.ValueKind);
        Assert.Equal(3, videos.GetArrayLength());

        var ligaVideo = videos.EnumerateArray()
            .Single(video => video.GetProperty("id").GetString() == "video-liga-nacional-2026");

        Assert.Equal("Melhores momentos — Liga Nacional 2026", ligaVideo.GetProperty("title").GetString());
        Assert.Equal("https://example.com/videos/liga-nacional-2026", ligaVideo.GetProperty("url").GetString());
        Assert.Equal(765, ligaVideo.GetProperty("durationSeconds").GetInt32());
        Assert.Equal(128, ligaVideo.GetProperty("views").GetInt32());
    }
}
