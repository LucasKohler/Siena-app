namespace Siena.Api.Endpoints;

public static class AuthorizationExtensions
{
    public static TBuilder RequireStaff<TBuilder>(this TBuilder builder)
        where TBuilder : IEndpointConventionBuilder
    {
        return builder.RequireAuthorization("Staff");
    }
}
