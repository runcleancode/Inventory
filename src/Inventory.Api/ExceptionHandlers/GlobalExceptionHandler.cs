using Inventory.Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.ExceptionHandlers;

public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(
            exception,
            "Unhandled exception occured. TraceId: {TraceId}",
            httpContext.TraceIdentifier);

        int statusCode = exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError,
        };

        httpContext.Response.StatusCode = statusCode;

        ProblemDetails problemDetails = new()
        {
            Status = statusCode,
            Title = statusCode == StatusCodes.Status404NotFound
                ? "Resource not found"
                : "An unexpected error occured",
            Detail = statusCode == StatusCodes.Status404NotFound
                ? exception.Message
                : "An internal server error occured. Please contact support with the trace id.",
            Instance = httpContext.Request.Path,
            Extensions =
            {
                ["traceId"] = httpContext.TraceIdentifier,
            },
        };

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });
    }
}
