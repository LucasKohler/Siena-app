using Microsoft.EntityFrameworkCore;
using Siena.Application.Trainings;
using Siena.Domain;
using Siena.Domain.Attendance;
using Siena.Infrastructure.Persistence.Entities;

namespace Siena.Infrastructure.Persistence;

public sealed class EfAttendanceRepository : IAttendanceRepository
{
    private readonly SienaDbContext _dbContext;

    public EfAttendanceRepository(SienaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<Attendance>> ListByEventAsync(
        string eventId,
        CancellationToken cancellationToken)
    {
        var attendances = await _dbContext.Attendances
            .AsNoTracking()
            .Where(attendance => attendance.EventId == eventId)
            .ToListAsync(cancellationToken);

        return attendances.Select(EntityMappings.ToDomain).ToArray();
    }

    public async Task<Attendance?> GetAsync(
        string eventId,
        string userId,
        CancellationToken cancellationToken)
    {
        var attendance = await _dbContext.Attendances
            .AsNoTracking()
            .FirstOrDefaultAsync(
                entity => entity.EventId == eventId && entity.UserId == userId,
                cancellationToken);

        return attendance is null ? null : EntityMappings.ToDomain(attendance);
    }

    public async Task UpsertAsync(Attendance attendance, CancellationToken cancellationToken)
    {
        var existing = await _dbContext.Attendances.FirstOrDefaultAsync(
            entity => entity.EventId == attendance.EventId && entity.UserId == attendance.UserId,
            cancellationToken);

        if (existing is null)
        {
            _dbContext.Attendances.Add(new AttendanceEntity
            {
                EventId = attendance.EventId,
                UserId = attendance.UserId,
                Status = DomainLabels.ToLabel(attendance.Status),
                ApprovalStatus = DomainLabels.ToLabel(attendance.ApprovalStatus),
                UpdatedAt = attendance.UpdatedAt
            });
        }
        else
        {
            existing.Status = DomainLabels.ToLabel(attendance.Status);
            existing.ApprovalStatus = DomainLabels.ToLabel(attendance.ApprovalStatus);
            existing.UpdatedAt = attendance.UpdatedAt;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<PendingAttendanceInfo>> ListPendingWithUsersByEventAsync(
        string eventId,
        CancellationToken cancellationToken)
    {
        var pendingLabel = DomainLabels.ToLabel(AttendanceApprovalStatus.Pending);

        var rows = await (
            from attendance in _dbContext.Attendances.AsNoTracking()
            join user in _dbContext.Users.AsNoTracking() on attendance.UserId equals user.Id
            where attendance.EventId == eventId
                  && attendance.ApprovalStatus == pendingLabel
                  && user.IsActive
            select new
            {
                attendance.UserId,
                user.DisplayName,
                user.Position,
                attendance.Status,
                attendance.ApprovalStatus
            }).ToListAsync(cancellationToken);

        return rows
            .Select(row => new PendingAttendanceInfo(
                row.UserId,
                row.DisplayName,
                string.IsNullOrWhiteSpace(row.Position) ? null : row.Position,
                DomainLabels.ParseAttendanceStatus(row.Status),
                DomainLabels.ParseApprovalStatus(row.ApprovalStatus)))
            .OrderBy(row => row.DisplayName, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    public async Task SetApprovalAsync(
        string eventId,
        string userId,
        AttendanceApprovalStatus approvalStatus,
        CancellationToken cancellationToken)
    {
        var existing = await _dbContext.Attendances.FirstOrDefaultAsync(
            entity => entity.EventId == eventId && entity.UserId == userId,
            cancellationToken);

        if (existing is null)
        {
            throw new InvalidOperationException(
                $"Attendance for event '{eventId}' and user '{userId}' was not found.");
        }

        existing.ApprovalStatus = DomainLabels.ToLabel(approvalStatus);
        existing.UpdatedAt = DateTimeOffset.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
