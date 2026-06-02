namespace Siena.Application.Events;

public interface IEventService
{
    Task<IReadOnlyCollection<EventSummaryDto>> GetEventsAsync(CancellationToken cancellationToken);

    Task<EventDetailDto?> GetEventByIdAsync(string id, CancellationToken cancellationToken);

    Task<CreateEventResult> CreateAsync(CreateEventRequest request, CancellationToken cancellationToken);

    Task<UpdateEventResult> UpdateAsync(string id, UpdateEventRequest request, CancellationToken cancellationToken);

    Task<DeleteEventResult> DeleteAsync(string id, CancellationToken cancellationToken);
}

public enum CreateEventResult
{
    Success,
    DuplicateId,
    InvalidData
}

public enum UpdateEventResult
{
    Success,
    NotFound,
    InvalidData
}

public enum DeleteEventResult
{
    Success,
    NotFound,
    HasAttendances
}
