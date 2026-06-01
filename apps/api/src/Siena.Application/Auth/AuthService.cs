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

        if (user is null)
        {
            return null;
        }

        var (token, expiresAt) = _tokenService.CreateToken(user);

        return new AuthResultDto(
            token,
            expiresAt,
            user.DisplayName,
            AuthMappings.ToLabel(user.Role));
    }
}

public static class AuthMappings
{
    public static string ToLabel(UserRole role)
    {
        return role switch
        {
            UserRole.Athlete => "Atleta",
            UserRole.Coach => "Comissão",
            UserRole.Admin => "Administrador",
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
        };
    }

    public static string ToClaimValue(UserRole role)
    {
        return role switch
        {
            UserRole.Athlete => nameof(UserRole.Athlete),
            UserRole.Coach => nameof(UserRole.Coach),
            UserRole.Admin => nameof(UserRole.Admin),
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
        };
    }

    public static string ToLabelFromClaim(string claimValue)
    {
        return claimValue switch
        {
            nameof(UserRole.Athlete) => ToLabel(UserRole.Athlete),
            nameof(UserRole.Coach) => ToLabel(UserRole.Coach),
            nameof(UserRole.Admin) => ToLabel(UserRole.Admin),
            _ => claimValue
        };
    }
}
