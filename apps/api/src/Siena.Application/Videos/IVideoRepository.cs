using Siena.Domain.Videos;

namespace Siena.Application.Videos;

public interface IVideoRepository
{
    Task<IReadOnlyCollection<Video>> ListAsync(CancellationToken cancellationToken);
}
