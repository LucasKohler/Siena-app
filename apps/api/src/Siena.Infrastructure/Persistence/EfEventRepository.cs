using Microsoft.EntityFrameworkCore;
using Siena.Application.Events;
using Siena.Domain;
using Siena.Domain.Events;
using Siena.Infrastructure.Persistence.Entities;

namespace Siena.Infrastructure.Persistence;

public sealed class EfEventRepository : IEventRepository
{
    private readonly SienaDbContext _dbContext;

    public EfEventRepository(SienaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<Event>> ListAsync(CancellationToken cancellationToken)
    {
        var events = await _dbContext.Events
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return events
            .OrderBy(eventItem => eventItem.StartsAt)
            .Select(EntityMappings.ToDomain)
            .ToArray();
    }

    public async Task<Event?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var eventItem = await _dbContext.Events
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);

        return eventItem is null ? null : EntityMappings.ToDomain(eventItem);
    }

    public async Task AddAsync(Event eventItem, CancellationToken cancellationToken)
    {
        _dbContext.Events.Add(ToEntity(eventItem));
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Event eventItem, CancellationToken cancellationToken)
    {
        var existing = await _dbContext.Events.FirstOrDefaultAsync(
            entity => entity.Id == eventItem.Id,
            cancellationToken);

        if (existing is null)
        {
            throw new InvalidOperationException($"Event '{eventItem.Id}' was not found.");
        }

        existing.Title = eventItem.Title;
        existing.Type = DomainLabels.ToLabel(eventItem.Type);
        existing.Category = DomainLabels.ToLabel(eventItem.Category);
        existing.StartsAt = eventItem.StartsAt;
        existing.Location = eventItem.Location;
        existing.Opponent = eventItem.Opponent;
        existing.Description = eventItem.Description;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken)
    {
        var existing = await _dbContext.Events.FirstOrDefaultAsync(
            entity => entity.Id == id,
            cancellationToken);

        if (existing is null)
        {
            return;
        }

        _dbContext.Events.Remove(existing);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> HasAttendancesAsync(string eventId, CancellationToken cancellationToken)
    {
        return await _dbContext.Attendances.AnyAsync(
            attendance => attendance.EventId == eventId,
            cancellationToken);
    }

    public async Task<Event?> GetNextUpcomingTrainingAsync(
        DateTimeOffset asOf,
        CancellationToken cancellationToken)
    {
        var trainingType = DomainLabels.ToLabel(EventType.TreinoFisico);

        var candidates = await _dbContext.Events
            .AsNoTracking()
            .Where(entity => entity.Type == trainingType)
            .ToListAsync(cancellationToken);

        var eventItem = candidates
            .Where(entity => entity.StartsAt >= asOf)
            .OrderBy(entity => entity.StartsAt)
            .FirstOrDefault();

        return eventItem is null ? null : EntityMappings.ToDomain(eventItem);
    }

    private static EventEntity ToEntity(Event eventItem)
    {
        return new EventEntity
        {
            Id = eventItem.Id,
            Title = eventItem.Title,
            Type = DomainLabels.ToLabel(eventItem.Type),
            Category = DomainLabels.ToLabel(eventItem.Category),
            StartsAt = eventItem.StartsAt,
            Location = eventItem.Location,
            Opponent = eventItem.Opponent,
            Description = eventItem.Description
        };
    }
}
