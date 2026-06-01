using Microsoft.Extensions.DependencyInjection;
using Siena.Application.Auth;
using Siena.Application.Events;
using Siena.Application.Trainings;
using Siena.Application.Videos;

namespace Siena.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITrainingQueryService, TrainingQueryService>();
        services.AddScoped<IAttendanceService, AttendanceService>();
        services.AddScoped<IEventQueryService, EventQueryService>();
        services.AddScoped<IVideoQueryService, VideoQueryService>();

        return services;
    }
}
