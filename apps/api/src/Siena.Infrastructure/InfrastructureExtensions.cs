using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Siena.Application.Auth;
using Siena.Application.Events;
using Siena.Application.Trainings;
using Siena.Application.Videos;
using Siena.Infrastructure.Auth;
using Siena.Infrastructure.Persistence;

namespace Siena.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var provider = configuration["Persistence:Provider"] ?? "Postgres";
        var connectionString = configuration.GetConnectionString("Default")
            ?? "Host=localhost;Port=5432;Database=siena;Username=siena;Password=siena_dev";

        services.AddDbContext<SienaDbContext>(options =>
        {
            if (string.Equals(provider, "Sqlite", StringComparison.OrdinalIgnoreCase))
            {
                options.UseSqlite(connectionString);
            }
            else
            {
                options.UseNpgsql(connectionString);
            }
        });

        services.AddScoped<IUserRepository, EfUserRepository>();
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IAttendanceRepository, EfAttendanceRepository>();
        services.AddScoped<IEventRepository, EfEventRepository>();
        services.AddScoped<IVideoRepository, EfVideoRepository>();

        return services;
    }

    public static async Task ApplyMigrationsAndSeedAsync(
        this IServiceProvider services,
        IConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        await using var scope = services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<SienaDbContext>();
        var provider = configuration["Persistence:Provider"] ?? "Postgres";

        if (string.Equals(provider, "Sqlite", StringComparison.OrdinalIgnoreCase))
        {
            await dbContext.Database.EnsureCreatedAsync(cancellationToken);
        }
        else
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
        }

        await DatabaseSeeder.SeedAsync(dbContext, cancellationToken);
    }
}
