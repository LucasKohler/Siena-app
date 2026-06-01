using System.Security.Claims;
using Siena.Application.Trainings;
using Siena.Domain.Users;

namespace Siena.Api.Endpoints;

public static class TrainingEndpoints
{
    public static RouteGroupBuilder MapTrainingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/trainings")
            .RequireAuthorization()
            .WithTags("Trainings");

        group.MapGet("/next", async (
            ClaimsPrincipal user,
            ITrainingQueryService trainings,
            CancellationToken cancellationToken) =>
        {
            if (!TryGetUserId(user, out var userId))
            {
                return Results.Unauthorized();
            }

            var result = await trainings.GetNextTrainingAsync(userId, cancellationToken);

            return result is null
                ? Results.Problem(
                    title: "No upcoming training",
                    detail: "No physical training session was found in the calendar.",
                    statusCode: StatusCodes.Status404NotFound)
                : Results.Ok(result);
        })
        .WithName("GetNextTraining")
        .WithSummary("Gets the next physical training session with attendance summary.")
        .Produces<NextTrainingDto>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapPost("/{id}/attendance", async (
            string id,
            SetAttendanceRequest request,
            ClaimsPrincipal user,
            IAttendanceService attendanceService,
            CancellationToken cancellationToken) =>
        {
            if (!TryGetUserId(user, out var userId) || !TryGetUserRole(user, out var userRole))
            {
                return Results.Unauthorized();
            }

            var result = await attendanceService.SetAttendanceAsync(
                id,
                userId,
                userRole,
                request,
                cancellationToken);

            return result switch
            {
                SetAttendanceResult.Success => Results.NoContent(),
                SetAttendanceResult.Forbidden => Results.Forbid(),
                SetAttendanceResult.EventNotFound => Results.Problem(
                    title: "Training not found",
                    detail: $"No training was found for id '{id}'.",
                    statusCode: StatusCodes.Status404NotFound),
                SetAttendanceResult.NotATrainingEvent => Results.Problem(
                    title: "Not a training event",
                    detail: $"Event '{id}' is not a physical training session.",
                    statusCode: StatusCodes.Status404NotFound),
                SetAttendanceResult.TrainingAlreadyStarted => Results.Problem(
                    title: "Training already started",
                    detail: "Attendance can no longer be changed after the training has started.",
                    statusCode: StatusCodes.Status409Conflict),
                SetAttendanceResult.InvalidStatus => Results.ValidationProblem(
                    new Dictionary<string, string[]>
                    {
                        ["status"] = ["Status must be 'Eu vou' or 'Não vou'."]
                    }),
                _ => Results.Problem(statusCode: StatusCodes.Status500InternalServerError)
            };
        })
        .WithName("SetTrainingAttendance")
        .WithSummary("Sets the authenticated athlete attendance for a training session.")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .ProducesValidationProblem();

        return group;
    }

    private static bool TryGetUserId(ClaimsPrincipal user, out string userId)
    {
        userId = user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? user.FindFirstValue("sub")
            ?? string.Empty;

        return !string.IsNullOrWhiteSpace(userId);
    }

    private static bool TryGetUserRole(ClaimsPrincipal user, out UserRole userRole)
    {
        var roleClaim = user.FindFirstValue(ClaimTypes.Role);

        if (string.IsNullOrWhiteSpace(roleClaim))
        {
            userRole = default;
            return false;
        }

        return Enum.TryParse(roleClaim, ignoreCase: true, out userRole);
    }
}
