namespace Siena.Application.Auth;

public interface IAuthService
{
    Task<AuthResultDto?> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
}
