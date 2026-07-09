namespace Siena.Application.Videos;

public sealed class VideoQueryService : IVideoQueryService
{
    private readonly IVideoRepository _videoRepository;

    public VideoQueryService(IVideoRepository videoRepository)
    {
        _videoRepository = videoRepository;
    }

    public async Task<IReadOnlyCollection<VideoSummaryDto>> GetVideosAsync(
        CancellationToken cancellationToken)
    {
        var videos = await _videoRepository.ListAsync(cancellationToken);

        return videos
            .Select(VideoMappings.ToSummaryDto)
            .ToArray();
    }
}
