using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Siena.Application.Auth;
using Siena.Domain.Users;

namespace Siena.Api.Configuration;

public static class AuthConfiguration
{
    public static IServiceCollection AddSienaAuth(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

        var jwtSection = configuration.GetSection("Jwt");
        var issuer = jwtSection["Issuer"] ?? string.Empty;
        var audience = jwtSection["Audience"] ?? string.Empty;
        var signingKey = jwtSection["SigningKey"] ?? string.Empty;

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("Staff", policy =>
                policy.RequireRole(nameof(UserRole.Admin), nameof(UserRole.Coach)));
        });

        return services;
    }

    public static WebApplication UseSienaAuth(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}
