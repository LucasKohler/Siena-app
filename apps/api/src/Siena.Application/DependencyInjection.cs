using Microsoft.Extensions.DependencyInjection;
using Siena.Application.Auth;
using Siena.Application.Events;
using Siena.Application.Trainings;
using Siena.Application.Users;
using Siena.Application.Videos;

namespace Siena.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITrainingQueryService, TrainingQueryService>();
        services.AddScoped<IAttendanceService, AttendanceService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IUserCommandService, UserCommandService>();
        services.AddScoped<IAttendanceApprovalService, AttendanceApprovalService>();
        services.AddScoped<IVideoQueryService, VideoQueryService>();

        return services;
    }
}

