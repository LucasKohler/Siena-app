using Siena.Domain.Users;

namespace Siena.Application.Auth;

public interface ITokenService
{
    (string Token, DateTimeOffset ExpiresAt) CreateToken(UserAccount user);
}
