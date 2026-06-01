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
}
