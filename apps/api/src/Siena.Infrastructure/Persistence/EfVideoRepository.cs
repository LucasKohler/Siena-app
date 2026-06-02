using Microsoft.EntityFrameworkCore;
using Siena.Application.Videos;
using Siena.Domain.Videos;

namespace Siena.Infrastructure.Persistence;

public sealed class EfVideoRepository : IVideoRepository
{
    private readonly SienaDbContext _dbContext;

    public EfVideoRepository(SienaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<Video>> ListAsync(CancellationToken cancellationToken)
    {
        var videos = await _dbContext.Videos
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return videos
            .OrderByDescending(video => video.PublishedAt)
            .Select(EntityMappings.ToDomain)
            .ToArray();
    }
}
