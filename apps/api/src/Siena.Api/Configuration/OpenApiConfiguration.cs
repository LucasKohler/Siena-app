using Scalar.AspNetCore;

namespace Siena.Api.Configuration;

public static class OpenApiConfiguration
{
    public static IServiceCollection AddSienaOpenApi(this IServiceCollection services)
    {
        services.AddOpenApi();

        return services;
    }

    public static WebApplication MapSienaOpenApi(this WebApplication app)
    {
        app.MapOpenApi();

        app.MapScalarApiReference(options =>
        {
            options.Title = "Siena API";
        });

        return app;
    }
}
