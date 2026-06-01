namespace Siena.Domain.Videos;

public sealed class Video
{
    public Video(
        string id,
        string title,
        string url,
        int durationSeconds,
        DateTimeOffset publishedAt,
        int views)
    {
        Id = id;
        Title = title;
        Url = url;
        DurationSeconds = durationSeconds;
        PublishedAt = publishedAt;
        Views = views;
    }

    public string Id { get; }
    public string Title { get; }
    public string Url { get; }
    public int DurationSeconds { get; }
    public DateTimeOffset PublishedAt { get; }
    public int Views { get; }
}
