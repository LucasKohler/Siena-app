using Microsoft.Extensions.DependencyInjection;
using Siena.Application.Auth;
using Siena.Application.Events;
using Siena.Application.Trainings;
using Siena.Application.Videos;
using Siena.Infrastructure.Persistence;
using Siena.Infrastructure.Auth;
using Siena.Infrastructure.Events;
using Siena.Infrastructure.Users;
using Siena.Infrastructure.Videos;

namespace Siena.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, JsonUserRepository>();
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IAttendanceRepository, JsonAttendanceRepository>();
        services.AddScoped<IEventRepository, JsonEventRepository>();
        services.AddScoped<IVideoRepository, JsonVideoRepository>();

        return services;
    }
}
