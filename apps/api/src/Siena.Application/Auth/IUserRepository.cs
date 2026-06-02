using Siena.Domain.Users;

namespace Siena.Application.Auth;

public interface IUserRepository
{
    Task<UserAccount?> FindByPhoneAsync(string phoneNumber, CancellationToken cancellationToken);

    Task<UserAccount?> GetByIdAsync(string id, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<UserAccount>> ListAsync(
        bool includeInactive,
        CancellationToken cancellationToken);

    Task AddAsync(UserAccount user, CancellationToken cancellationToken);

    Task UpdateAsync(UserAccount user, CancellationToken cancellationToken);

    Task SetActiveAsync(string id, bool isActive, CancellationToken cancellationToken);

    Task<bool> ExistsByPhoneAsync(string phoneNumber, CancellationToken cancellationToken);
}
