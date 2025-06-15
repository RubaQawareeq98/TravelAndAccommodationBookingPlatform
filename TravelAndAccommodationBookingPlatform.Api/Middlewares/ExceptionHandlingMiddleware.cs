using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;

namespace TravelAndAccommodationBookingPlatform.Api.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message) = exception switch
        {
            EmailAlreadyExistsException => (StatusCodes.Status409Conflict, exception.Message),
            NotFoundException => (StatusCodes.Status404NotFound, exception.Message),
            ArgumentException => (StatusCodes.Status400BadRequest, exception.Message),
            
            DbUpdateException { InnerException: SqlException { Number: 547 } } =>
                (StatusCodes.Status400BadRequest, "Invalid foreign key reference. A related entity does not exist."),

            DbUpdateException => (StatusCodes.Status500InternalServerError, "A database update error occurred."),
            
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
        };

        context.Response.StatusCode = statusCode;

        var response = new
        {
            error = new
            {
                message,
                statusCode
            }
        };

        return context.Response.WriteAsJsonAsync(response);
    }
}
