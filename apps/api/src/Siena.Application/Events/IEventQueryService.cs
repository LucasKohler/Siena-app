namespace Siena.Application.Events;

public interface IEventQueryService
{
    Task<IReadOnlyCollection<EventSummaryDto>> GetEventsAsync(CancellationToken cancellationToken);

    Task<EventDetailDto?> GetEventByIdAsync(string id, CancellationToken cancellationToken);
}
