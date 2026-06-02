namespace Siena.Infrastructure.Persistence.Entities;

public sealed class EventEntity
{
    public string Id { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public DateTimeOffset StartsAt { get; set; }

    public string Location { get; set; } = string.Empty;

    public string? Opponent { get; set; }

    public string? Description { get; set; }

    public ICollection<AttendanceEntity> Attendances { get; set; } = [];
}
