using System.Diagnostics;
using Inventory.Application.Common.Exceptions;
using Inventory.Domain.Exceptions;
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
        int statusCode;
        string title;
        string detail;

        if (exception is AppException appException)
        {
            logger.LogWarning(
                appException,
                "Application exception: {ExceptionType} TraceId: {TraceId}",
                appException.GetType().Name,
                httpContext.TraceIdentifier);

            statusCode = appException.StatusCode;
            title = statusCode == 404 ? "Resource not found" : "Application error";
            detail = appException.PublicMessage;
        }
        else if (exception is DomainException domainException)
        {
            logger.LogWarning(
                domainException,
                "Domain exception: {ExceptionType} TraceId: {TraceId}",
                domainException.GetType().Name,
                httpContext.TraceIdentifier);

            statusCode = StatusCodes.Status400BadRequest;
            title = "Domain rule violated";
            detail = domainException.Message;
        }
        else
        {
            logger.LogError(
                exception,
                "Unhandled exception occurred. TraceId: {TraceId}",
                httpContext.TraceIdentifier);

            statusCode = StatusCodes.Status500InternalServerError;
            title = "An unexpected error occurred";
            detail = "An internal server error occurred. Please contact support with the trace id.";
        }

        httpContext.Response.StatusCode = statusCode;

        string traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        ProblemDetails problemDetails = new()
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path,
            Extensions =
            {
                ["traceId"] = traceId,
            },
        };

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails,
        });
    }
}
