namespace Siena.Application.Users;

public interface IUserCommandService
{
    Task<IReadOnlyCollection<UserSummaryDto>> ListAsync(
        bool includeInactive,
        CancellationToken cancellationToken);

    Task<CreateUserResult> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken);

    Task<UpdateUserResult> UpdateAsync(
        string id,
        UpdateUserRequest request,
        CancellationToken cancellationToken);

    Task<SetUserActiveResult> SetActiveAsync(
        string id,
        bool isActive,
        CancellationToken cancellationToken);
}

public enum CreateUserResult
{
    Success,
    DuplicateId,
    DuplicatePhone,
    InvalidData
}

public enum UpdateUserResult
{
    Success,
    NotFound,
    InvalidData
}

public enum SetUserActiveResult
{
    Success,
    NotFound
}
