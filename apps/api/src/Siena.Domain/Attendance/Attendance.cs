namespace Siena.Domain.Attendance;

public sealed class Attendance
{
    public Attendance(
        string eventId,
        string userId,
        AttendanceStatus status,
        AttendanceApprovalStatus approvalStatus,
        DateTimeOffset updatedAt)
    {
        EventId = eventId;
        UserId = userId;
        Status = status;
        ApprovalStatus = approvalStatus;
        UpdatedAt = updatedAt;
    }

    public string EventId { get; }
    public string UserId { get; }
    public AttendanceStatus Status { get; }
    public AttendanceApprovalStatus ApprovalStatus { get; }
    public DateTimeOffset UpdatedAt { get; }
}
