using Inventory.Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.ExceptionHandlers;

internal sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        bool isAppException = exception is AppException;

        if (isAppException)
        {
            logger.LogWarning(
                exception,
                "Application exception: {ExceptionType} TraceId: {TraceId}",
                exception.GetType().Name,
                httpContext.TraceIdentifier);
        }
        else
        {
            logger.LogError(exception,
            "Unhandled exception occured. TraceId: {TraceId}",
            httpContext.TraceIdentifier);
        }

        int statusCode = exception switch
        {
            AppException appException => appException.StatusCode,
            _ => StatusCodes.Status500InternalServerError,
        };

        httpContext.Response.StatusCode = statusCode;

        ProblemDetails problemDetails = new()
        {
            Status = statusCode,
            Title = statusCode switch
            {
                StatusCodes.Status404NotFound => "Resource not found",
                StatusCodes.Status400BadRequest => "Bad request",
                _ => "An unexpected error occured",
            },
            Detail = isAppException
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
            ProblemDetails = problemDetails,
        });
    }
}
