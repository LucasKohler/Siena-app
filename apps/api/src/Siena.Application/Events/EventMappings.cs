using Siena.Domain;
using Siena.Domain.Events;

namespace Siena.Application.Events;

public static class EventMappings
{
    public static EventSummaryDto ToSummaryDto(Event eventItem) =>
        new(
            eventItem.Id,
            eventItem.Title,
            DomainLabels.ToLabel(eventItem.Type),
            DomainLabels.ToLabel(eventItem.Category),
            eventItem.StartsAt,
            eventItem.Location,
            eventItem.Opponent);

    public static EventDetailDto ToDetailDto(Event eventItem) =>
        new(
            eventItem.Id,
            eventItem.Title,
            DomainLabels.ToLabel(eventItem.Type),
            DomainLabels.ToLabel(eventItem.Category),
            eventItem.StartsAt,
            eventItem.Location,
            eventItem.Opponent,
            eventItem.Description);
}
