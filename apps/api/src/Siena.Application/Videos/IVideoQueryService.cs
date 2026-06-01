namespace Siena.Application.Videos;

public interface IVideoQueryService
{
    Task<IReadOnlyCollection<VideoSummaryDto>> GetVideosAsync(CancellationToken cancellationToken);
}
