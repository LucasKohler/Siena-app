using Siena.Api.Configuration;
using Siena.Api.Endpoints;
using Siena.Application;
using Siena.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddSienaAuth(builder.Configuration)
    .AddSienaCors(builder.Configuration)
    .AddSienaOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.Services.ApplyMigrationsAndSeedAsync(app.Configuration);
}

app.UseSienaCors();
app.UseSienaAuth();

app.MapGet("/", () => Results.Ok(new
{
    service = "siena-api",
    status = "ok"
}))
.WithName("GetApiRoot")
.WithTags("System");

app.MapSienaOpenApi();
app.MapHealthEndpoints();
app.MapAuthEndpoints();
app.MapTrainingEndpoints();
app.MapEventEndpoints();
app.MapAdminEndpoints();
app.MapVideoEndpoints();

app.Run();

public partial class Program;
