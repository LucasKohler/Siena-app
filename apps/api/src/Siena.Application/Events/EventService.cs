using Siena.Domain;
using Siena.Domain.Events;

namespace Siena.Application.Events;

public sealed class EventService : IEventService
{
    private readonly IEventRepository _repository;

    public EventService(IEventRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<EventSummaryDto>> GetEventsAsync(
        CancellationToken cancellationToken)
    {
        var events = await _repository.ListAsync(cancellationToken);

        return events
            .Select(EventMappings.ToSummaryDto)
            .ToArray();
    }

    public async Task<EventDetailDto?> GetEventByIdAsync(
        string id,
        CancellationToken cancellationToken)
    {
        var eventItem = await _repository.GetByIdAsync(id, cancellationToken);

        return eventItem is null ? null : EventMappings.ToDetailDto(eventItem);
    }

    public async Task<CreateEventResult> CreateAsync(
        CreateEventRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Id))
        {
            return CreateEventResult.InvalidData;
        }

        Event eventItem;

        try
        {
            eventItem = ToDomain(request);
        }
        catch (ArgumentException)
        {
            return CreateEventResult.InvalidData;
        }

        var existing = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (existing is not null)
        {
            return CreateEventResult.DuplicateId;
        }

        await _repository.AddAsync(eventItem, cancellationToken);

        return CreateEventResult.Success;
    }

    public async Task<UpdateEventResult> UpdateAsync(
        string id,
        UpdateEventRequest request,
        CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(id, cancellationToken);

        if (existing is null)
        {
            return UpdateEventResult.NotFound;
        }

        Event eventItem;

        try
        {
            eventItem = new Event(
                id,
                request.Title,
                DomainLabels.ParseEventType(request.Type),
                DomainLabels.ParseEventCategory(request.Category),
                request.StartsAt,
                request.Location,
                string.IsNullOrWhiteSpace(request.Opponent) ? null : request.Opponent,
                string.IsNullOrWhiteSpace(request.Description) ? null : request.Description);
        }
        catch (ArgumentException)
        {
            return UpdateEventResult.InvalidData;
        }

        await _repository.UpdateAsync(eventItem, cancellationToken);

        return UpdateEventResult.Success;
    }

    public async Task<DeleteEventResult> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(id, cancellationToken);

        if (existing is null)
        {
            return DeleteEventResult.NotFound;
        }

        if (await _repository.HasAttendancesAsync(id, cancellationToken))
        {
            return DeleteEventResult.HasAttendances;
        }

        await _repository.DeleteAsync(id, cancellationToken);

        return DeleteEventResult.Success;
    }

    private static Event ToDomain(CreateEventRequest request) =>
        new(
            request.Id,
            request.Title,
            DomainLabels.ParseEventType(request.Type),
            DomainLabels.ParseEventCategory(request.Category),
            request.StartsAt,
            request.Location,
            string.IsNullOrWhiteSpace(request.Opponent) ? null : request.Opponent,
            string.IsNullOrWhiteSpace(request.Description) ? null : request.Description);
}
