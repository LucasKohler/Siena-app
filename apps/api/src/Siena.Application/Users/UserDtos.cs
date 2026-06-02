namespace Siena.Application.Users;

public sealed record UserSummaryDto(
    string Id,
    string PhoneNumber,
    string DisplayName,
    string Role,
    string? Position,
    bool IsActive);

public sealed record CreateUserRequest(
    string Id,
    string PhoneNumber,
    string DisplayName,
    string Role,
    string? Position);

public sealed record UpdateUserRequest(
    string DisplayName,
    string Role,
    string? Position);

public sealed record SetUserActiveRequest(bool IsActive);
