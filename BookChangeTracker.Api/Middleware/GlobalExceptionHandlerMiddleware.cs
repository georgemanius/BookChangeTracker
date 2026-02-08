using BookChangeTracker.Api.Models.Responses;
using BookChangeTracker.Application.Models;

namespace BookChangeTracker.Api.Middleware;

public class GlobalExceptionHandlerMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception)
        {
            // TODO: Add structured logging here to log exception details
            // TODO: Implement specific exception type mapping to respective error codes and HTTP status codes
            
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            
            var response = new ErrorResponse(
                ErrorCodes.InternalServerError,
                "An internal server error occurred. Please try again later."
            );
            
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
