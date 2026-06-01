namespace Siena.Application.Videos;

public sealed record VideoSummaryDto(
    string Id,
    string Title,
    string Url,
    int DurationSeconds,
    DateTimeOffset PublishedAt,
    int Views);
