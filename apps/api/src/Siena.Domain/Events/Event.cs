namespace Siena.Domain.Events;

public sealed class Event
{
    public Event(
        string id,
        string title,
        EventType type,
        EventCategory category,
        DateTimeOffset startsAt,
        string location,
        string? opponent,
        string? description)
    {
        Id = id;
        Title = title;
        Type = type;
        Category = category;
        StartsAt = startsAt;
        Location = location;
        Opponent = opponent;
        Description = description;
    }

    public string Id { get; }
    public string Title { get; }
    public EventType Type { get; }
    public EventCategory Category { get; }
    public DateTimeOffset StartsAt { get; }
    public string Location { get; }
    public string? Opponent { get; }
    public string? Description { get; }
}
