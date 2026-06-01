namespace Siena.Application.Events;

public sealed record EventSummaryDto(
    string Id,
    string Title,
    string Type,
    string Category,
    DateTimeOffset StartsAt,
    string Location,
    string? Opponent);

public sealed record EventDetailDto(
    string Id,
    string Title,
    string Type,
    string Category,
    DateTimeOffset StartsAt,
    string Location,
    string? Opponent,
    string? Description);
