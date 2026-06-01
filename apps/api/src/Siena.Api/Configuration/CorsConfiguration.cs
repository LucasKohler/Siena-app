namespace Siena.Api.Configuration;

public static class CorsConfiguration
{
    public const string PolicyName = "SienaCors";

    public static IServiceCollection AddSienaCors(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var allowedOrigins = configuration
            .GetSection("Siena:Cors:AllowedOrigins")
            .Get<string[]>() ?? ["http://localhost:3000", "http://localhost:8081"];

        services.AddCors(options =>
        {
            options.AddPolicy(PolicyName, policy =>
            {
                policy
                    .WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        return services;
    }

    public static WebApplication UseSienaCors(this WebApplication app)
    {
        app.UseCors(PolicyName);

        return app;
    }
}
