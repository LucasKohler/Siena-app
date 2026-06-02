using Siena.Application.Events;
using Siena.Application.Trainings;
using Siena.Application.Users;

namespace Siena.Api.Endpoints;

public static class AdminEndpoints
{
    public static RouteGroupBuilder MapAdminEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/admin")
            .RequireStaff()
            .WithTags("Admin");

        group.MapGet("/events", async (
            IEventService events,
            CancellationToken cancellationToken) =>
        {
            var result = await events.GetEventsAsync(cancellationToken);
            return Results.Ok(result);
        })
        .WithName("AdminGetEvents")
        .WithSummary("Lists all events (staff).")
        .Produces<IReadOnlyCollection<EventSummaryDto>>(StatusCodes.Status200OK);

        group.MapPost("/events", async (
            CreateEventRequest request,
            IEventService events,
            CancellationToken cancellationToken) =>
        {
            var result = await events.CreateAsync(request, cancellationToken);

            return result switch
            {
                CreateEventResult.Success => Results.Created($"/api/events/{request.Id}", null),
                CreateEventResult.DuplicateId => Results.Problem(
                    title: "Duplicate event id",
                    detail: $"An event with id '{request.Id}' already exists.",
                    statusCode: StatusCodes.Status409Conflict),
                CreateEventResult.InvalidData => Results.ValidationProblem(
                    new Dictionary<string, string[]>
                    {
                        ["request"] = ["Invalid event data. Check id, type and category labels."]
                    }),
                _ => Results.Problem(statusCode: StatusCodes.Status500InternalServerError)
            };
        })
        .WithName("AdminCreateEvent")
        .WithSummary("Creates an event (staff).")
        .Produces(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .ProducesValidationProblem();

        group.MapPut("/events/{id}", async (
            string id,
            UpdateEventRequest request,
            IEventService events,
            CancellationToken cancellationToken) =>
        {
            var result = await events.UpdateAsync(id, request, cancellationToken);

            return result switch
            {
                UpdateEventResult.Success => Results.NoContent(),
                UpdateEventResult.NotFound => Results.Problem(
                    title: "Event not found",
                    detail: $"No event was found for id '{id}'.",
                    statusCode: StatusCodes.Status404NotFound),
                UpdateEventResult.InvalidData => Results.ValidationProblem(
                    new Dictionary<string, string[]>
                    {
                        ["request"] = ["Invalid event data. Check type and category labels."]
                    }),
                _ => Results.Problem(statusCode: StatusCodes.Status500InternalServerError)
            };
        })
        .WithName("AdminUpdateEvent")
        .WithSummary("Updates an event (staff).")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesValidationProblem();

        group.MapDelete("/events/{id}", async (
            string id,
            IEventService events,
            CancellationToken cancellationToken) =>
        {
            var result = await events.DeleteAsync(id, cancellationToken);

            return result switch
            {
                DeleteEventResult.Success => Results.NoContent(),
                DeleteEventResult.NotFound => Results.Problem(
                    title: "Event not found",
                    detail: $"No event was found for id '{id}'.",
                    statusCode: StatusCodes.Status404NotFound),
                DeleteEventResult.HasAttendances => Results.Problem(
                    title: "Event has attendances",
                    detail: "Cannot delete an event that has attendance records.",
                    statusCode: StatusCodes.Status409Conflict),
                _ => Results.Problem(statusCode: StatusCodes.Status500InternalServerError)
            };
        })
        .WithName("AdminDeleteEvent")
        .WithSummary("Deletes an event without attendances (staff).")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict);

        group.MapGet("/users", async (
            bool includeInactive,
            IUserCommandService users,
            CancellationToken cancellationToken) =>
        {
            var result = await users.ListAsync(includeInactive, cancellationToken);
            return Results.Ok(result);
        })
        .WithName("AdminGetUsers")
        .WithSummary("Lists users in the allowlist (staff).")
        .Produces<IReadOnlyCollection<UserSummaryDto>>(StatusCodes.Status200OK);

        group.MapPost("/users", async (
            CreateUserRequest request,
            IUserCommandService users,
            CancellationToken cancellationToken) =>
        {
            var result = await users.CreateAsync(request, cancellationToken);

            return result switch
            {
                CreateUserResult.Success => Results.Created($"/api/admin/users/{request.Id}", null),
                CreateUserResult.DuplicateId => Results.Problem(
                    title: "Duplicate user id",
                    detail: $"A user with id '{request.Id}' already exists.",
                    statusCode: StatusCodes.Status409Conflict),
                CreateUserResult.DuplicatePhone => Results.Problem(
                    title: "Duplicate phone number",
                    detail: "A user with this phone number already exists.",
                    statusCode: StatusCodes.Status409Conflict),
                CreateUserResult.InvalidData => Results.ValidationProblem(
                    new Dictionary<string, string[]>
                    {
                        ["request"] = ["Invalid user data. Check phone, role and position labels."]
                    }),
                _ => Results.Problem(statusCode: StatusCodes.Status500InternalServerError)
            };
        })
        .WithName("AdminCreateUser")
        .WithSummary("Adds a user to the allowlist (staff).")
        .Produces(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .ProducesValidationProblem();

        group.MapPut("/users/{id}", async (
            string id,
            UpdateUserRequest request,
            IUserCommandService users,
            CancellationToken cancellationToken) =>
        {
            var result = await users.UpdateAsync(id, request, cancellationToken);

            return result switch
            {
                UpdateUserResult.Success => Results.NoContent(),
                UpdateUserResult.NotFound => Results.Problem(
                    title: "User not found",
                    detail: $"No user was found for id '{id}'.",
                    statusCode: StatusCodes.Status404NotFound),
                UpdateUserResult.InvalidData => Results.ValidationProblem(
                    new Dictionary<string, string[]>
                    {
                        ["request"] = ["Invalid user data. Check role and position labels."]
                    }),
                _ => Results.Problem(statusCode: StatusCodes.Status500InternalServerError)
            };
        })
        .WithName("AdminUpdateUser")
        .WithSummary("Updates a user in the allowlist (staff).")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesValidationProblem();

        group.MapPatch("/users/{id}/active", async (
            string id,
            SetUserActiveRequest request,
            IUserCommandService users,
            CancellationToken cancellationToken) =>
        {
            var result = await users.SetActiveAsync(id, request.IsActive, cancellationToken);

            return result switch
            {
                SetUserActiveResult.Success => Results.NoContent(),
                SetUserActiveResult.NotFound => Results.Problem(
                    title: "User not found",
                    detail: $"No user was found for id '{id}'.",
                    statusCode: StatusCodes.Status404NotFound),
                _ => Results.Problem(statusCode: StatusCodes.Status500InternalServerError)
            };
        })
        .WithName("AdminSetUserActive")
        .WithSummary("Activates or deactivates a user (staff).")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapGet("/trainings/{eventId}/attendances/pending", async (
            string eventId,
            IAttendanceRepository attendances,
            CancellationToken cancellationToken) =>
        {
            var result = await attendances.ListPendingByEventAsync(eventId, cancellationToken);
            return Results.Ok(result);
        })
        .WithName("AdminListPendingAttendances")
        .WithSummary("Lists pending attendances for a training (staff).")
        .Produces<IReadOnlyCollection<PendingAttendanceDto>>(StatusCodes.Status200OK);

        group.MapPost("/trainings/{eventId}/attendances/{userId}/approval", async (
            string eventId,
            string userId,
            SetAttendanceApprovalRequest request,
            IAttendanceApprovalService approval,
            CancellationToken cancellationToken) =>
        {
            var result = await approval.SetApprovalAsync(
                eventId,
                userId,
                request.Action,
                cancellationToken);

            return result switch
            {
                SetApprovalResult.Success => Results.NoContent(),
                SetApprovalResult.EventNotFound => Results.Problem(
                    title: "Training not found",
                    detail: $"No training was found for id '{eventId}'.",
                    statusCode: StatusCodes.Status404NotFound),
                SetApprovalResult.NotATrainingEvent => Results.Problem(
                    title: "Not a training event",
                    detail: $"Event '{eventId}' is not a physical training session.",
                    statusCode: StatusCodes.Status404NotFound),
                SetApprovalResult.AttendanceNotFound => Results.Problem(
                    title: "Attendance not found",
                    detail: $"No attendance was found for user '{userId}'.",
                    statusCode: StatusCodes.Status404NotFound),
                SetApprovalResult.NotPending => Results.Problem(
                    title: "Attendance not pending",
                    detail: "Only pending attendances can be approved or rejected.",
                    statusCode: StatusCodes.Status409Conflict),
                SetApprovalResult.InvalidAction => Results.ValidationProblem(
                    new Dictionary<string, string[]>
                    {
                        ["action"] = ["Action must be 'approve' or 'reject'."]
                    }),
                _ => Results.Problem(statusCode: StatusCodes.Status500InternalServerError)
            };
        })
        .WithName("AdminSetAttendanceApproval")
        .WithSummary("Approves or rejects a pending attendance (staff).")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .ProducesValidationProblem();

        return group;
    }
}
