using Microsoft.EntityFrameworkCore;
using Siena.Application.Auth;
using Siena.Domain;
using Siena.Domain.Users;
using Siena.Infrastructure.Persistence.Entities;

namespace Siena.Infrastructure.Persistence;

public sealed class EfUserRepository : IUserRepository
{
    private readonly SienaDbContext _dbContext;

    public EfUserRepository(SienaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserAccount?> FindByPhoneAsync(
        string phoneNumber,
        CancellationToken cancellationToken)
    {
        var normalizedPhone = PhoneNumberNormalizer.Normalize(phoneNumber);

        var user = await _dbContext.Users
            .AsNoTracking()
            .Where(entity => entity.IsActive && entity.PhoneNumber == normalizedPhone)
            .FirstOrDefaultAsync(cancellationToken);

        return user is null ? null : EntityMappings.ToDomain(user);
    }

    public async Task<UserAccount?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);

        return user is null ? null : EntityMappings.ToDomain(user);
    }

    public async Task<IReadOnlyCollection<UserAccount>> ListAsync(
        bool includeInactive,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.Users.AsNoTracking();

        if (!includeInactive)
        {
            query = query.Where(user => user.IsActive);
        }

        var users = await query.ToListAsync(cancellationToken);

        return users.Select(EntityMappings.ToDomain).ToArray();
    }

    public async Task AddAsync(UserAccount user, CancellationToken cancellationToken)
    {
        _dbContext.Users.Add(ToEntity(user));
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(UserAccount user, CancellationToken cancellationToken)
    {
        var existing = await _dbContext.Users.FirstOrDefaultAsync(
            entity => entity.Id == user.Id,
            cancellationToken);

        if (existing is null)
        {
            throw new InvalidOperationException($"User '{user.Id}' was not found.");
        }

        existing.DisplayName = user.DisplayName;
        existing.Role = DomainLabels.ToLabel(user.Role);
        existing.Position = user.Position is null
            ? null
            : DomainLabels.ToLabel(user.Position.Value);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SetActiveAsync(string id, bool isActive, CancellationToken cancellationToken)
    {
        var existing = await _dbContext.Users.FirstOrDefaultAsync(
            entity => entity.Id == id,
            cancellationToken);

        if (existing is null)
        {
            throw new InvalidOperationException($"User '{id}' was not found.");
        }

        existing.IsActive = isActive;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsByPhoneAsync(string phoneNumber, CancellationToken cancellationToken)
    {
        var normalizedPhone = PhoneNumberNormalizer.Normalize(phoneNumber);

        return await _dbContext.Users
            .AsNoTracking()
            .AnyAsync(entity => entity.PhoneNumber == normalizedPhone, cancellationToken);
    }

    private static UserEntity ToEntity(UserAccount user)
    {
        return new UserEntity
        {
            Id = user.Id,
            PhoneNumber = user.PhoneNumber,
            DisplayName = user.DisplayName,
            Role = DomainLabels.ToLabel(user.Role),
            IsActive = user.IsActive,
            Position = user.Position is null
                ? null
                : DomainLabels.ToLabel(user.Position.Value)
        };
    }
}
