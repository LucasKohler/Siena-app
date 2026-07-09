using Siena.Domain.Attendance;

namespace Siena.Application.Trainings;

/// <summary>
/// Read model for pending attendances with user display fields (not an HTTP DTO).
/// </summary>
public sealed record PendingAttendanceInfo(
    string UserId,
    string DisplayName,
    string? Position,
    AttendanceStatus Status,
    AttendanceApprovalStatus ApprovalStatus);
