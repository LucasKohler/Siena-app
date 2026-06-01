using Siena.Domain.Users;

namespace Siena.Application.Auth;

public interface IUserRepository
{
    Task<UserAccount?> FindByPhoneAsync(string phoneNumber, CancellationToken cancellationToken);

    Task<UserAccount?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
