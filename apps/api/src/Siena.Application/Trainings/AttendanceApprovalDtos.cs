namespace Siena.Application.Trainings;

public sealed record PendingAttendanceDto(
    string UserId,
    string DisplayName,
    string? Position,
    string Response,
    string ApprovalStatus);

public sealed record SetAttendanceApprovalRequest(string Action);
