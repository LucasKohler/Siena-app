using Siena.Domain;
using Siena.Domain.Attendance;
using Siena.Domain.Events;
using Siena.Domain.Users;
using Siena.Domain.Videos;
using Siena.Infrastructure.Persistence.Entities;

namespace Siena.Infrastructure.Persistence;

internal static class EntityMappings
{
    public static UserAccount ToDomain(UserEntity entity) =>
        new(
            entity.Id,
            entity.PhoneNumber,
            entity.DisplayName,
            DomainLabels.ParseRole(entity.Role),
            entity.IsActive,
            DomainLabels.ParsePosition(entity.Position));

    public static Event ToDomain(EventEntity entity) =>
        new(
            entity.Id,
            entity.Title,
            DomainLabels.ParseEventType(entity.Type),
            DomainLabels.ParseEventCategory(entity.Category),
            entity.StartsAt,
            entity.Location,
            entity.Opponent,
            entity.Description);

    public static Video ToDomain(VideoEntity entity) =>
        new(
            entity.Id,
            entity.Title,
            entity.Url,
            entity.DurationSeconds,
            entity.PublishedAt,
            entity.Views);

    public static Attendance ToDomain(AttendanceEntity entity) =>
        new(
            entity.EventId,
            entity.UserId,
            DomainLabels.ParseAttendanceStatus(entity.Status),
            DomainLabels.ParseApprovalStatus(entity.ApprovalStatus),
            entity.UpdatedAt);
}
