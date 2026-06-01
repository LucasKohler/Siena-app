using Microsoft.Extensions.DependencyInjection;
using Siena.Application.Events;
using Siena.Application.Videos;

namespace Siena.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IEventQueryService, EventQueryService>();
        services.AddScoped<IVideoQueryService, VideoQueryService>();

        return services;
    }
}
