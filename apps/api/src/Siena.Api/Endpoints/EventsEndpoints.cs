using Siena.Application.Events;

namespace Siena.Api.Endpoints;

public static class EventsEndpoints
{
    public static RouteGroupBuilder MapEventEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/events")
            .WithTags("Events");

        group.MapGet("/", async (
            IEventService events,
            CancellationToken cancellationToken) =>
        {
            var result = await events.GetEventsAsync(cancellationToken);

            return Results.Ok(result);
        })
        .WithName("GetEvents")
        .WithSummary("Gets event summaries.")
        .Produces<IReadOnlyCollection<EventSummaryDto>>(StatusCodes.Status200OK);

        group.MapGet("/{id}", async (
            string id,
            IEventService events,
            CancellationToken cancellationToken) =>
        {
            var result = await events.GetEventByIdAsync(id, cancellationToken);

            return result is null
                ? Results.Problem(
                    title: "Event not found",
                    detail: $"No event was found for id '{id}'.",
                    statusCode: StatusCodes.Status404NotFound)
                : Results.Ok(result);
        })
        .WithName("GetEventById")
        .WithSummary("Gets event detail by id.")
        .Produces<EventDetailDto>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound);

        return group;
    }
}
