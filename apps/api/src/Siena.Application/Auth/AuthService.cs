using Siena.Domain;
using Siena.Domain.Users;

namespace Siena.Application.Auth;

public sealed class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public AuthService(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthResultDto?> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var normalizedPhone = PhoneNumberNormalizer.Normalize(request.PhoneNumber);

        if (string.IsNullOrEmpty(normalizedPhone))
        {
            return null;
        }

        var user = await _userRepository.FindByPhoneAsync(normalizedPhone, cancellationToken);

        if (user is null || !user.IsActive)
        {
            return null;
        }

        var (token, expiresAt) = _tokenService.CreateToken(user);

        return new AuthResultDto(
            token,
            expiresAt,
            user.DisplayName,
            DomainLabels.ToLabel(user.Role));
    }
}
