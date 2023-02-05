using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace VacationRental.Api.Extensions;

public class OperationCanceledExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is not OperationCanceledException) return;
        context.ExceptionHandled = true;
        context.Result = new StatusCodeResult(499); // Client Closed Request
    }
}
