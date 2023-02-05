using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace VacationRental.Api.Extensions;

public static class HealthChecksExtensions
{
    private const string HealthCheckPath = "/health";

    public static IServiceCollection AddApplicationHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks();
        services.Configure<SwaggerGenOptions>(opts => opts.DocumentFilter<HealthCheckDocumentFilter>());

        return services;
    }

    public static void AddHealthChecksPath(this OpenApiPaths paths, OpenApiPathItem pathItem)
    {
        paths.Add(HealthCheckPath, pathItem);
    }

    public static void MapApplicationHealthChecks(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks(HealthCheckPath);
    }

    public static IApplicationBuilder UseApplicationHealthChecks(this IApplicationBuilder app)
    {
        app.UseHealthChecks(HealthCheckPath, new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status206PartialContent,
                [HealthStatus.Unhealthy] = StatusCodes.Status500InternalServerError,
            }
        });
        return app;
    }
}
