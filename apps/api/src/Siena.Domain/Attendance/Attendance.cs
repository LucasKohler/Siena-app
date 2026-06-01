namespace Siena.Domain.Attendance;

public sealed class Attendance
{
    public Attendance(
        string eventId,
        string userId,
        AttendanceStatus status,
        DateTimeOffset updatedAt)
    {
        EventId = eventId;
        UserId = userId;
        Status = status;
        UpdatedAt = updatedAt;
    }

    public string EventId { get; }
    public string UserId { get; }
    public AttendanceStatus Status { get; }
    public DateTimeOffset UpdatedAt { get; }
}
