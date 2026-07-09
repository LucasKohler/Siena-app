namespace Siena.Application.Trainings;

public interface IAttendanceApprovalService
{
    Task<IReadOnlyCollection<PendingAttendanceDto>> ListPendingAsync(
        string eventId,
        CancellationToken cancellationToken);

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
