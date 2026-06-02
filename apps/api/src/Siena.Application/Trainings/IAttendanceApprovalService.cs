namespace Siena.Application.Trainings;

public interface IAttendanceApprovalService
{
    Task<SetApprovalResult> SetApprovalAsync(
        string eventId,
        string userId,
        string action,
        CancellationToken cancellationToken);
}

public enum SetApprovalResult
{
    Success,
    EventNotFound,
    NotATrainingEvent,
    AttendanceNotFound,
    NotPending,
    InvalidAction
}
