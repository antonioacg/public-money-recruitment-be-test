using HealthChecks.UI.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;

namespace VacationRental.Api.Extensions;

public class HealthCheckDocumentFilter : IDocumentFilter
{
    private readonly IApiVersionDescriptionProvider _provider;

    public HealthCheckDocumentFilter(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var clientClosedRequestResponses = swaggerDoc.Paths.SelectMany(p => p.Value.Operations)
            .SelectMany(o => o.Value.Responses).Where(r => r.Key is "499").Select(r => r.Value);
        foreach (var response in clientClosedRequestResponses) response.Description = "Client Closed Request";

        var lastVersion = _provider.ApiVersionDescriptions.OrderBy(d => d.ApiVersion).Last();

        // Add HealthChecks only on latest version of the api
        if (swaggerDoc.Info.Version != lastVersion.GroupName) return;

        var uiHealthCheckReportSchema = context.SchemaGenerator.GenerateSchema(typeof(UIHealthReport), context.SchemaRepository);

        var operation = new OpenApiOperation();
        operation.Tags.Add(new OpenApiTag { Name = "HealthCheck" });

        var content = new Dictionary<string, OpenApiMediaType>
        {
            {
                MediaTypeNames.Application.Json,
                new OpenApiMediaType { Schema = uiHealthCheckReportSchema }
            }
        };

        operation.Responses.Add(StatusCodes.Status200OK.ToString(),
            new OpenApiResponse
            {
                Content = content,
                Description = "Success"
            });

        operation.Responses.Add(StatusCodes.Status206PartialContent.ToString(),
            new OpenApiResponse
            {
                Content = content,
                Description = "PartialContent"
            });

        operation.Responses.Add(StatusCodes.Status500InternalServerError.ToString(),
            new OpenApiResponse
            {
                Content = content,
                Description = "Server Error"
            });

        var pathItem = new OpenApiPathItem();
        pathItem.AddOperation(OperationType.Get, operation);

        swaggerDoc.Paths.AddHealthChecksPath(pathItem);
    }
}
