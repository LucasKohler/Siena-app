using Siena.Domain;
using Siena.Domain.Users;

namespace Siena.Application.Users;

public static class UserMappings
{
    public static UserSummaryDto ToSummaryDto(UserAccount user) =>
        new(
            user.Id,
            user.PhoneNumber,
            user.DisplayName,
            DomainLabels.ToLabel(user.Role),
            user.Position is null ? null : DomainLabels.ToLabel(user.Position.Value),
            user.IsActive);

    public static UserRole ParseRole(string value) => DomainLabels.ParseRole(value);

    public static PlayerPosition? ParsePosition(string? value) => DomainLabels.ParsePosition(value);
}
