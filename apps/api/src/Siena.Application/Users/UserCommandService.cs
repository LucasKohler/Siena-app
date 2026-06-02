using Siena.Application.Auth;
using Siena.Domain.Users;

namespace Siena.Application.Users;

public sealed class UserCommandService : IUserCommandService
{
    private readonly IUserRepository _userRepository;

    public UserCommandService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IReadOnlyCollection<UserSummaryDto>> ListAsync(
        bool includeInactive,
        CancellationToken cancellationToken)
    {
        var users = await _userRepository.ListAsync(includeInactive, cancellationToken);

        return users
            .Select(UserMappings.ToSummaryDto)
            .OrderBy(user => user.DisplayName, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    public async Task<CreateUserResult> CreateAsync(
        CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Id))
        {
            return CreateUserResult.InvalidData;
        }

        var normalizedPhone = PhoneNumberNormalizer.Normalize(request.PhoneNumber);

        if (string.IsNullOrEmpty(normalizedPhone))
        {
            return CreateUserResult.InvalidData;
        }

        UserRole role;
        PlayerPosition? position;

        try
        {
            role = UserMappings.ParseRole(request.Role);
            position = UserMappings.ParsePosition(request.Position);
        }
        catch (ArgumentException)
        {
            return CreateUserResult.InvalidData;
        }

        var existingById = await _userRepository.GetByIdAsync(request.Id, cancellationToken);

        if (existingById is not null)
        {
            return CreateUserResult.DuplicateId;
        }

        if (await _userRepository.ExistsByPhoneAsync(normalizedPhone, cancellationToken))
        {
            return CreateUserResult.DuplicatePhone;
        }

        var user = new UserAccount(
            request.Id,
            normalizedPhone,
            request.DisplayName.Trim(),
            role,
            isActive: true,
            position);

        await _userRepository.AddAsync(user, cancellationToken);

        return CreateUserResult.Success;
    }

    public async Task<UpdateUserResult> UpdateAsync(
        string id,
        UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var existing = await _userRepository.GetByIdAsync(id, cancellationToken);

        if (existing is null)
        {
            return UpdateUserResult.NotFound;
        }

        UserRole role;
        PlayerPosition? position;

        try
        {
            role = UserMappings.ParseRole(request.Role);
            position = UserMappings.ParsePosition(request.Position);
        }
        catch (ArgumentException)
        {
            return UpdateUserResult.InvalidData;
        }

        var updated = new UserAccount(
            existing.Id,
            existing.PhoneNumber,
            request.DisplayName.Trim(),
            role,
            existing.IsActive,
            position);

        await _userRepository.UpdateAsync(updated, cancellationToken);

        return UpdateUserResult.Success;
    }

    public async Task<SetUserActiveResult> SetActiveAsync(
        string id,
        bool isActive,
        CancellationToken cancellationToken)
    {
        var existing = await _userRepository.GetByIdAsync(id, cancellationToken);

        if (existing is null)
        {
            return SetUserActiveResult.NotFound;
        }

        await _userRepository.SetActiveAsync(id, isActive, cancellationToken);

        return SetUserActiveResult.Success;
    }
}
