using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace GlobalSolution2.Endpoints;

public static class HealthCheckEndpoints
{
    public static void MapHealthCheckEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapHealthChecks("/api/health-checks/", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        })
        .WithName("GetHealthCheck")
        .WithSummary("Retorna o Health Check da aplicação")
        .WithDescription("Retorna o Health Check da API.")
        .WithTags("HealthCheck");

        app.MapHealthChecks("/api/health-checks/database", new HealthCheckOptions
        {
            Predicate = (check) => check.Tags.Contains("db"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        })
        .WithName("GetDatabaseHealthCheck")
        .WithSummary("Retorna o Health Check do banco de dados")
        .WithDescription("Retorna o Health Check do banco de dados Oracle.")
        .WithTags("HealthCheck");
    }
}