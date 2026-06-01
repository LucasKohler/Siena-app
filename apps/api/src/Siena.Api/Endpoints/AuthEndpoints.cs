using System.Security.Claims;
using Siena.Application.Auth;

namespace Siena.Api.Endpoints;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth")
            .WithTags("Auth");

        group.MapPost("/login", async (
            LoginRequest request,
            IAuthService authService,
            CancellationToken cancellationToken) =>
        {
            var result = await authService.LoginAsync(request, cancellationToken);

            return result is null
                ? Results.Unauthorized()
                : Results.Ok(result);
        })
        .WithName("Login")
        .WithSummary("Authenticates a phone number on the internal allowlist and returns a JWT.")
        .Produces<AuthResultDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        group.MapGet("/me", (ClaimsPrincipal user) =>
        {
            var id = user.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? user.FindFirstValue("sub");

            var displayName = user.FindFirstValue(ClaimTypes.Name);
            var roleClaim = user.FindFirstValue(ClaimTypes.Role);

            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(displayName))
            {
                return Results.Unauthorized();
            }

            var role = string.IsNullOrWhiteSpace(roleClaim)
                ? string.Empty
                : AuthMappings.ToLabelFromClaim(roleClaim);

            return Results.Ok(new CurrentUserDto(id, displayName, role));
        })
        .RequireAuthorization()
        .WithName("GetCurrentUser")
        .WithSummary("Returns the authenticated user from the JWT.")
        .Produces<CurrentUserDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        return group;
    }
}
