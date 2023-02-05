using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Diagnostics.CodeAnalysis;

namespace VacationRental.Api.Extensions;

[ExcludeFromCodeCoverage]
public class OperationCanceledExceptionFilter : ExceptionFilterAttribute
{
    public const int Status499ClientClosedRequest = 499;

    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is not OperationCanceledException) return;
        context.ExceptionHandled = true;
        context.Result = new StatusCodeResult(Status499ClientClosedRequest);
    }
}
