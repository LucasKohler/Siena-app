using Siena.Domain.Users;

namespace Siena.Application.Trainings;

public interface IAttendanceService
{
    Task<SetAttendanceResult> SetAttendanceAsync(
        string eventId,
        string userId,
        UserRole userRole,
        SetAttendanceRequest request,
        CancellationToken cancellationToken);
}

public enum SetAttendanceResult
{
    Success,
    Forbidden,
    EventNotFound,
    NotATrainingEvent,
    TrainingAlreadyStarted,
    InvalidStatus
}
