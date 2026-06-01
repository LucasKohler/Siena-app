using Siena.Domain.Videos;

namespace Siena.Application.Videos;

public sealed class VideoQueryService : IVideoQueryService
{
    private readonly IVideoRepository _repository;

    public VideoQueryService(IVideoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<VideoSummaryDto>> GetVideosAsync(
        CancellationToken cancellationToken)
    {
        var videos = await _repository.ListAsync(cancellationToken);

        return videos
            .Select(VideoMappings.ToSummaryDto)
            .ToArray();
    }
}

internal static class VideoMappings
{
    public static VideoSummaryDto ToSummaryDto(Video video)
    {
        return new VideoSummaryDto(
            video.Id,
            video.Title,
            video.Url,
            video.DurationSeconds,
            video.PublishedAt,
            video.Views);
    }
}
