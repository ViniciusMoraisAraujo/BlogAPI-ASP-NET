using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace BlogAs.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in server. Please try again.", ex.Message );
            await HandleExceptionAsync(context, ex);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var (statusCode, message, errorCode) = exception switch
        {
            ArgumentNullException => (StatusCodes.Status400BadRequest, "Internal error", "09X0"),
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Internal error,", "09X1"),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Internal error ","09X3"),
            DbUpdateException => (StatusCodes.Status409Conflict, "Internal error - ", "09X4"),
            _ => (StatusCodes.Status500InternalServerError, "Internal error - ", "09X2")
        };
        
        context.Response.StatusCode = statusCode;
        var response = new
        {
            StatusCode = statusCode,
            Message = message,
            ErrorCode = errorCode,
            Details = _env.IsDevelopment() ? exception.ToString() : null,
            StackTrace = _env.IsDevelopment() ? exception.StackTrace : null,
        };
        await context.Response.WriteAsJsonAsync(response);
    }
}