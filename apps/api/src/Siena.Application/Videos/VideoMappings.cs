using Siena.Domain.Videos;

namespace Siena.Application.Videos;

public static class VideoMappings
{
    public static VideoSummaryDto ToSummaryDto(Video video) =>
        new(
            video.Id,
            video.Title,
            video.Url,
            video.DurationSeconds,
            video.PublishedAt,
            video.Views);
}
