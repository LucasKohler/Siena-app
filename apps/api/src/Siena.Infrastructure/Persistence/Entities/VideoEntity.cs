namespace Siena.Infrastructure.Persistence.Entities;

public sealed class VideoEntity
{
    public string Id { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public int DurationSeconds { get; set; }

    public DateTimeOffset PublishedAt { get; set; }

    public int Views { get; set; }
}
