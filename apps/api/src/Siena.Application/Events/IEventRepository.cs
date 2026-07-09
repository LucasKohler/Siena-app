using Siena.Domain.Events;

namespace Siena.Application.Events;

public interface IEventRepository
{
    Task<IReadOnlyCollection<Event>> ListAsync(CancellationToken cancellationToken);

    Task<Event?> GetByIdAsync(string id, CancellationToken cancellationToken);

    Task AddAsync(Event eventItem, CancellationToken cancellationToken);

    Task UpdateAsync(Event eventItem, CancellationToken cancellationToken);

    Task DeleteAsync(string id, CancellationToken cancellationToken);

    Task<bool> HasAttendancesAsync(string eventId, CancellationToken cancellationToken);

    Task<Event?> GetNextUpcomingTrainingAsync(
        DateTimeOffset asOf,
        CancellationToken cancellationToken);
}
