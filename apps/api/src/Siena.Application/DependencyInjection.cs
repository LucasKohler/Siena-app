using Microsoft.Extensions.DependencyInjection;
using Siena.Application.Auth;
using Siena.Application.Events;
using Siena.Application.Trainings;
using Siena.Application.Users;

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

        return services;
    }
}
