using Siena.Application.Videos;

namespace Siena.Api.Endpoints;

public static class VideosEndpoints
{
    public static RouteGroupBuilder MapVideoEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/videos")
            .WithTags("Videos");

        group.MapGet("/", async (
            IVideoQueryService videos,
            CancellationToken cancellationToken) =>
        {
            var items = await videos.GetVideosAsync(cancellationToken);
            return Results.Ok(items);
        })
        .WithName("GetVideos")
        .WithSummary("Gets video summaries.")
        .Produces<IReadOnlyCollection<VideoSummaryDto>>(StatusCodes.Status200OK);

        return group;
    }
}
