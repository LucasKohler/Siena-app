using Siena.Domain.Events;

namespace Siena.Application.Events;

public sealed class EventQueryService : IEventQueryService
{
    private readonly IEventRepository _repository;

    public EventQueryService(IEventRepository repository)
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
}

internal static class EventMappings
{
    public static EventSummaryDto ToSummaryDto(Event eventItem)
    {
        return new EventSummaryDto(
            eventItem.Id,
            eventItem.Title,
            ToLabel(eventItem.Type),
            ToLabel(eventItem.Category),
            eventItem.StartsAt,
            eventItem.Location,
            eventItem.Opponent);
    }

    public static EventDetailDto ToDetailDto(Event eventItem)
    {
        return new EventDetailDto(
            eventItem.Id,
            eventItem.Title,
            ToLabel(eventItem.Type),
            ToLabel(eventItem.Category),
            eventItem.StartsAt,
            eventItem.Location,
            eventItem.Opponent,
            eventItem.Description);
    }

    public static string ToLabel(EventType type)
    {
        return type switch
        {
            EventType.LigaNacional => "Liga Nacional",
            EventType.TreinoFisico => "Treino Físico",
            EventType.Amistoso => "Amistoso",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public static string ToLabel(EventCategory category)
    {
        return category switch
        {
            EventCategory.Masculino => "Masculino",
            EventCategory.Feminino => "Feminino",
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
        };
    }
}
