namespace Siena.Infrastructure.Persistence.Entities;

public sealed class AttendanceEntity
{
    public string EventId { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string ApprovalStatus { get; set; } = string.Empty;

    public DateTimeOffset UpdatedAt { get; set; }

    public EventEntity Event { get; set; } = null!;

    public UserEntity User { get; set; } = null!;
}
