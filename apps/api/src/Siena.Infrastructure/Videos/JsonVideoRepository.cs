using System.Text.Json;
using Siena.Application.Videos;
using Siena.Domain.Videos;

namespace Siena.Infrastructure.Videos;

public sealed class JsonVideoRepository : IVideoRepository
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);
    private readonly string _dataPath;

    public JsonVideoRepository()
    {
        _dataPath = Path.Combine(AppContext.BaseDirectory, "Data", "videos.json");
    }

    public async Task<IReadOnlyCollection<Video>> ListAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(_dataPath))
        {
            throw new FileNotFoundException("Video data file was not found.", _dataPath);
        }

        await using var stream = File.OpenRead(_dataPath);
        var records = await JsonSerializer.DeserializeAsync<VideoRecord[]>(
            stream,
            SerializerOptions,
            cancellationToken);

        return records?
            .Select(ToDomain)
            .ToArray() ?? [];
    }

    private static Video ToDomain(VideoRecord record)
    {
        return new Video(
            record.Id,
            record.Title,
            record.Url,
            record.DurationSeconds,
            record.PublishedAt,
            record.Views);
    }
}

internal sealed record VideoRecord(
    string Id,
    string Title,
    string Url,
    int DurationSeconds,
    DateTimeOffset PublishedAt,
    int Views);
