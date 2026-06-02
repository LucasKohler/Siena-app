using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Siena.Infrastructure.Persistence;

namespace Siena.Api.Tests;

public sealed class SienaWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly SqliteConnection _connection = new($"Data Source=siena-tests-{Guid.NewGuid():N};Mode=Memory;Cache=Shared");

    public SienaWebApplicationFactory()
    {
        _connection.Open();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.UseSetting("ConnectionStrings:Default", _connection.ConnectionString);
        builder.UseSetting("Persistence:Provider", "Sqlite");
        builder.UseSetting("Jwt:Issuer", "siena-test");
        builder.UseSetting("Jwt:Audience", "siena-test");
        builder.UseSetting("Jwt:SigningKey", "test-signing-key-at-least-32-characters-long");
        builder.UseSetting("Jwt:AccessTokenMinutes", "60");
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);
        InitializeDatabase(host.Services);

        return host;
    }

    private void InitializeDatabase(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<SienaDbContext>();

        dbContext.Database.EnsureCreated();

        if (!dbContext.Users.Any())
        {
            DatabaseSeeder.SeedAsync(dbContext).GetAwaiter().GetResult();
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _connection.Dispose();
        }

        base.Dispose(disposing);
    }
}
