using Microsoft.AspNetCore.RateLimiting;

namespace Siena.Api.Configuration;

public static class RateLimitConfiguration
{
    public const string LoginPolicy = "login";

    public static IServiceCollection AddSienaRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.AddFixedWindowLimiter(LoginPolicy, limiterOptions =>
            {
                limiterOptions.Window = TimeSpan.FromMinutes(1);
                limiterOptions.PermitLimit = 20;
                limiterOptions.QueueLimit = 0;
            });
        });

        return services;
    }

    public static WebApplication UseSienaRateLimiting(this WebApplication app)
    {
        app.UseRateLimiter();
        return app;
    }
}
