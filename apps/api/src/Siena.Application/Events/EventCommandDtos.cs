namespace Siena.Application.Events;

public sealed record CreateEventRequest(
    string Id,
    string Title,
    string Type,
    string Category,
    DateTimeOffset StartsAt,
    string Location,
    string? Opponent,
    string? Description);

public sealed record UpdateEventRequest(
    string Title,
    string Type,
    string Category,
    DateTimeOffset StartsAt,
    string Location,
    string? Opponent,
    string? Description);
