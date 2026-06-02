using Siena.Domain.Attendance;

namespace Siena.Application.Trainings;

public interface IAttendanceRepository
{
    Task<IReadOnlyCollection<Attendance>> ListByEventAsync(
        string eventId,
        CancellationToken cancellationToken);

    Task<Attendance?> GetAsync(
        string eventId,
        string userId,
        CancellationToken cancellationToken);

    Task UpsertAsync(Attendance attendance, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<PendingAttendanceDto>> ListPendingByEventAsync(
        string eventId,
        CancellationToken cancellationToken);

    Task SetApprovalAsync(
        string eventId,
        string userId,
        AttendanceApprovalStatus approvalStatus,
        CancellationToken cancellationToken);
}
