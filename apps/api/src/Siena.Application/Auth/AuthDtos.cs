namespace Siena.Application.Auth;

public sealed record LoginRequest(string PhoneNumber);

public sealed record AuthResultDto(
    string Token,
    DateTimeOffset ExpiresAt,
    string DisplayName,
    string Role);

public sealed record CurrentUserDto(string Id, string DisplayName, string Role);
