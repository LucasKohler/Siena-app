namespace Siena.Api.Endpoints;

public static class HealthEndpoints
{
    public static RouteGroupBuilder MapHealthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api")
            .WithTags("Health");

        group
            .MapGet("/health", () => Results.Ok(new HealthResponse("ok", "siena-api")))
            .WithName("GetHealth")
            .WithSummary("Checks whether the Siena API is running.")
            .Produces<HealthResponse>(StatusCodes.Status200OK);

        return group;
    }
}

public sealed record HealthResponse(string Status, string Service);
