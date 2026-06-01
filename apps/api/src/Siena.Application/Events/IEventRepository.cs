using Siena.Domain.Events;

namespace Siena.Application.Events;

public interface IEventRepository
{
    Task<IReadOnlyCollection<Event>> ListAsync(CancellationToken cancellationToken);

    Task<Event?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
