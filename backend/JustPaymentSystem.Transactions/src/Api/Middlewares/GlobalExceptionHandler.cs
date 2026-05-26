using Application.Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Text.Json;

namespace Api.Middlewares;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.ContentType = "application/json";

        httpContext.Response.StatusCode = exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            ValidationException => StatusCodes.Status400BadRequest,
            JsonException => StatusCodes.Status400BadRequest,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

        var showGenericMessage = exception is JsonException;

        logger.LogError($"Something went wrong: {exception.Message}");

        ValidationException? validationException = exception as ValidationException;
        if (validationException is not null)
        {
            var errors = validationException.Errors
            .GroupBy(g => g.ErrorCode)
            .ToDictionary(
                group => group.Key,                                       
                group => group.Select(e => e.ErrorMessage).ToArray()      
            );

            var problemDetails = new HttpValidationProblemDetails()
            {
                Title = "Validation Failed",
                Status = StatusCodes.Status400BadRequest,
                Detail = "One or more validation errors occurred.",
                Instance = httpContext.Request.Path
            };

            var json = JsonSerializer.Serialize(problemDetails);
            await httpContext.Response.WriteAsync(json);
            return false;

        }
        await httpContext.Response.WriteAsJsonAsync(new
        {
            message = showGenericMessage
                ? "The request payload is invalid or malformed."
                : exception.Message
        }, cancellationToken);

        return true;
    }
}
