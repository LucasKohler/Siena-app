using Microsoft.Extensions.DependencyInjection;
using Siena.Application.Events;
using Siena.Application.Videos;
using Siena.Infrastructure.Events;
using Siena.Infrastructure.Videos;

namespace Siena.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IEventRepository, JsonEventRepository>();
        services.AddScoped<IVideoRepository, JsonVideoRepository>();

        return services;
    }
}
